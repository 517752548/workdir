using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

namespace Assets.Scripts.UI.Windows
{
	partial class UIMake
	{
		#region 制作条目

		public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
		{
			public ItemGridTableModel ()
			{
			}

			public override void InitModel ()
			{
				//todo
				Template.Bt_itemName.OnMouseClick ((s, e) => {
					if (this.OnClick == null)
						return;
					this.OnClick (this);
				});
				Template.bt_info.OnMouseClick ((s, e) => {
					#region Message
					if (_MakeConfig == null)
						return;
					var sb = new StringBuilder ();
					sb.Append (LanguageManager.Singleton ["UIMake_Cost_Title"]);
					if (_MakeConfig.RequireGold > 0) {
						
						sb.Append (string.Format (LanguageManager.Singleton ["UIMake_Cost_gold"],
							_MakeConfig.RequireGold));
						
					}
					var costItems = UtilityTool.SplitKeyValues (_MakeConfig.RequireItems, _MakeConfig.RequireItemsNumber);
					foreach (var i in costItems) {
						var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i.Key);
						if (item == null)
							continue;
	
						sb.Append (string.Format (LanguageManager.Singleton ["UIMake_Cost_Item"], item.Name, i.Value));
				
					}

					sb.Append (LanguageManager.Singleton ["UIMake_Reward_Title"]);
					var rewardItem = UtilityTool.SplitKeyValues (_MakeConfig.RewardItems, _MakeConfig.RewardItemsNumber);
					foreach (var i in rewardItem) {
						var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i.Key);
						if (item == null)
							continue;
						sb.Append (string.Format (LanguageManager.Singleton ["UIMake_Cost_Item"], item.Name, i.Value));
					}

					UIControllor.Singleton.ShowMessage (sb.ToString (), 10);

					#endregion
				});
			}


			public Action<ItemGridTableModel> OnClick;
			private ExcelConfig.MakeConfig _MakeConfig;

			public ExcelConfig.MakeConfig MakeConfig {
				get {
					return _MakeConfig;
				}
				set {
					_MakeConfig = value;
					Template.Bt_itemName.Text (_MakeConfig.Name);
					var sb = new StringBuilder ();
					var items = UtilityTool.SplitKeyValues (_MakeConfig.RequireItems, _MakeConfig.RequireItemsNumber);
					if (_MakeConfig.RequireGold > 0) {
						var colorGold = _MakeConfig.RequireGold >= DataManagers.GamePlayerManager.Singleton.Gold ? 
							LanguageManager.Singleton ["APP_GREEN"] : LanguageManager.Singleton ["APP_RED"];
						//sb.Append (colorGold);
						sb.Append ( 
							string.Format (LanguageManager.Singleton ["UIMake_Cost_gold"],
								string.Format(colorGold,
							    _MakeConfig.RequireGold)));
						sb.Append ("[-]");
					}
					foreach (var i in items) {
						var item = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i.Key);
						if (item == null)
							continue;
						var colorItem = i.Value <= DataManagers.PlayerItemManager.Singleton.GetItemCount (i.Key) ? 
							LanguageManager.Singleton ["APP_GREEN"] : LanguageManager.Singleton ["APP_RED"];
						
						sb.Append ( 
							string.Format (LanguageManager.Singleton ["UIMake_Cost_Item"], item.Name,
								string.Format(colorItem,i.Value))
						);
						sb.Append ("[-]");
					}

					Template.lb_cost.text = sb.ToString ();
				}
			}
		}

		#endregion

		#region 类别

		public class TypeItemGridTableModel : TableItemModel<TypeItemGridTableTemplate>
		{
			public TypeItemGridTableModel ()
			{
			}

			public override void InitModel ()
			{
				//todo
				Template.Bt_itemName.OnMouseClick ((s, e) => {
					if (OnClick == null)
						return;
					OnClick (this);
				});

                
			}

			public Action<TypeItemGridTableModel> OnClick;
          
		}

		#endregion

		public enum ShowTypeName
		{
			Types,
			Info
		}

		private ShowTypeName CurrentType = ShowTypeName.Types;

		public override void InitModel ()
		{
			base.InitModel ();
			//Write Code here
			bt_close.OnMouseClick ((s, e) => {
				//if(CurrentType == ShowTypeName.Info)
				//{
				//    ShowTypes();
				//}
				//else { HideWindow(); }
				HideWindow ();
			});
		}

		public override void OnShow ()
		{
			base.OnShow ();
			ShowType ();
		}

		private void ShowTypes ()
		{
			CurrentType = ShowTypeName.Types;
			//PackageTypeView.ActiveSelfObject(true);
			//PackageView.ActiveSelfObject(false);

			//var configs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.MakeCategoryConfig>();
			//TypeItemGridTableManager.Count = configs.Length;
			//int index = 0;
			//foreach (var i in TypeItemGridTableManager)
			//{
			//    i.Model.OnClick = OnClick;
			//    i.Model.Category = configs[index];
			//    index++;
			//}
		}

		private void OnClick (TypeItemGridTableModel obj)
		{
			//ShowType(obj.Category.ID);
		}

		public override void OnHide ()
		{
			base.OnHide ();
		}

		public void ShowType ()
		{
			CurrentType = ShowTypeName.Info;
			PackageTypeView.ActiveSelfObject (false);
			PackageView.ActiveSelfObject (true);
			var makeConfigs = ExcelConfig.ExcelToJSONConfigManager
                .Current.GetConfigs<ExcelConfig.MakeConfig> ()
                .Where (
				                           (t) => {
					switch ((Proto.MakeItemUnlockType)t.UnlockType) {
					case Proto.MakeItemUnlockType.NONE:
						return true;
					case Proto.MakeItemUnlockType.NeedScroll:
						int item = Tools.UtilityTool.ConvertToInt (t.UnlockPars1);
						return DataManagers.PlayerItemManager.Singleton.GetItemCount (item) > 0;
					}
					return false;

				}).ToArray ();
			int index = 0;
			ItemGridTableManager.Count = makeConfigs.Length;
			foreach (var i in ItemGridTableManager) {
				i.Model.MakeConfig = makeConfigs [index];
				i.Model.OnClick = OnMakeItemClick;
				index++;
			}
		}

		private void OnMakeItemClick (ItemGridTableModel obj)
		{
			if (DataManagers.PlayerItemManager.Singleton.MakeItem (obj.MakeConfig)) {
				UIManager.Singleton.UpdateUIData ();
			}
		}
	}
}