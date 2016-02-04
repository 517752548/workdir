using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

namespace Assets.Scripts.UI.Windows
{
    partial class UIScrectShop
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
				this.Template.Bt_itemName.OnMouseClick ((s, e) => {
					if(OnBuy==null) return;
					OnBuy(this);
				});
                //todo
            }
			public Action<ItemGridTableModel> OnBuy;
			public ExcelConfig.SecretStoreConfig _Config;
			public ExcelConfig.SecretStoreConfig Config{ 
				set
				{
					_Config = value;
					var item = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (value.item_id);
					Template.Bt_itemName.Text (item.Name);

					switch ((Proto.EmployCostCurrent)_Config.current_type) {
					case Proto.EmployCostCurrent.Coin:
						{
							var Color = _Config.Sold_price <= DataManagers.GamePlayerManager.Singleton.Coin ?
							LanguageManager.Singleton ["APP_GREEN"] : LanguageManager.Singleton ["APP_RED"];
							Template.Bt_itemName.Text (item.Name);
							Template.lb_cost.text = string.Format (LanguageManager.Singleton ["UIShop_Model_Cost"],
								string.Format (Color,
									_Config.Sold_price));
							Template.s_coin.spriteName ="Main_ui_coin";
						}
						break;
					case Proto.EmployCostCurrent.Gold:
						{
							Template.s_coin.spriteName ="Main_ui_gold";
							var Color = _Config.Sold_price <= DataManagers.GamePlayerManager.Singleton.Gold ?
							LanguageManager.Singleton ["APP_GREEN"] : LanguageManager.Singleton ["APP_RED"];
							Template.Bt_itemName.Text (item.Name);
							Template.lb_cost.text = string.Format (LanguageManager.Singleton ["UIShop_Model_Cost"],
								string.Format (Color,
									_Config.Sold_price));
						}
						break;
					}
				
				} 
				get{return _Config;} 
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
			bt_close.OnMouseClick ((s, e) => {
				HideWindow();
			});
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
			OnUpdateUIData ();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

		public override void OnUpdateUIData ()
		{
			base.OnUpdateUIData ();
			if (mapID == -1)
				return;
			var shopItems = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.SecretStoreConfig> (t => {
				var isShop = t.Store_id == this.shopID ; 
				if(isShop)
				{
					if(t.Max_purchase_times>0)
						return t.Max_purchase_times>DataManagers.PlayerItemManager.Singleton.GetScrectShopCount(t.Store_id,t.ID);
					return true;
				}

				return false;
			});

			ItemGridTableManager.Count = shopItems.Length;
			int index = 0;
			foreach (var i in ItemGridTableManager) {
				i.Model.Config = shopItems [index];
				i.Model.OnBuy = BuyItem;
				index++;
				i.Model.SetDrag (shopItems.Length >= 9);
			}
		}

		public void BuyItem(ItemGridTableModel model)
		{
			if (DataManagers.PlayerItemManager.Singleton.BuyScrectShopItem (model.Config)) 
			{
				UIManager.Singleton.UpdateUIData ();
			}
		}

		private int mapID = -1;
		private int index;
		private int shopID;

		public static void ShowMapScrectShop(int mapID, int index, int shopID)
		{
			var ui = Show ();
			ui.mapID = mapID;
			ui.index = index;
			ui.shopID = shopID;
		    
		}
    }
}