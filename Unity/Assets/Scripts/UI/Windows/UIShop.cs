using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;
using Proto;


namespace Assets.Scripts.UI.Windows
{
    partial class UIShop
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                Template.bt_info.OnMouseClick((s, e) => {
                    if (OnInfoClick == null) return;
                    OnInfoClick(this);
                });

                Template.Bt_itemName.OnMouseClick((s, e) => {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }

            public Action<ItemGridTableModel> OnInfoClick;
            public Action<ItemGridTableModel> OnClick;

            public ItemConfig ItemConfig { private set; get; }
            private StoreDataConfig _Config;
            public StoreDataConfig Config
            {
                set
                {
                    _Config = value;
                    var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(_Config.ID);
                    ItemConfig = item;
                    if (item == null) return;
                    var Color = _Config.Sold_price <= DataManagers.GamePlayerManager.Singleton.Gold ?
                        LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];
                    Template.Bt_itemName.Text(item.Name);
                    Template.lb_cost.text = string.Format(LanguageManager.Singleton["UIShop_Model_Cost"], 
                        string.Format(Color,
                        _Config.Sold_price));
                   
                }
                get
                {
                    return _Config;
                }
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
        }
        public override void OnShow()
        {
            base.OnShow();
            //¥¶¿Ì
            OnUpdateUIData();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData(); 
            var shopItems = ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.StoreDataConfig>(
                (item) =>
                {
                    #region Condition
                    var unlockType = (StoreUnlockType)item.Unlock_type;
                    switch (unlockType)
                    {
                        case StoreUnlockType.None:
                            return true;
                        case StoreUnlockType.BuildGetTargetLevel:
                            var builds = Tools.UtilityTool.SplitKeyValues(item.Unlock_para1);
                            foreach (var i in builds)
                            {
                                var build = DataManagers.BuildingManager.Singleton[i.Key];
                                if (build == null) return false;
                                if (build.Level < i.Value) return false;
                            }
                            return true;
                        case StoreUnlockType.ExploreGetTarget:
                            int explore = 0;
                            if (!int.TryParse(item.Unlock_para1, out explore)) return false;
                            if (DataManagers.GamePlayerManager.Singleton[DataManagers.PlayDataKeys.EXPLORE_VALUE] < explore) return false;
                            return true;
                    }
                    return true;
                    #endregion
                });

            ItemGridTableManager.Count = shopItems.Length;
            for (var i = 0; i < ItemGridTableManager.Count; i++)
            {
                ItemGridTableManager[i].Model.Config = shopItems[i];
                ItemGridTableManager[i].Model.OnInfoClick = OnInfoClick;
                ItemGridTableManager[i].Model.OnClick = OnClick;
            }
        }

        private void OnClick(ItemGridTableModel obj)
        {
            if (DataManagers.PlayerItemManager.Singleton.BuyItem(obj.Config))
            {
                UIManager.Singleton.UpdateUIData();
            }
        }

        private void OnInfoClick(ItemGridTableModel obj)
        {
            UIControllor.Singleton.ShowMessage(obj.ItemConfig.Desription);
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}