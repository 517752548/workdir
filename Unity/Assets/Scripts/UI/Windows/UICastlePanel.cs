using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    partial class UICastlePanel
    {

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_market.OnMouseClick((s, e) =>
            {
                //this.HideWindow();
                var ui = UIManager.Singleton.CreateOrGetWindow<UIShop>();
                ui.ShowWindow();
            });

            bt_bar.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UITavern>();
                ui.ShowWindow();
            });

            bt_produce.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIProducePanel>();
                ui.ShowWindow();
            });

            Title.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UISetting>();
                ui.ShowWindow();
            });

            bt_make.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIMake>();
                ui.ShowWindow();
            });

            bt_Coin.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIPaymentShop>();
                ui.ShowWindow();
            });

            bt_battle.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIGoToExplore>();
                ui.ShowWindow();
            });

            bt_contruct.OnMouseClick((s, e) =>
            {
                UIStructureBuilding.Show();
            });

            bt_train.OnMouseClick((s, e) =>
            {
                UIArmyHouseSelect.Show();
                //UIArmyHouse.Show();
            });

            bt_Package.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIBag>();
                ui.ShowAsChildWindow(this, false);
            });

            bt_gold.OnMouseClick((s, e) =>
            {
                var cd = DataManagers.GamePlayerManager.Singleton.CallProduceGold();
                cdTime = Time.time + cd;
                bt_gold.Disable(true);
            });


        }
        private float cdTime = 0f;
        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (cdTime > 0) {
                if (cdTime <= Time.time) {
                    bt_gold.Disable(false);
                    cdTime = -1f;
                }
            }
        }


        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();

            bt_gold.Text(string.Format(LanguageManager.Singleton["APP_NUM_FORMAT"], DataManagers.GamePlayerManager.Singleton.Gold));

            bt_Coin.Text(string.Format(LanguageManager.Singleton["APP_NUM_FORMAT"], DataManagers.GamePlayerManager.Singleton.Coin));
        }
    }
}