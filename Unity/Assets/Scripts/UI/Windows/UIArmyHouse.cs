using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIArmyHouse
    {

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
            Bt_ArmyLvlUp.OnMouseClick((s, e) => {
                UIArmyHouseSelect.Show();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}