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
				() => {
					SelectOk(result);
				},
				() => {
				});
		}

		private void SelectOk(RandomEventConfig config)
		{
			
		}
	}


}

