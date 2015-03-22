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
    public interface IUIRender
    {
        void Render(GameObject uiRoot);
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
            State = WindowStates.Normal;
            Root = root;
            OnCreate();
        }
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
        public virtual void OnUpdate() { }
        public void ShowWindow()
        {
            OnShow();
            AfterShow();
            State = WindowStates.Show;
        }
        public void ShowAsChildWindow(UIWindow window)
        {
            ShowWindow();
        }
        public void ShowAsDialogWindow(bool showMask)
        {
            ShowWindow();
        }

        public void HideWindow() 
        {
            OnHide();
            AfterShow();

            State = WindowStates.Hide;

        }

        public WindowStates State { private set; get; }
        
    }


    public class UIManager : Tools.XSingleton<UIManager>
    {
        public IUIRender Render { private set; get; }

        
        private Dictionary<string, UIWindow> _windows { set; get; }

        public void Init(IUIRender render)
        {
            Render = render;
            _windows = new Dictionary<string, UIWindow>();
        }
        public void OnUpdate()
        {
            foreach(var i in _windows)
            {
                if(i.Value.State == WindowStates.Hiding || i.Value.State == WindowStates.Show || i.Value.State == WindowStates.Showing )
                    i.Value.OnUpdate();
            }
        }

        public void OnLateUpdate() {
            foreach (var i in _windows)
            {
                if ( i.Value.State == WindowStates.Show )
                    i.Value.OnLateUpdate();
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

        public T CreateOrShowWindow<T>() where T: UIWindow, new ()
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
                var uiRoot = GameObject.Instantiate<GameObject>(resourse);             
                this.Render.Render(uiRoot);
                ui.Init(uiRoot);
                _windows.Add(typeof(T).Name, ui);

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
    }
}
