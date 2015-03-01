using Assets.Scripts.Appliaction;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataManagers
{
    public enum PlayDataKeys
    {
        PRODUCE_LEVEL = 1, //玩家的炼金等级
    }
    public class GamePlayerManager : Tools.XSingleton<GamePlayerManager>
    {
        
        //user data
        public const string PLAYER_DATA_PATH = "_PALYER_DATA_.json";
        private Dictionary<int, string> PlayerData { set; get; }

        public GamePlayerManager()
        {
            PlayerData = new Dictionary<int, string>();
        }

        public string this[PlayDataKeys key]
        {
            get
            {
                string value;
                if (PlayerData.TryGetValue((int)key, out value)) return value;
                return null;
            }
        }

        public void Load()
        {
            string json = GameAppliaction.Singleton.ReadFile(PLAYER_DATA_PATH);
            PlayerData = new Dictionary<int, string>();
            if(!string.IsNullOrEmpty(json))
            {
                var list = JsonTool.Deserialize<List<PlayerData>>(json);
                foreach(var i in list)
                {
                    PlayerData.Add(i.Key, i.Value);
                }
            }
        }

        public void Presist()
        {
            var list = PlayerData.Select(t => new PlayerData { Key = t.Key, Value = t.Value }).ToList();
            var json = JsonTool.Serialize(list);
            GameAppliaction.Singleton.SaveFile(PLAYER_DATA_PATH, json, false);

        }
    }

    public class PlayerData
    {
        public int Key { set; get; }
        public string Value { set; get; }
    }
}
