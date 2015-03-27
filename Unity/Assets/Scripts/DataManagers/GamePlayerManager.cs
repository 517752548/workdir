using Assets.Scripts.Appliaction;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataManagers
{
    public enum PlayDataKeys
    {
        PRODUCE_LEVEL = 1, //玩家的炼金等级
        PRODUCE_CLICK_TIMES = 2//炼金点击次数
    }
    public class GamePlayerManager : Tools.XSingleton<GamePlayerManager>
    {
        
        //user data
        public const string PLAYER_DATA_PATH = "_PALYER_DATA_.json";
        //资源生产技术开放
        public const string PLAYER_PRODUCE_OPEN = "_PLAYER_PRODUCE_OPEN.json";
        private Dictionary<int, string> PlayerData { set; get; }

        private Dictionary<int, bool> ProduceOpenState { set; get; }

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
            set
            {
                if(this[key]==null)
                {
                    PlayerData.Add((int)key, value);
                }else
                {
                    PlayerData[(int)key] = value;
                }
            }
        }

        public void Load()
        {
            var list = Tools.PresistTool.LoadJson<List<PlayerData>>(PLAYER_DATA_PATH);
            PlayerData = new Dictionary<int, string>();
            if (list != null)
            {
                foreach (var i in list)
                {
                    PlayerData.Add(i.Key, i.Value);
                }
            }
            var data = Tools.PresistTool.LoadJson<List<ProducePrisitData>>(PLAYER_PRODUCE_OPEN);
            ProduceOpenState = new Dictionary<int, bool>();
            if (data!=null)
            {
                foreach (var i in data)
                {
                    if (ProduceOpenState.ContainsKey(i.ProduceID)) continue;
                    ProduceOpenState.Add(i.ProduceID, i.IsOpen);
                }
            }
        }

        public void Presist()
        {
            var list = PlayerData.Select(t => new PlayerData { Key = t.Key, Value = t.Value }).ToList();
            var json = JsonTool.Serialize(list);
            GameAppliaction.Singleton.SaveFile(PLAYER_DATA_PATH, json, false);
            
            var produceList = ProduceOpenState.Select(t => new ProducePrisitData { ProduceID = t.Key, IsOpen = t.Value }).ToList();
            var produceJson = JsonTool.Serialize(produceList);
            GameAppliaction.Singleton.SaveFile(PLAYER_PRODUCE_OPEN, produceJson, false);
        }

        public List<ExcelConfig.ResourcesProduceConfig> OpenProduceConfigs()
        {
            var configs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.ResourcesProduceConfig>();
            var openeds = configs.Where(t => {
                bool result = false;
                if(ProduceOpenState.TryGetValue(t.ID, out result))
                {
                    return result;
                }
                return result;
            })
            .ToList();

            return openeds;
        }

        public void OpenProduceById(int id)
        {
            if(this.ProduceOpenState.ContainsKey(id))
            {
                this.ProduceOpenState[id] = true;
            }else
            {
                this.ProduceOpenState.Add(id, true);
            }
            //保存
            Presist();
        }

        public int GetIntValue(PlayDataKeys key)
        {
            var data = this[key];
            if (string.IsNullOrEmpty(data)) return 0;
            return Convert.ToInt32(data);
        }

        public List<Proto.Item> CallFunByID(int id)
        {
            var result = new List<Proto.Item>();
            var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.FunctionConfig>(id);
            if (config != null)
            {
                var produces = PlayerItemManager.SplitFormatItemData(config.Produce);
                foreach (var i in produces)
                {
                   Proto.Item item= PlayerItemManager.Singleton.AddItem(i[0], i[1]);
                   result.Add(item);
                }
            }
            return result;
        }
        public ExcelConfig.ProduceLevelUpConfig CurrentLevel { private set; get; }
        internal float CallProduceGold()
        {
            if (CurrentLevel == null)
            {
                var produceLevel = DataManagers.GamePlayerManager.Singleton.GetIntValue(DataManagers.PlayDataKeys.PRODUCE_LEVEL);
                if (produceLevel <= 0)
                {
                    this[DataManagers.PlayDataKeys.PRODUCE_LEVEL] = "1";
                    produceLevel = 1;

                }
                var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ProduceLevelUpConfig>(produceLevel);
                CurrentLevel = config;
            }
            var items = CallFunByID(CurrentLevel.FunctionID);
            GameAppliaction.Singleton.NotifyReward(items);
            AddProduceTimes(1);
            //refresh all ui
            UI.UIManager.Singleton.OnUpdateUIData();
            return CurrentLevel == null ? 0f : CurrentLevel.CdTime;
        }
        private void AddProduceTimes(int time)
        {
            var currentTime = GetIntValue(PlayDataKeys.PRODUCE_CLICK_TIMES);
            currentTime+=time;
            this[PlayDataKeys.PRODUCE_CLICK_TIMES] = string.Format("{0}",currentTime);
        }

    }

    public class ProducePrisitData
    {
        [JsonName("P")]
        public int ProduceID { set; get; }

        [JsonName("S")]
        public bool IsOpen { set; get; }
    }
    public class PlayerData
    {
        public int Key { set; get; }
        public string Value { set; get; }
    }
}
