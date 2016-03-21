using System;

namespace Assets.Scripts.DataManagers
{
	
	public class TalentManager: Tools.XSingleton<TalentManager>,IPresist
	{
		public TalentManager ()
		{
		}

		#region IPresist implementation
		public void Presist ()
		{
			throw new NotImplementedException ();
		}
		public void Load ()
		{
			throw new NotImplementedException ();
		}
		public void Reset ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

