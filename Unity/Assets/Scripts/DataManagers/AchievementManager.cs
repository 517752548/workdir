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
		[JsonName("I")]
		public int ID { set; get; }
		[JsonName("C")]
		public bool IsCompleted{ set; get; }
		[JsonName("P")]
		public string Pararms { set; get; }
	}

    public class AchievementManager : Tools.XSingleton<AchievementManager>, IPresist
    {
        public const string ACHIEVEMENT_DATA = "__ACHIEVEMENT_DATA.JSON";
		private Dictionary<int,AchievementData> _datas = new Dictionary<int, AchievementData> ();

		public int AchievementPoint {  get{ return GamePlayerManager.Singleton.AchievementPoint;} }

        public void Presist()
        {
			Tools.PresistTool.SaveJson (_datas.Values.ToList (), ACHIEVEMENT_DATA);
            // throw new NotImplementedException();
        }

        public void Load()
        {
			_datas.Clear ();
			var list = Tools.PresistTool.LoadJson<List<AchievementData>> (ACHIEVEMENT_DATA);
			if (list == null || list.Count == 0)
				return;
			foreach (var i in list) {
				_datas.Add (i.ID, i);
			}
        }

        public void Reset()
        {
			_datas.Clear ();
			this.Presist ();
        }

        public void CostGold(int gold)
        { 
			
        }

		public void CostFood(int food)
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

		public void CostCoin(int coin)
		{
			
		}

		public void ProduceGold(int gold)
		{
			
		}

        public void BuildLevel(int buildID, int level)
        { 
            
        }

        public void Export(int mapID ,int exportValue)
        { 
        
        }

        public void ArmyLevelUp(int army, int level)
        { 
        
        }

		public void MapCompleted(int mapID)
		{
		   
		}

		/// <summary>
		/// NO completed
		/// </summary>
		/// <returns>The all configs.</returns>
		/// <param name="eventType">Event type.</param>
		private List<AchievementConfig> GetAllConfigs(Proto.AchievementEventType eventType)
		{
			var allAchievenments = ExcelConfig.ExcelToJSONConfigManager.Current.
				GetConfigs<ExcelConfig.AchievementConfig> (t=>{ 
					if(_datas.ContainsKey(t.ID))
				    {
						if(_datas[t.ID].IsCompleted ) return false;
					}
					return	t.ConditionType == (int)eventType;
				});
			return allAchievenments.ToList();
		}

		private void OnCompletedAchievement(int id)
		{
			this [id].IsCompleted = true;
			var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.AchievementConfig> (id);
			if (config == null)
				return;//error will crash or not?
			//oncompleted?
		}

		private bool TryToCompleteAchievement(AchievementData data, AchievementConfig config)
		{
            var type = (Proto.AchievementEventType)config.ConditionType;
            bool completed = false;
            switch (type)
            {
                case Proto.AchievementEventType.CostFood:
                    break;
                case Proto.AchievementEventType.ExploreValue:
                    break;
                case Proto.AchievementEventType.Gamable:
                    break;
                case Proto.AchievementEventType.GetItem:
                    break;
                case Proto.AchievementEventType.GetStarThree:
                    break;
                case Proto.AchievementEventType.GetStarTwo:
                    break;
                case Proto.AchievementEventType.KillBoss:
                    break;
                case Proto.AchievementEventType.KillMonsterCount:
                    break;
                case Proto.AchievementEventType.PackageSize:
                    break;
                case Proto.AchievementEventType.PKCount:
                    break;
                case Proto.AchievementEventType.ProduceGold:
                    break;
            }



			return completed;
		}


		private AchievementData this [int id] 
		{
			get {
				AchievementData data;
				if (!_datas.TryGetValue (id, out data))
				{
					data = new AchievementData{ ID = id, IsCompleted = false, Pararms = string.Empty };
					_datas.Add (data.ID, data);
				}
				return data;
			}
		}

	}


 
}
