using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UICastlePanel
    {

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_market.OnMouseClick((s, e) => {
                //this.HideWindow();
                var ui = UIManager.Singleton.CreateOrGetWindow<UIShop>();
                ui.ShowAsChildWindow(this);
            });

            bt_bar.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UITavern>();
                ui.ShowAsChildWindow(this);
            });

            bt_produce.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIProducePanel>();
                ui.ShowAsChildWindow(this);
            });
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