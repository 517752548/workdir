using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.DataManagers
{
	
	public class TalentManager: Tools.XSingleton<TalentManager>,IPresist
	{
		public TalentManager ()
		{
		}

		private HashSet<int> _talents = new HashSet<int> ();

		public const string _TALENT_FILE = "__TALENT_FILE.json";

		#region IPresist implementation
		public void Presist ()
		{

			var list = _talents.ToList ();
			Tools.PresistTool.SaveJson (list, _TALENT_FILE);
		}
		public void Load ()
		{
			_talents.Clear ();
			var list = Tools.PresistTool.LoadJson<List<int>> (_TALENT_FILE);
			foreach (var i in list) {
				_talents.Add (i);
			}
		}
		public void Reset ()
		{
			_talents.Clear ();
			Presist ();
		}
		#endregion

		public void ActiveTalent(int talentID)
		{
			if (this._talents.Contains (talentID))
				return;


			var talent = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.TalentConfig> (talentID);
			var attribute = Tools.UtilityTool.ConvertToInt (talent.Pars1);
			var value = Tools.UtilityTool.ConvertToInt (talent.Pars2);
			this._talents.Add (talentID);
			switch (attribute) {
			case 1:
				{
					//hp
					var data = Mathf.Max (0, GamePlayerManager.Singleton [PlayDataKeys.TeamHPAppend]);
					GamePlayerManager.Singleton [PlayDataKeys.TeamHPAppend] = data + value;
				}
				break;
			case 2:
				{
					var data = Mathf.Max (0, GamePlayerManager.Singleton [PlayDataKeys.TeamDamage]);
					GamePlayerManager.Singleton [PlayDataKeys.TeamDamage] = data + value;
				}
				break;
			case 3:
				{
					var data = Mathf.Max (0, GamePlayerManager.Singleton [PlayDataKeys.TeamDamageSub]);
					GamePlayerManager.Singleton [PlayDataKeys.TeamDamageSub] = data + value;
				}
				break;
			case 4:
				{
					var data = Mathf.Max (0, GamePlayerManager.Singleton [PlayDataKeys.TeamPrecision]);
					GamePlayerManager.Singleton [PlayDataKeys.TeamPrecision] = data + value;
				}
				break;
			case 5:
				{
					var data = Mathf.Max (0, GamePlayerManager.Singleton [PlayDataKeys.TeamJouk]);
					GamePlayerManager.Singleton [PlayDataKeys.TeamJouk] = data + value;
				}
				break;
			}

			this.Presist ();
			GamePlayerManager.Singleton.Presist ();
		}



	}
}

