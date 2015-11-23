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

                if (DataManagers.GamePlayerManager.Singleton.OpenProduceConfigs().Count == 0)
                {
                    //NO_BUILD
                    UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_BUILD"], 3f);
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["NO_BUILD"]);
                    return;
                }
                var ui = UIManager.Singleton.CreateOrGetWindow<UIProducePanel>();
                ui.ShowWindow();
            });

            this.PlayerIcon.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UISetting>();
                ui.ShowWindow();
            });

            bt_make.OnMouseClick((s, e) =>
            {
                var makeConfigs = ExcelConfig.ExcelToJSONConfigManager
                 .Current.GetConfigs<ExcelConfig.MakeConfig>()
                 .Where(
                          (t) =>
                          {
                              switch ((Proto.MakeItemUnlockType)t.UnlockType)
                              {
                                  case Proto.MakeItemUnlockType.NONE: return true;
                                  case Proto.MakeItemUnlockType.NeedScroll:
                                      int item = Tools.UtilityTool.ConvertToInt(t.UnlockPars1);
                                      return DataManagers.PlayerItemManager.Singleton.GetItemCount(item) > 0;
                              }
                              return false;

                          }).ToList() ;
                if (makeConfigs.Count == 0)
                {
                    UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_SCROLL"], 3f);
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["NO_SCROLL"]);
                    return;
                    return;
                }
                var ui = UIManager.Singleton.CreateOrGetWindow<UIMake>();
                ui.ShowWindow();
            });

            bt_Coin.OnMouseClick((s, e) =>
            {
                var ui = UIManager.Singleton.CreateOrGetWindow<UIPayment>();
                ui.ShowWindow();
            });

            bt_battle.OnMouseClick((s, e) =>
            {
                if (DataManagers.PlayerArmyManager.Singleton.GetAllSoldier().Count == 0)
                {
                    UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_HERO"], 3f);
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["NO_HERO"]);
                    return;
                }

                var ui = UIManager.Singleton.CreateOrGetWindow<UIGoToExplore>();
                ui.ShowWindow();
            });

            bt_contruct.OnMouseClick((s, e) =>
            {
                UIStructureBuilding.Show();
            });

            bt_train.OnMouseClick((s, e) =>
            {
                if (DataManagers.PlayerArmyManager.Singleton.GetAllSoldier().Count == 0)
                {
                    UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_HERO"],3f);
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["NO_HERO"]);
                    return;
                }
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
                iTween.scaleFrom(lb_gold.gameObject, 0.3f, 0, Vector3.one * 1.3f, iTween.EasingType.spring);
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

            this.lb_gold.text=(string.Format(LanguageManager.Singleton["APP_NUM_FORMAT"], DataManagers.GamePlayerManager.Singleton.Gold));

            bt_Coin.Text(string.Format(LanguageManager.Singleton["APP_NUM_FORMAT"], DataManagers.GamePlayerManager.Singleton.Coin));
        }
    }
}