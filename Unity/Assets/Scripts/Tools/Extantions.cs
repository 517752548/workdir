using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ExcelConfig;

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

        /// <summary>
        /// 获取所有子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static List<T> FindAllChilds<T>(this Transform trans) where T : Component
        {
            var list = new List<T>();
            GetAllChilds(trans, ref list);
            return list;
        }
        private static void GetAllChilds<T>(Transform trans, ref List<T> list) where T : Component
        {
            var t = trans.GetComponent<T>();
            if (t != null)
            {
                list.Add(t);
            }
            for (var i = 0; i < trans.childCount; i++)
            {
                GetAllChilds(trans.GetChild(i), ref list);
            }
        }
        public static UIMousePress OnMousePress(this Component comp, EventHandler<MousePressArgs> handler)
        {
            return OnMousePress(comp, handler, null);
        }

        public static UIMousePress OnMousePress(this Component comp, EventHandler<MousePressArgs> handler, object userState)
        {
            UIMousePress press = comp.GetComponent<UIMousePress>(); ;
            if (press == null)
            {
                press = comp.gameObject.AddComponent<UIMousePress>();
            }
            press.Press = handler;
            press.UserState = userState;
            return press;
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

        public static UIButton Disable(this UIButton button, bool disable)
        {
            var collier = button.GetComponent<Collider>();
            collier.enabled = !disable;
            button.SetState(disable ? UIButton.State.Disabled : UIButtonColor.State.Normal, true);
            return button;
        }

        public static void Text(this UIButton uiButton, string text)
        {
            var label = uiButton.transform.FindChild<UILabel>("Label");
            if (label == null) return;
            label.text = text;
        }

        public static string Text(this UIButton uiButton)
        {
            var label = uiButton.transform.FindChild<UILabel>("Label");
            if (label == null) return string.Empty;
            return label.text;
        }

        public static string ToDebugString(this MonsterConfig monster)
        {
            string format = "N:{0} L:{1} Res:{2} Damage:{3} Type:{4} Speed:{5} star:{6}";

            return string.Format(format, monster.Name, monster.Level, monster.ResName, monster.Damage, monster.Type, monster.Speed, monster.Star);
        }

        public static string ToDebugString(this SkillConfig skill)
        {
            string format = "N:{0} MType:{1} MNum:{2} MTarget:{9} Par1:{3} par2:{4} par3:{5} CD:{6} Status:{7} StatusTarget:{8}";
            return string.Format(format, skill.Name, (Proto.SkillDamageType)skill.MainEffectType, 
                skill.MainEffectNumber, skill.Pars1, skill.Pars2, skill.Pars3, skill.SkillCd, 
                (Proto.SkillEffectType)skill.StatusType,(Proto.SkillEffectTaget) skill.StatusTarget,
                (Proto.SkillEffectTaget) skill.MainEffectTarget);
        }
    }
}
