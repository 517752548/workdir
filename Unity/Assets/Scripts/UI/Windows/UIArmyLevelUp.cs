using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIArmyLevelUp
    {
        public class formStartGridTableModel : TableItemModel<formStartGridTableTemplate>
        {
            public formStartGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }
        public class toStartGridTableModel : TableItemModel<toStartGridTableTemplate>
        {
            public toStartGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            UIMessageBox.ShowMessage("", "", () => { this.HideWindow(); }, () => { this.HideWindow(); });

        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}