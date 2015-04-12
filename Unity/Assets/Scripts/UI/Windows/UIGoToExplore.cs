using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIGoToExplore
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            base.InitModel();
            this.bt_right.OnMouseClick((s, e) =>
            {
                this.HideWindow();
                var ui = UIManager.Singleton.CreateOrShowWindow<UICastlePanel>();
                ui.ShowWindow();
            });
            this.bt_left.ActiveSelfObject(false);
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            //onupdateui data
        }

        public override void OnLanguage()
        {
            base.OnLanguage();
            this.lb_army_size_lb.text = LanguageManager.Singleton["UI_GOTOEX_ARMY_LB"];
            this.lb_package_size_lb.text = LanguageManager.Singleton["UI_GOTOEX_PACKAGE_LB"];
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}