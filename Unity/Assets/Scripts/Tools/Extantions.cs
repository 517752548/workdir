using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    public static class Extantions
    {
        public static void ActiveSelfObject(this Component componet, bool actived)
        {
            componet.gameObject.SetActive(actived);
        }

        public static T FindChild<T>(this Transform trans, string name) where T : Component
        {
            return FindInAllChild<T>(trans, name);
        }
        private static T FindInAllChild<T>(Transform trans, string name) where T : Component
        {
            var child = trans.FindChild(name);
            if (child == null)
            {
                for (var i = 0; i < trans.childCount; i++)
                {
                    var current = trans.GetChild(i);
                    var result = FindInAllChild<T>(current, name);
                    if (result != null) return result;
                }
                return null;
            }
            else
                return child.GetComponent<T>();
        }

        public static UIMouseClick OnMouseClick(this Component comp, EventHandler<UIEventArgs> handler)
        {
            return OnMouseClick(comp, handler, null);
        }

        public static UIMouseClick OnMouseClick(this Component comp, EventHandler<UIEventArgs> handler, object userState)
        {
            UIMouseClick click = comp.GetComponent<UIMouseClick>(); ;
            if (click == null)
            {
                click = comp.gameObject.AddComponent<UIMouseClick>();
            }
            click.Click = handler;
            click.UserState = userState;
            return click;
        }
    }
}
