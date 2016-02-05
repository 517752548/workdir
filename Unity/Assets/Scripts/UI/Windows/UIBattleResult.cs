using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Proto;
using Assets.Scripts.DataManagers;
using UnityEngine;
using System.Collections;

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
			this.CanDestoryWhenHide = true;
			bt_close.OnMouseClick ((s, e) => {
				this.HideWindow();
			});

			bt_collectall.OnMouseClick ((s, e) => {
				foreach(var i in items)
				{
					var currentSize = DataManagers.PlayerItemManager.Singleton.CurrentSize;
					var maxSize = DataManagers.GamePlayerManager.Singleton.PackageSize;
					int max = maxSize - currentSize;
					int diff = Mathf.Min(max,i.Num);
					i.Num -=diff;
					DataManagers.PlayerItemManager.Singleton.AddItemIntoPack(i.Entry,diff);
				}
				items.RemoveAll(t=>t.Num<=0);
				UIManager.Singleton.UpdateUIData();

				if(callAfterCollect ==null ) return;
				callAfterCollect(this.mapID,this.posIndex, items);

				if(items.Count==0)
					StartCoroutine(DelayClose());
			});
            //Write Code here
        }

		private IEnumerator DelayClose()
		{
			yield return new WaitForSeconds (0.3f);
			HideWindow();
		}

		public Action<int,int,List<Item>> callAfterCollect;

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

			lb_packageSize.text = string.Format (LanguageManager.Singleton ["UI_RESULT_PACKAGE_SIZE"],

				PlayerItemManager.Singleton.CurrentSize,
				GamePlayerManager.Singleton.PackageSize);
		}
        public override void OnHide()
        {
            base.OnHide();
        }


		private int posIndex;
		private int mapID;

		private List<Item> items = new List<Item>();
		public void ShowResult(int mapID, List<Item> item,int indexPos)
		{
			items = item;
			posIndex = indexPos;
			this.mapID = mapID;
			OnUpdateUIData ();
			//show
		}
    }
}