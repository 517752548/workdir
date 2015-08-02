using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI
{
    public enum WindowStates
    {
        Normal,
        Show,
        Showing,
        Hide,
        Hiding
    }

    public enum ShowModel
    {
        Normal,
        Dialog,
        Children
    }

    public interface IUIRender
    {
        void Render(GameObject uiRoot);
        void ShowOrHideBack(bool show);
    }

    public interface IEffect
    {
        bool ShowEffect();
        bool HideEffect();
    }
    public class UIWindowAttribute : Attribute
    {
        public UIWindowAttribute(string resources)
        {
            Resource = resources;
        }
        public string Resource { set; get; }
    }

    //[UIWindow("default")]
    public class UIWindow
    {
        private GameObject Root { set; get; }
        public void Init(GameObject root)
        {
            WindowEffect = root.GetComponent<IEffect>();
            State = WindowStates.Normal;
            Root = root;
            OnCreate();
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
        public virtual void OnPreScecondUpdate() { }
        public void ShowWindow()
        {
            this.Depth = UIManager.Singleton.MaxDepth + 1;
            this.Root.SetActive(true);
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

            OnShow();
            AfterShow();
            State = WindowStates.Show;
        }
        public void ShowAsChildWindow(UIWindow window)
        {
            Model = ShowModel.Children;
            parent = window;
            if (parent != null)
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
            OnHide();
            AfterHide();
            if (!CanDestoryWhenHide)
                this.Root.SetActive(false);
            State = WindowStates.Hide;
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
                }
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
        }

        private float lastTime = 0f;
        public void OnUpdate()
        {
            foreach(var i in _windows)
            {
                if(i.Value.IsVisable )
                    i.Value.OnUpdate();
            }

            if(lastTime+1<Time.time)
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
        public void OnLateUpdate() {
            foreach (var i in _windows)
            {
                if (i.Value.State == WindowStates.Show)
                {
                    i.Value.OnLateUpdate();
                }
                if(i.Value.State== WindowStates.Hide)
                {
                    if (i.Value.CanDestoryWhenHide) 
                    {
                        _deletes.Enqueue(i.Value);
                    }
                }
            }

            while(_deletes.Count>0)
            {
                var ui = _deletes.Dequeue();
                if (_windows.ContainsKey(ui.Key))
                {
                    _windows.Remove(ui.Key);
                    ui.Destory();
                }
            }
        }
        public void OnUpdateUIData()
        {
            foreach (var i in _windows)
            {
                if (i.Value.State == WindowStates.Show)
                    i.Value.OnUpdateUIData();
            }
        }
        public void OnUpdateUIData(params string[] keys)
        {
            foreach(var i in keys)
            {
                UIWindow w;
                if (_windows.TryGetValue(i, out w))
                    if (w.State == WindowStates.Show)
                        w.OnUpdateUIData();
            }
        }

        public void OnUpdateUIData<T>() where T : UIWindow
        {
            OnUpdateUIData(typeof(T).Name);
        }

        public T CreateOrGetWindow<T>() where T: UIWindow, new ()
        {
            var key = typeof(T).Name;
            T window;
            if(_windows.ContainsKey(key))
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
                this.Render.Render(uiRoot);
                ui.Init(uiRoot);
                ui.Key = typeof(T).Name;
                _windows.Add(ui.Key, ui);

                return ui;
            }
            else
                return default(T);
        }

       
        public delegate bool FindContion<T>(T item) where T : UIWindow;

        public void Each<T>(FindContion<T> cond) where T:UIWindow
        {
            foreach(var i in _windows)
            {
                if (i.Value.State != WindowStates.Show) continue;
                if (!(i.Value is T)) continue; 
                if (cond(i.Value as T)) break;
            }
        }

        public T GetUIWindow<T>() where T:UIWindow
        {
            var type = typeof(T).Name;
            UIWindow t;
            if(this._windows.TryGetValue(type,out t))
            {
                return t as T;
            }

            return null;
        }
    }
}
