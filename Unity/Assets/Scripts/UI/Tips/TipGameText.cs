using UnityEngine;
using System.Collections;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Tips
{
    [Tip("TipGameText")]
    public class TipGameText : UITip
    {
        private UILabel text;

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
        }
    }
}