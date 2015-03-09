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
    }
    public interface ITipRender
    {
        void RenderTip(GameObject tip);
    }

    public class UITipManager :Tools.XSingleton<UITipManager>
    {
        public void Init(ITipRender render)
        {
            Render = render;
        }

        public ITipRender Render { private set; get; }

        private Dictionary<int, UITip> _tips = new Dictionary<int, UITip>();

        public void DrawTip(string name)
        {

        }

        public T CreateTip<T>(int key) where T : UITip, new ()
        {
            var atts = typeof(T).GetCustomAttributes(typeof(TipAttribute), false) as TipAttribute[];
            if(atts.Length>0)
            {
                var name = atts[0].Name;
                var res = Tools.ResourcesManager.Singleton.LoadResources<GameObject>(name);
                var ui = GameObject.Instantiate(res, Vector3.zero, Quaternion.identity) as GameObject;
                this.Render.RenderTip(ui);
                var tip = new T();
                tip.Root = ui;
                tip.Key = key;
                tip.OnCreate();
                return tip;
            }

            return null;
        }
    }
}
