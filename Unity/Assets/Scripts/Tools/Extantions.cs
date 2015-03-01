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
                for (var i = 0; i < child.childCount; i++)
                {
                    var current = child.GetChild(i);
                    var result = FindInAllChild<T>(current, name);
                    if (result != null) return result;
                }

                return null;
            }
            else
            {

                return child.GetComponent<T>();
            }
        }

    }
}
