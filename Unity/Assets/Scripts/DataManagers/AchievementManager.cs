using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;

namespace Assets.Scripts.DataManagers
{
	public class AchievementData
	{
		[JsonName ("I")]
		public int ID { set; get; }

		[JsonName ("C")]
		public bool IsCompleted{ set; get; }

		[JsonName ("P")]
		public string Pararms { set; get; }

		public AchievementData()
		{
			Pararms = string.Empty;
		}
	}

	public class AchievementManager : Tools.XSingleton<AchievementManager>, IPresist
	{
		public const string ACHIEVEMENT_DATA = "__ACHIEVEMENT_DATA.JSON";
		private Dictionary<int,AchievementData> _datas = new Dictionary<int, AchievementData> ();

		public int AchievementPoint { get { return GamePlayerManager.Singleton.AchievementPoint; } }

		public void Presist ()
		{
			Tools.PresistTool.SaveJson (_datas.Values.ToList (), ACHIEVEMENT_DATA);
			// throw new NotImplementedException();
		}

		public void Load ()
		{
			_datas.Clear ();
			var list = Tools.PresistTool.LoadJson<List<AchievementData>> (ACHIEVEMENT_DATA);
			if (list == null || list.Count == 0)
				return;
			foreach (var i in list) {
				_datas.Add (i.ID, i);
			}
		}

		public void Reset ()
		{
			_datas.Clear ();
			this.Presist ();
		}

		public void CostGold (int gold)
		{ 
			
		}

