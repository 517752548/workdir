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
             

				Template.Bt_itemName.OnMouseClick ((s, e) => {
					UI.UIControllor.Singleton.ShowInfo(LanguageManager.Singleton["Charge_Shop_info"],3);
				});

				Template.bt_add.OnMouseClick ((s, e) => {
					OnClickAdd(this);
				});

				Template.bt_sub.OnMouseClick ((s, e) => {
					OnClickSub(this);
				});
            }

			public Action<ItemGridTableModel> OnClickAdd;
			public Action<ItemGridTableModel> OnClickSub;
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
				//Template.lb_s_cost.text = string.Format ("{0}", gold);
				ItemID = itemid;
			}
        }

        public override void InitModel()
        {
            base.InitModel();

			bt_close.OnMouseClick((s,e)=>{
				this.HideWindow();
			});
			bt_Ok.OnMouseClick ((s, e) => {
				Buy();
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
			this.ItemGridTableManager [0].Model.OnClickAdd = OnClickAdd;
			this.ItemGridTableManager [0].Model.OnClickSub = OnClickSub;
			ShowCost ();
		}


		public void ShowFood(int itemid,int gold)
		{
			this.itemid = itemid;
			this.gold = gold;
			OnUpdateUIData ();

		}


		public bool Buy()
		{
			int packageCur = PlayerItemManager.Singleton.CurrentSize;
			int packageSize = GamePlayerManager.Singleton.PackageSize;
			if (packageCur >= packageSize) {
				UI.UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["UI_CHARGE_PACKAGE_FULL"]);
				return false;
			}

			if (total == 0)
				return false;
			if (packageCur + total > packageSize)
				return false;
			
			var goldTotal = total * gold;

			if (!(DataManagers.GamePlayerManager.Singleton.Gold >= goldTotal)) {
				UI.UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["Charge_NOGOLD"]);
			} else {
				DataManagers.GamePlayerManager.Singleton.SubGold (goldTotal);
				DataManagers.PlayerItemManager.Singleton.AddItemIntoPack (this.itemid, total);
				UI.UIManager.Singleton.UpdateUIData ();
				var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig> (this.itemid);
				UI.UITipDrawer.Singleton.DrawNotify (
					string.Format (LanguageManager.Singleton ["Charge_BuySuccess"], goldTotal, config.Name, total));
				
			}

			return true;
		}

		private int total = 0;

		public void OnClickSub(ItemGridTableModel model)
		{

			int packageCur = PlayerItemManager.Singleton.CurrentSize;
			int packageSize = GamePlayerManager.Singleton.PackageSize;
			if (total ==0) {
				//UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_CHARGE_PACKAGE_FULL"]);
				return;
			}

			total--;

			ShowCost ();
		}

		public void OnClickAdd(ItemGridTableModel model)
		{
			int packageCur = PlayerItemManager.Singleton.CurrentSize;
			int packageSize = GamePlayerManager.Singleton.PackageSize;
			if (packageCur + total >= packageSize) {
				UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_CHARGE_PACKAGE_FULL"]);
				return;
			}
			total++;
			ShowCost ();
		}

		public void ShowCost()
		{

			lb_cost.text = string.Format (LanguageManager.Singleton ["UIChargeShop_Cost"], gold * total);
			ItemGridTableManager [0].Template.lb_s_cost.text = string.Format ("{0}", total);
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