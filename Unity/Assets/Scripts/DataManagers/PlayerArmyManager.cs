using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataManagers
{
    public class PlayerArmyManager : Tools.XSingleton<PlayerArmyManager>,IPresist
    {
        public const string PLAYER_ARMY_FILE = "_PLAYER_ARMY.json";

        public int MaxPackageSize
        {
            get
            {
                return GamePlayerManager.Singleton[PlayDataKeys.PACKAGE_SIZE];
            }
        }

        public int MaxArmySize
        {
            get
            {
                return GamePlayerManager.Singleton[PlayDataKeys.TEAM_SIZE];
            }
        }

        public void Presist()
        {
            //throw new NotImplementedException();
        }

        public void Load()
        {
            //throw new NotImplementedException();
        }
    }

    public class ArmyTeam
    {
        /// <summary>
        /// Item of team
        /// </summary>
        [JsonName("i")]
        public List<PlayerGameItem> Items { set; get; }
        /// <summary>
        /// 士兵
        /// </summary>
        [JsonName("s")]
        public List<PlayerSoldierData> Soldiers { set; get; }
    }

}