		public void BattleCostFood (int food)
		{
			var haveCompleted = false;
			var configs = GetAllConfigs (Proto.AchievementEventType.CostFood);
			foreach (var i in configs) {
				var data = this [i.ID];
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num += food;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
		}

		public void CostCoin (int coin)
		{
			
		}

		public void ProduceGold (int gold)
		{
			var haveCompleted = false;
			var configs = GetAllConfigs (Proto.AchievementEventType.ProduceGold);
			foreach (var i in configs) {
				var data = this [i.ID];
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num += gold;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
		}

		public void BuildLevel (int buildID, int level)
		{ 
            
		}

		public void Export (int mapID, int exportValue)
		{ 
			var haveCompleted = false;
			var configs = GetAllConfigs (Proto.AchievementEventType.ExploreValue);
			foreach (var i in configs) {
				var data = this [i.ID];
				int needMap =  Tools.UtilityTool.ConvertToInt (i.Pars1);
				if (needMap != mapID)
					continue;
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num += exportValue;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
		}

		public void ArmyLevelUp (int army, int star)
		{ 
			if (star == 2) {
				var haveCompleted = false;
				var configs = GetAllConfigs (Proto.AchievementEventType.GetStarTwo);
				foreach (var i in configs) {
					var data = this [i.ID];
					var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
					num += 1;
					data.Pararms = string.Format ("{0}", num);
					if (TryToCompleteAchievement (data, i)) {
						haveCompleted = true;
					}
				}
			}
			if (star == 3) {
				var haveCompleted = false;
				var configs = GetAllConfigs (Proto.AchievementEventType.GetStarThree);
				foreach (var i in configs) {
					var data = this [i.ID];
					var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
					num += 1;
					data.Pararms = string.Format ("{0}", num);
					if (TryToCompleteAchievement (data, i)) {
						haveCompleted = true;
					}
				}
			}

		}

		public void GetItem(int itemid,int count)
		{
			var haveCompleted = false;
			var configs = GetAllConfigs (Proto.AchievementEventType.GetItem);
			foreach (var i in configs) {
				var data = this [i.ID];
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num = itemid;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
		}

		public void PackageChanged(int packageSize)
		{
			var haveCompleted = false;
			var configs = GetAllConfigs (Proto.AchievementEventType.PackageSize);
			foreach (var i in configs) {
				var data = this [i.ID];
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num = packageSize;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
		}

		public void MapCompleted (int mapID)
		{
		   
		}



		/// <summary>
		/// NO completed
		/// </summary>
		/// <returns>The all configs.</returns>
		/// <param name="eventType">Event type.</param>
		private List<AchievementConfig> GetAllConfigs (Proto.AchievementEventType eventType)
		{
			var allAchievenments = ExcelConfig.ExcelToJSONConfigManager.Current.
				GetConfigs<ExcelConfig.AchievementConfig> (t => { 
				if (_datas.ContainsKey (t.ID)) {
					if (_datas [t.ID].IsCompleted)
						return false;
				}
				return	t.ConditionType == (int)eventType;
			});
			return allAchievenments.ToList ();
		}

		private void OnCompletedAchievement (int id)
		{
			this [id].IsCompleted = true;
			var config = ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.AchievementConfig> (id);
			if (config == null)
				return;//error will crash or not?
			//oncompleted?

			var item = (config.itmeReward);
			if (item > 0) {
			
				var itemnum = Tools.UtilityTool.ConvertToInt (config.RewardPar1);
				var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (item);
				PlayerItemManager.Singleton.AddItem (item, itemnum);
				if (itemConfig != null) {
					UI.UITipDrawer.Singleton.DrawNotify (
						string.Format (LanguageManager.Singleton ["Achievement_Reward_item"], itemConfig.Name, itemnum));
					
				}
					
			}

			DataManagers.GamePlayerManager.Singleton.AddAchievementPoint (config.RewardPoint);
			UI.UIControllor.Singleton.ShowInfo (
				string.Format ( LanguageManager.Singleton[ "Achievement_Completed"], config.Name)
			);

		}

		private bool TryToCompleteAchievement (AchievementData data, AchievementConfig config)
		{
			if (data.IsCompleted)
				return false;
			var type = (Proto.AchievementEventType)config.ConditionType;
			bool completed = false;

			switch (type) {
			case Proto.AchievementEventType.CostFood:
				{
					int needFood = Tools.UtilityTool.ConvertToInt (config.Pars1);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (needFood <= have) {
						completed = true;
					}
				}
				break;
			case Proto.AchievementEventType.ExploreValue:
				{
					int needFood = Tools.UtilityTool.ConvertToInt (config.Pars2);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (needFood <= have) {
						completed = true;
					}
				}
				break;
			case Proto.AchievementEventType.Gamable:
				{
					int needFood = Tools.UtilityTool.ConvertToInt (config.Pars1);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (needFood <= have) {
						completed = true;
					}
				}
				break;
			case Proto.AchievementEventType.GetItem:
				{
					int itemID = Tools.UtilityTool.ConvertToInt (config.Pars1);
					int saveID =  Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (itemID == saveID)
						completed = true;
				}
				break;
			case Proto.AchievementEventType.GetStarThree:
				{
					int star =  Tools.UtilityTool.ConvertToInt (config.Pars1);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (star <= have)
						completed = true;
				}
				break;
			case Proto.AchievementEventType.GetStarTwo:
				{
					int star =  Tools.UtilityTool.ConvertToInt (config.Pars1);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (star <= have)
						completed = true;
				}
				break;
			case Proto.AchievementEventType.KillBoss:
				{
					int bossID =  Tools.UtilityTool.ConvertToInt (data.Pararms);
					int needBoss = Tools.UtilityTool.ConvertToInt (config.Pars2);
					if (bossID == needBoss)
						completed = true;
				}
				break;
			case Proto.AchievementEventType.KillMonsterCount:
				{
					int need = Tools.UtilityTool.ConvertToInt (config.Pars1);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (need <= have) {
						completed = true;
					}
				}
				break;
			case Proto.AchievementEventType.PackageSize:
				{
					int need = Tools.UtilityTool.ConvertToInt (config.Pars1);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (need <= have) {
						completed = true;
					}
				}
				break;
			case Proto.AchievementEventType.PKCount:
				{
					int need = Tools.UtilityTool.ConvertToInt (config.Pars1);
					int have = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (need <= have) {
						completed = true;
					}
				}
				break;
			case Proto.AchievementEventType.ProduceGold:
				{
					int needgold = Tools.UtilityTool.ConvertToInt (config.Pars1);
					int havegold = Tools.UtilityTool.ConvertToInt (data.Pararms);
					if (needgold <= havegold) {
						completed = true;
					}
				}
				break;
			}


			if (completed)
				OnCompletedAchievement (config.ID);

			return completed;
		}

		public void PKSuccess()
		{			
			var haveCompleted = false;
			var configs = GetAllConfigs (Proto.AchievementEventType.PKCount);
			foreach (var i in configs) {
				var data = this [i.ID];
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num += 1;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
		}

		public void KillMonster(int mapID,int monsterID)
		{
			//checkBoss
			var haveCompleted = false;
			var configs = GetAllConfigs (Proto.AchievementEventType.KillBoss);
			foreach (var i in configs) {
				var data = this [i.ID];
				int needMapID = Tools.UtilityTool.ConvertToInt (i.Pars1);
				if (needMapID != mapID)
					continue;
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num = monsterID;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
			//count monster kill
			configs = GetAllConfigs (Proto.AchievementEventType.KillMonsterCount);
			foreach (var i in configs) {
				var data = this [i.ID];
				var num = Tools.UtilityTool.ConvertToInt (data.Pararms);
				num += 1;
				data.Pararms = string.Format ("{0}", num);
				if (TryToCompleteAchievement (data, i)) {
					haveCompleted = true;
				}
			}
		}


		public AchievementData this [int id] {
			get {
				AchievementData data;
				if (!_datas.TryGetValue (id, out data)) {
					data = new AchievementData{ ID = id, IsCompleted = false, Pararms = string.Empty };
					_datas.Add (data.ID, data);
				}
				return data;
			}
		}

	}


 
}
