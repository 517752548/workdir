using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataManagers
{
    public class AchievementManager : Tools.XSingleton<AchievementManager>, IPresist
    {
        public const string ACHIEVEMENT_DATA = "__ACHIEVEMENT_DATA.JSON";

		public int AchievementPoint {  get{ return GamePlayerManager.Singleton.AchievementPoint;} }

        public void Presist()
        {
            // throw new NotImplementedException();
        }

        public void Load()
        {
            //throw new NotImplementedException();
        }

        public void Reset()
        {
            //throw new NotImplementedException();
        }

        public void CostGold(int gold)
        { 
        
        }

        public void BuildLevel(int buildID, int level)
        { 
        
        }

        public void Export(int exportValue)
        { 
        
        }

        public void ArmyLevelUp(int army, int level)
        { 
        
        }


    }


    public class AchievementData
    {
        [JsonName("I")]
        public int ID { set; get; }
        [JsonName("P")]
        public string Pararms { set; get; }
    }
}
