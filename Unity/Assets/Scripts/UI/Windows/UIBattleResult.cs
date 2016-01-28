using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Proto;
using Assets.Scripts.DataManagers;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    partial class UIBattleResult
    {
        public class PackageGridTableModel : TableItemModel<PackageGridTableTemplate>
        {
            public PackageGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }

			private PlayerGameItem _ItemData;

			public PlayerGameItem ItemData {
				set { 
					_ItemData = value; 
					Template.lb_name.text = value.Config.Name;
					Template.lb_vlaue.text = string.Format ("{0}", value.Num);
				} 
				get { return _ItemData; } 
			}
		}
        public class DropGridTableModel : TableItemModel<DropGridTableTemplate>
        {
            public DropGridTableModel(){}
            public override void InitModel()
            {
                //todo
            }

			private Item _ItemData;

			public Item ItemData {
				set { 
					_ItemData = value; 
					var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig> (value.Entry);
					Template.lb_name.text = config.Name;
					Template.lb_vlaue.text = string.Format ("{0}", value.Num);
				} 
				get { return _ItemData; } 
			}
        }

        public override void InitModel()
        {
            base.InitModel();
			bt_close.OnMouseClick ((s, e) => {
				this.HideWindow();
			});

			bt_collectall.OnMouseClick ((s, e) => {
				foreach(var i in items)
				{
					var currentSize = DataManagers.PlayerItemManager.Singleton.CurrentSize;
					var maxSize = DataManagers.GamePlayerManager.Singleton.PackageSize;
					int max = maxSize - currentSize;
					int diff = Mathf.Max(max,i.Num);
					i.Num -=diff;
					DataManagers.PlayerItemManager.Singleton.AddItemIntoPack(i.Entry,diff);
				}
				items.RemoveAll(t=>t.Num<=0);
				UIManager.Singleton.UpdateUIData();
			});
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
        }

		public override void OnUpdateUIData ()
		{
			base.OnUpdateUIData ();
			var item = items;
		
			var allPackageItems = DataManagers.PlayerItemManager.Singleton.PackageToList;
			PackageGridTableManager.Count = allPackageItems.Count;
			int index = 0;
			foreach (var i in PackageGridTableManager) 
			{
				i.Model.ItemData = allPackageItems [index];
				index++;
			}

			index = 0;
			DropGridTableManager.Count = item.Count;
			foreach (var i in DropGridTableManager) {
				i.Model.ItemData = item [index];
				index++;
			}
		}
        public override void OnHide()
        {
            base.OnHide();
        }


		private List<Item> items = new List<Item>();
		public void ShowResult(int mapID, List<Item> item,int indexPos)
		{
			items = item;
			OnUpdateUIData ();

			//show
		}
    }
}