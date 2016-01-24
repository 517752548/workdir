
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

        public enum ShowType
        {
            Gold, Coin
        }

        public class ItemGridCoinTableModel : TableItemModel<ItemGridCoinTableTemplate>
        {
            public ItemGridCoinTableModel() { }
            public override void InitModel()
            {
                //todo
				Template.Bt_itemName.OnMouseClick((s,e)=>{
					if(OnClick==null) return;
					OnClick(this);
				});
				Template.bt_info.OnMouseClick ((s, e) => {
					if(OnInfoClick==null)return;
					OnInfoClick(this);
				});
            }

			public Action<ItemGridCoinTableModel> OnInfoClick;

			public Action<ItemGridCoinTableModel> OnClick;

            public ItemConfig ItemConfig { private set; get; }
            private DimondStoreConfig _Config;
            public DimondStoreConfig Config
            {
                set
                {
                    _Config = value;
					var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(
						Tools.UtilityTool.ConvertToInt(_Config.ItemKey));
                    ItemConfig = item;
                    if (item == null) return;
					var Color = _Config.Sold_price <= DataManagers.GamePlayerManager.Singleton.Coin ?
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

			public void SetDrag(bool enable)
			{
				var d = this.Item.Root.GetComponent<UIDragScrollView> ();
				if (d == null)
					return;
				d.enabled = enable;
			}

        }

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
					var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(_Config.ItemId);
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

			public void SetDrag(bool enable)
			{
				var d = this.Item.Root.GetComponent<UIDragScrollView> ();
				if (d == null)
					return;
				d.enabled = enable;
			}
        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });

            t_coin.OnMouseClick((s, e) => {
                Type = ShowType.Coin;
                OnUpdateUIData();
            });

            t_gold.OnMouseClick((s, e) => {
                Type = ShowType.Gold;
                OnUpdateUIData();
            });
        }
        public override void OnShow()
        {
            base.OnShow();
            //¥¶¿Ì
            OnUpdateUIData();
        }


        private ShowType Type = ShowType.Gold;
        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            switch (Type)
            {
                case ShowType.Coin:
                    ShowCoin();
                    break;
                case ShowType.Gold:
                    ShowGold();
                    break;
            }
        }

        private void ShowGold()
        {
            PackageView.ActiveSelfObject(true);
            PackageViewCoin.ActiveSelfObject(false);
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
						    var buildID = Tools.UtilityTool.ConvertToInt(item.Unlock_para1);
							var config = ExcelToJSONConfigManager.Current.GetConfigByID<BuildingConfig>(buildID);
						    if(config ==null) return false;
							var build = DataManagers.BuildingManager.Singleton[config.BuildingId];
							if(build.Level>= config.Level) return true;
							return false;
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
				ItemGridTableManager [i].Model.SetDrag (shopItems.Length >= 7);
            }
        }

        private void ShowCoin()
        {
            PackageView.ActiveSelfObject(false);
            PackageViewCoin.ActiveSelfObject(true);
            var shopData = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.DimondStoreConfig>();
			ItemGridCoinTableManager.Count = shopData.Length;
            int index = 0;
            foreach (var i in ItemGridCoinTableManager)
            {
                i.Model.Config = shopData[index];
				i.Model.OnClick = OnClickBuy;
				i.Model.OnInfoClick = OnClickCoinInfo;
                i.Model.SetDrag(shopData.Length >= 7);
                index++;
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


		private void OnClickBuy(ItemGridCoinTableModel obj)
		{
			if (DataManagers.PlayerItemManager.Singleton.BuyItemUseCoin(obj.Config))
			{
				UIManager.Singleton.UpdateUIData();
			}
		}

		private void OnClickCoinInfo(ItemGridCoinTableModel obj)
		{
			UIControllor.Singleton.ShowMessage(obj.ItemConfig.Desription);
		}


        public override void OnHide()
        {
            base.OnHide();
        }
    }
}