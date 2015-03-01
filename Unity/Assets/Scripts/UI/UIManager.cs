using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI
{
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
        public void ShowWindow() { }
        public void ShowAsChildWindow(UIWindow window)
        { }
        public void ShowAsDialogWindow(bool showMask) { }
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

        }
        public void OnLateUpdate() {
        
        }
        public void OnUpdateUIData() { }
        public void OnUpdateUIData(params string[] keys) { }

        public void OnUpdateUIData<T>() where T : UIWindow
        {
            OnUpdateUIData(typeof(T).Name);
        }
    }
}
