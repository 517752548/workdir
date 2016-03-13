using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.DataManagers;

namespace Assets.Scripts.UI.Windows
{
    partial class UIChargeShop
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
				Template.Bt_itemName.OnMouseClick((s,e)=>{
					OnClick(this);

				});

				Template.bt_info.OnMouseClick ((s, e) => {
					UI.UIControllor.Singleton.ShowInfo(LanguageManager.Singleton["Charge_Shop_info"],3);
				});
            }

			public Action<ItemGridTableModel> OnClick;
			private ExcelConfig.ItemConfig Config;

			public int ItemID
			{ 
				set
				{
					Config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig> (value);
					Template.Bt_itemName.Text (Config.Name);

				} 
				get{ return Config.ID; }
			}

			public void SetPrice(int itemid,int gold)
			{
				Template.lb_cost.text = string.Format ("{0}", gold);
				ItemID = itemid;
			}
        }

        public override void InitModel()
        {
            base.InitModel();

			bt_close.OnMouseClick((s,e)=>{
				this.HideWindow();
			});
            //Write Code here
        }

		private int itemid;
		private int gold;

		public override void OnUpdateUIData ()
		{
			base.OnUpdateUIData ();

			this.ItemGridTableManager.Count = 1;
			this.ItemGridTableManager [0].Model.SetPrice (itemid, gold);
			this.ItemGridTableManager [0].Model.OnClick = OnClickBuy;
		}

		public void ShowFood(int itemid,int gold)
		{
			this.itemid = itemid;
			this.gold = gold;
			OnUpdateUIData ();

		}

		public void OnClickBuy(ItemGridTableModel model)
		{
			int packageCur = PlayerItemManager.Singleton.CurrentSize;
			int packageSize = GamePlayerManager.Singleton.PackageSize;
			if (packageCur >= packageSize) {
				UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_CHARGE_PACKAGE_FULL"]);
				return;
			}
			if (DataManagers.GamePlayerManager.Singleton.Gold >= gold) {
				DataManagers.GamePlayerManager.Singleton.SubGold (this.gold);
				DataManagers.PlayerItemManager.Singleton.AddItemIntoPack (this.itemid, 1);
				UI.UIManager.Singleton.UpdateUIData ();
				//Charge_BuySuccess

				var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig> (this.itemid);
				UI.UITipDrawer.Singleton.DrawNotify(
					string.Format(LanguageManager.Singleton ["Charge_BuySuccess"],gold,config.Name));
			} else {
				
				UI.UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["Charge_NOGOLD"]);
			}
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