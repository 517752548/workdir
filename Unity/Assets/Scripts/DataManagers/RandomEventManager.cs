using System;
using ExcelConfig;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.DataManagers
{
	

	public class RandomEventManager: Tools.XSingleton<RandomEventManager>,IPresist
	{
		public class RandomKeyValue
		{
			public int Key{ set; get;}
			public int Count{ set; get;}
		}

		public const string _FILE_EVENT_ = "___EVENT.json";

		public RandomEventManager ()
		{
			
		}


		#region IPresist implementation

		public void Load ()
		{
			var list = Tools.PresistTool.LoadJson<List<RandomKeyValue>> (_FILE_EVENT_);
			_events.Clear ();
			foreach (var i in list) {
				_events.Add (i.Key, i.Count);
			}
		}

		public void Presist  ()
		{
			var list = _events.Select (t => new RandomKeyValue{ Key = t.Key, Count = t.Value }).ToList ();
			Tools.PresistTool.SaveJson (list, _FILE_EVENT_);
		}

		public void Reset ()
		{
			_events.Clear ();
			Presist ();
		}

		#endregion

		private Dictionary<int,int> _events = new Dictionary<int, int>();


		public void TryToRandom()
		{
			if (!GamePlayerManager.Singleton.IsEventTimeout)
				return;

			GamePlayerManager.Singleton.SetEventTimeTo (60 * 10);

			var configs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.RandomEventConfig> ();
			//unlock;

			var list = new List<RandomEventConfig> ();

			foreach (var i in configs) {

				switch (i.Type) {
				case 1: //天赋
					if (_events.ContainsKey (i.ID))
						continue;
					break;
				case 3: //目前不让赌博
					continue;
					break;
				}

				switch (i.EventUnlockType) {
				case 1:
					//unlock mapID
					{
						int map = i.UnlockCondition;
						if (!PlayerMapManager.Singleton.MapIsOpen (map))
							continue;
					}
					break;
				case 2:
					{
						int achID = (i.UnlockCondition);
						if (!AchievementManager.Singleton.HadGet (achID))
							continue;
					}
					break;
				case 3:
					{
						int itemid = (i.UnlockCondition);
						if (!(PlayerItemManager.Singleton.GetItemCount (itemid) > 0))
							continue;
					}
					break;
				}

				list.Add (i);
			}


			if (list.Count == 0)
				return;
			
			var result = Tools.GRandomer.RandomList (list);

			UI.Windows.UIMessageBox.ShowMessage (
				result.Name,
				result.Dialog1,
				result.OK,
				result.Cancel,
				() => {
					SelectOk(result);
				},
				() => {
				});
		}

		private void SelectOk(RandomEventConfig config)
		{
			switch (config.CostType) {
			case 1:  //gold
				{
					var gold = Tools.UtilityTool.ConvertToInt (config.NeedItem);
					if (gold > GamePlayerManager.Singleton.Gold) {
						UI.UITipDrawer.Singleton.DrawNotify (
							string.Format (LanguageManager.Singleton ["Event_No_Gold"], gold));
						return;
					}
					GamePlayerManager.Singleton.SubGold (gold) ;
					UI.UIControllor.Singleton.ShowMessage (
						string.Format(LanguageManager.Singleton["Event_Cost_gold"],gold));
					ProcessReward(config);
				}
				break;
			case 3: //item 11:2
				{
					var items = Tools.UtilityTool.SplitKeyValues (config.NeedItem);
					foreach (var i in items) {
						if (PlayerItemManager.Singleton.GetItemCount (i.Key) < i.Value) {
							var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i.Key);
							UI.UITipDrawer.Singleton.DrawNotify (
								string.Format (LanguageManager.Singleton ["Event_No_Item"], itemConfig.Name,i.Value));
							return;
						}
					}

					foreach (var i in items) {
						PlayerItemManager.Singleton.SubItem (i.Key, i.Value);
						var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i.Key);
						UI.UIControllor.Singleton.ShowMessage (string.Format (LanguageManager.Singleton ["Event_Cost_Item"],
							itemConfig.Name,i.Value));
					}

					ProcessReward (config);
				}
				break;
			}
		}

		private void ProcessReward(RandomEventConfig config)
		{
			if (!this._events.ContainsKey (config.ID))
				this._events.Add (config.ID, 1);
			else
				this._events [config.ID] += 1;


			switch (config.RewardType) {
			case 1:
				{
					var rewardGold = Tools.UtilityTool.ConvertToInt (config.RewardConditon);
					GamePlayerManager.Singleton.AddGold (rewardGold);
					UI.UITipDrawer.Singleton.DrawNotify (
						string.Format (LanguageManager.Singleton ["Event_reward_gold"], rewardGold));
				}
				break;
			case 2:
				{
					var talentID = Tools.UtilityTool.ConvertToInt (config.RewardConditon);
					var talentConfig = ExcelToJSONConfigManager.Current.GetConfigByID<TalentConfig> (talentID);
					TalentManager.Singleton.ActiveTalent (talentID);
					UI.UITipDrawer.Singleton.DrawNotify (
						string.Format (LanguageManager.Singleton ["Event_reward_talent"], talentConfig.Name));
				}
				break;
			case 3:
				{
					var items = Tools.UtilityTool.SplitKeyValues (config.RewardConditon);
					foreach (var i in items) {
						var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i.Key);
						PlayerItemManager.Singleton.AddItem (i.Key, i.Value);
						UI.UITipDrawer.Singleton.DrawNotify (
							string.Format (LanguageManager.Singleton ["Event_reward_item"], itemConfig.Name,i.Value));
					}
				}
				break;
			}
		}
	}


}

