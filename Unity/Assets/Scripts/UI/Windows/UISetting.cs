using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Proto;

namespace Assets.Scripts.UI.Windows
{
    partial class UISetting
    {

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) => {
                this.HideWindow();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            lb_version.text = string.Format(LanguageManager.Singleton["UI_Setting_version"],
                (int)GameVersion.Master, (int)GameVersion.Major, (int)GameVersion.Develop);
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}