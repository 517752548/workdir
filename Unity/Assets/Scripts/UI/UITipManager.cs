using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TipAttribute:Attribute
    {
        public TipAttribute(string name) 
        {
            Name = name;
        }

        public string Name { set; get; }
    }

    public abstract class UITip
    {
        public int Key { set; get; }
        public abstract void OnCreate();
        public GameObject Root { set; get; }
        public bool PreUpdate { set; get; }
        public virtual void OnDraw() { }
    }
    public interface ITipRender
    {
        void RenderTip(GameObject tip);
    }

    public class UITipManager :Tools.XSingleton<UITipManager>
    {
        private class TipText
        {
            public float HideTime { set; get; }
            public float StartTime { set; get; }
            public string Text { set; get; }
            public int Key { set; get; }
            public bool CanHide
            {
                get
                {
                    return Time.time > HideTime + StartTime;
                }
            }

            private static int _index = 1;

            public static float NextTime = 0;
            public TipText()
            {
                if (_index == int.MaxValue)
                    _index = 1;
                _index++;
                Key = ("KEY_OF_ITEM" + _index).GetHashCode();
                if (NextTime + 0.5f > Time.time)
                    StartTime = NextTime + 0.5f;
                else
                    StartTime = Time.time;
                NextTime =  StartTime;
            }
        }
        public void Init(ITipRender render)
        {
            Render = render;
        }

        public ITipRender Render { private set; get; }

        private Dictionary<int, UITip> _tips = new Dictionary<int, UITip>();

        
        public void DrawNotify(string name)
        {
            _tipTexts.Add(new TipText() { HideTime = 2, Text = name});
        }

        private void DrawNotifyText(TipText text)
        {
            var tip = this[text.Key];
            if (tip == null)
            {
                var ttip = CreateTip<Tips.TipGameText>(text.Key);
                ttip.Text = text.Text;
                tip = ttip;
            }
            if (tip == null) return;
            DrawTip(tip);
        }

        private void DrawTip(UITip tip)
        {
            tip.PreUpdate = true;
            tip.OnDraw();
        }

        public UITip this[int key]
        {
            get
            {
                UITip tip;
                if (_tips.TryGetValue(key, out tip))
                    return tip;
                return null;
            }
        }

        public T CreateTip<T>(int key) where T : UITip, new ()
        {
            if (_tips.ContainsKey(key)) return null;

            var atts = typeof(T).GetCustomAttributes(typeof(TipAttribute), false) as TipAttribute[];
            if(atts.Length>0)
            {
                var name = atts[0].Name;
                var res = Tools.ResourcesManager.Singleton.LoadResources<GameObject>("Tip/"+name);
                var ui = GameObject.Instantiate(res, Vector3.zero, Quaternion.identity) as GameObject;
                this.Render.RenderTip(ui);
                var tip = new T();
                tip.Root = ui;
                tip.Key = key;
                tip.OnCreate();
                _tips.Add(key, tip);
                return tip;
            }

            return null;
        }

        public void OnLateUpdate() 
        {
            foreach(var i in _tips )
            {
                if (!i.Value.PreUpdate)
                {
                    _delTemp.Enqueue(i.Value);
                    continue;
                }
                i.Value.PreUpdate = false;
            }

            while(_delTemp.Count>0)
            {
                var temp = _delTemp.Dequeue();
                GameObject.Destroy(temp.Root);
                _tips.Remove(temp.Key);
            }
        }

        public void OnUpdate()
        {
            #region 显示提示
            for (var i = 0; i < _tipTexts.Count; i++)
            {
                var item = _tipTexts[i];
                if (item.StartTime > Time.time) continue;
                if (item.CanHide)
                {
                    _delTextTemp.Enqueue(item);
                    continue;
                }
                this.DrawNotifyText(item);
            }
            while (_delTextTemp.Count > 0)
            {
                _tipTexts.Remove(_delTextTemp.Dequeue());
            }
            #endregion
        }

        private List<TipText> _tipTexts = new List<TipText>();
        private Queue<TipText> _delTextTemp = new Queue<TipText>();

        private Queue<UITip> _delTemp = new Queue<UITip>();
    }

    public class UITipDrawer : Tools.XSingleton<UITipDrawer>
    {
        public void DrawNotify(string notify)
        {
            UITipManager.Singleton.DrawNotify(notify);
        }
    }
}
