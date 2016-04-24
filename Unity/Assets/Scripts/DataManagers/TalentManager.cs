using System;
using System.Collections.Generic;
using System.Linq;

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
		}
	}
}

