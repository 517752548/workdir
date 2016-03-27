using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Tools;
using System.Collections;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// UI当前状态
    /// </summary>
    public enum WindowStates
    {
        /// <summary>
        /// 默认
        /// </summary>
        Normal,
        /// <summary>
        /// 显示
        /// </summary>
        Show,
        /// <summary>
        /// 播放打开效果
        /// </summary>
        Showing,
        /// <summary>
        /// 已经关闭
        /// </summary>
        Hide,
         /// <summary>
         ///  关闭中
         /// </summary>
        Hiding
    }

    /// <summary>
    /// UI打开模式
    /// </summary>
    public enum ShowModel
    {
        /// <summary>
        /// 一般方式
        /// </summary>
        Normal,
        /// <summary>
        /// 互斥模式
        /// </summary>
        Dialog,
        /// <summary>
        /// 子窗体
        /// </summary>
        Children
    }

    /// <summary>
    /// UI的渲染
    /// </summary>
    public interface IUIRender
    {
        void Render(GameObject uiRoot);
        void ShowMessage(string message);
		void ShowInfo (string message, float delay);
        void ShowOrHideMessage(bool show);
    }

    /// <summary>
    /// UI的显示和隐藏效果
    /// </summary>
    public interface IEffect
    {
        bool ShowEffect();
        bool HideEffect();
    }

    /// <summary>
    /// 描述UI和资源关系
    /// </summary>
    public class UIWindowAttribute : Attribute
    {
        public UIWindowAttribute(string resources)
        {
            Resource = resources;
        }
        public string Resource { set; get; }
    }

    /// <summary>
    /// UI中的window父类
    /// </summary>
    public class UIWindow
    {
		public UIWindow()
		{
			CanDestoryWhenHide = true;
		}
		public GameObject Root { private set;  get; }
        public void Init(GameObject root)
        {
            WindowEffect = root.GetComponent<IEffect>();
            State = WindowStates.Normal;
            Root = root;
			Mate = Root.AddComponent<UIResourcesMate> ();
            OnCreate();
        }
		private UIResourcesMate Mate{ set; get; }

		public void StartCoroutine(IEnumerator coroutine)
		{
			Mate.StartCoroutine (coroutine);
		}

		public void StopAllCoroutines(){
			Mate.StopAllCoroutines ();
		}

        private IEffect WindowEffect;
        public bool CanDestoryWhenHide { set; get; }
        public T FindChild<T>(string name) where T : Component
        {
            return Root.transform.FindChild<T>(name);
        }
        public virtual void OnCreate() { }
        public virtual void OnShow() { }
        public virtual void AfterShow() { }
        public virtual void OnHide() { }
        public virtual void AfterHide() { }
        public virtual void OnLanguage() { }
        public virtual void OnUpdateUIData() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnUpdate()
        {
            if (WindowEffect == null) return;
            switch (State)
            {
                case WindowStates.Hiding:
                    if (WindowEffect.HideEffect())
                    {
                        DoHide();
                    }
                    break;
                case WindowStates.Showing:
                    if (WindowEffect.ShowEffect())
                    {
                        DoShow();
                    }
                    break;
            }
        }
        public bool NoCollider = false;
        public virtual void OnPreScecondUpdate() { }
        public void ShowWindow()
        {
            
            this.Root.SetActive(true);
			this.Depth = UIManager.Singleton.MaxDepth + 1;
            if (!NoCollider)
                NGUITools.AddWidgetCollider(this.Root);
            if (WindowEffect != null)
            {
                State = WindowStates.Showing;
            }
            else
            {
                DoShow();
            }
        }
        private void DoShow()
        {
            State = WindowStates.Show;
            OnShow();
            AfterShow();
            
			DataManagers.GuideManager.Singleton.ShowGuild ();
        }
        public void ShowAsChildWindow(UIWindow window, bool hideParent = true)
        {
            Model = ShowModel.Children;
            parent = window;
            if (parent != null && hideParent)
            {
                parent.PositionShowOrHide(false);
            }
            ShowWindow();

        }
        public void ShowAsDialogWindow(bool showMask)
        {
            Model = ShowModel.Dialog;
            ShowWindow();
        }

        private UIWindow parent;
        private void PositionShowOrHide(bool isShow)
        {
            var anchers = this.Root.transform.GetComponentsInChildren<UIAnchor>();
            foreach (var i in anchers)
            {
                i.enabled = isShow;
            }
            this.Root.transform.localPosition = isShow ? Vector3.zero : new Vector3(-100000, 0, 0);
        }

        public void HideWindow()
        {
			Mate.StopAllCoroutines ();
            if (Model == ShowModel.Children)
            {
                if (parent != null)
                    parent.PositionShowOrHide(true);
                parent = null;
            }
            if (WindowEffect != null)
            {
                State = WindowStates.Hiding;
            }
            else
            {
                DoHide();
            }
        }
        private void DoHide()
        {
            State = WindowStates.Hide;
            OnHide();
            AfterHide();
            if (!CanDestoryWhenHide)
                this.Root.SetActive(false);
            
        }

        public WindowStates State { private set; get; }

        public ShowModel Model { private set; get; }

        public string Key { get; set; }

        internal void Destory()
        {
            OnDestory();
            GameObject.Destroy(this.Root);
        }

        public virtual void OnDestory() { }

        public int Depth
        {
            get
            {
                var uiPanels = this.Root.transform.FindAllChilds<UIPanel>();
                int depth = 0;
                foreach (var i in uiPanels)
                {
                    if (i.depth > depth)
                    {
                        depth = i.depth;
                    }
                }
                return depth;
            }
            set
            {
                var rootPanel = this.Root.GetComponent<UIPanel>();
                if (rootPanel == null) return;
                var rootDepth = rootPanel.depth;

                var uiPanels = this.Root.transform.FindAllChilds<UIPanel>();
                foreach (var i in uiPanels)
                {
                    i.depth = (i.depth - rootDepth) + value;
					i.Refresh ();
                }
				rootPanel.Refresh ();
            }
        }

        public bool IsVisable
        {
            get
            {
                return State == WindowStates.Showing || State == WindowStates.Hiding || State == WindowStates.Show;
            }
        }
    }

    /// <summary>
    /// UI管理者
    /// </summary>
    public class UIManager : Tools.XSingleton<UIManager>
    {
        public IUIRender Render { private set; get; }

        public int MaxDepth
        {
            get
            {
                int depth = 0;
                foreach (var i in _windows)
                {
                    if (!i.Value.IsVisable) continue;
                    var t = i.Value.Depth;
                    if (t > depth)
                        depth = t;
                }
                return depth;
            }
        }

        private Dictionary<string, UIWindow> _windows { set; get; }

        public void Init(IUIRender render)
        {
            Render = render;
            _windows = new Dictionary<string, UIWindow>();

			//LoadAllResources
			var windows = typeof(UIManager).Assembly.GetTypes().Where(t=> t.IsSubclassOf(typeof(UIWindow))).ToList();
			foreach (var i in windows) {
				var attris = i.GetCustomAttributes(typeof(UIWindowAttribute),false) as UIWindowAttribute[];
				if (attris == null || attris.Length == 0)
					continue;
				Resources.Load ("UI/" + attris [0].Resource);
			}
        }

        private float lastTime = 0f;
        public void OnUpdate()
        {
            foreach (var i in _windows)
            {
                if (i.Value.IsVisable)
                    i.Value.OnUpdate();
            }

            if (lastTime + 1 < Time.time)
            {
                lastTime = Time.time;
                foreach (var i in _windows)
                {
                    if (i.Value.State == WindowStates.Show)
                        i.Value.OnPreScecondUpdate();
                }
            }
        }

        private Queue<UIWindow> _deletes = new Queue<UIWindow>();
        public void OnLateUpdate()
        {
            foreach (var i in _windows)
            {
                if (i.Value.State == WindowStates.Show)
                {
                    i.Value.OnLateUpdate();
                }
                if (i.Value.State == WindowStates.Hide)
                {
                    if (i.Value.CanDestoryWhenHide)
                    {
                        _deletes.Enqueue(i.Value);
                    }
                }
            }

            while (_deletes.Count > 0)
            {
                var ui = _deletes.Dequeue();
                if (_windows.ContainsKey(ui.Key))
                {
                    _windows.Remove(ui.Key);
                    ui.Destory();
                }
            }
        }

        /// <summary>
        /// 通知UI刷新数据
        /// </summary>
        public void UpdateUIData()
        {
            foreach (var i in _windows)
            {
                if (i.Value.State == WindowStates.Show)
                    i.Value.OnUpdateUIData();
            }
        }

        private void UpdateUIData(params string[] keys)
        {
            foreach (var i in keys)
            {
                UIWindow w;
                if (_windows.TryGetValue(i, out w))
                    if (w.State == WindowStates.Show)
                        w.OnUpdateUIData();
            }
        }

        /// <summary>
        /// 通知指定UI刷新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UpdateUIData<T>() where T : UIWindow
        {
            UpdateUIData(typeof(T).Name);
        }

        /// <summary>
        /// 创建并且返回创建的UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateOrGetWindow<T>() where T : UIWindow, new()
        {
            var key = typeof(T).Name;
            T window;
            if (_windows.ContainsKey(key))
            {
                window = _windows[key] as T;
            }
            else
            {
                window = Create<T>();
            }

            return window;
        }

        private T Create<T>() where T : UIWindow, new()
        {
            var atts = typeof(T).GetCustomAttributes(typeof(UI.UIWindowAttribute), false) as UI.UIWindowAttribute[];
            if (atts.Length > 0)
            {
                var ui = new T();
                var resourse = Tools.ResourcesManager.Singleton.LoadResources<GameObject>("UI/" + atts[0].Resource);
                if (resourse == null) return default(T);
                var uiRoot = GameObject.Instantiate(resourse) as GameObject;
                uiRoot.name = "_UI_" + atts[0].Resource;
                this.Render.Render(uiRoot);
                ui.Init(uiRoot);
                ui.Key = typeof(T).Name;
                _windows.Add(ui.Key, ui);

                return ui;
            }
            else
                return default(T);
        }

        /// <summary>
        /// 提供给UI遍历的委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public delegate bool FindContion<T>(T item) where T : UIWindow;

        public void Each<T>(FindContion<T> cond) where T : UIWindow
        {
            foreach (var i in _windows)
            {
                if (i.Value.State != WindowStates.Show) continue;
                if (!(i.Value is T)) continue;
                if (cond(i.Value as T)) break;
            }
        }

        /// <summary>
        /// 获取UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUIWindow<T>() where T : UIWindow
        {
            var type = typeof(T).Name;
            UIWindow t;
            if (this._windows.TryGetValue(type, out t))
            {
                return t as T;
            }

            return null;
        }

        /// <summary>
        /// 通知所有ui刷新多语言
        /// </summary>
        public void LanguageChanaged()
        {

            foreach (var w in this._windows)
                if (w.Value.State == WindowStates.Show)
                    w.Value.OnLanguage();
        }
    }
}
