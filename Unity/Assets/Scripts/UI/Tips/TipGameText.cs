using UnityEngine;
using System.Collections;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Tips
{
    [Tip("TipGameText")]
    public class TipGameText : UITip
    {
        private UILabel text;

        private float delay = -1f;
        public string Text
        {
            set { text.text = value; }
            get
            {
                return text.text;
            }
        }
        public override void OnCreate()
        {
            this.text = this.Root.transform.FindChild<UILabel>("Label");
            delay = Time.time + 0.38f;
        }

        public override void OnDraw()
        {
            base.OnDraw();
            if (delay > 0f)
            {
                if (Time.time > delay)
                {
                    text.transform.localScale = Vector3.one;
                    delay = -1f;
                    var t = this.text.GetComponent<TweenScale>();
                    if (t != null)
                    {
                        GameObject.DestroyObject(t);
                    }
                }
            }
        }

    }
}