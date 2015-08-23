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
                ui.ShowWindow();
            });

            bt_bar.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UITavern>();
                ui.ShowWindow();
            });

            bt_produce.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIProducePanel>();
                ui.ShowWindow();
            });

            Title.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UISetting>();
                ui.ShowWindow();
            });

            bt_make.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIMake>();
                ui.ShowWindow();
            });

            bt_Coin.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIPaymentShop>();
                ui.ShowWindow();
            });

            bt_battle.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIGoToExplore>();
                ui.ShowWindow();
            });

            bt_contruct.OnMouseClick((s, e) => {
                UIStructureBuilding.Show();
            });

            bt_train.OnMouseClick((s, e) => {
                UIArmyHouse.Show();
            });

            bt_Package.OnMouseClick((s, e) => {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIBag>();
                ui.ShowAsChildWindow(this,false);
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