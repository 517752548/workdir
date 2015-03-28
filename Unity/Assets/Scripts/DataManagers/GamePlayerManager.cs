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
        PRODUCE_CLICK_TIMES = 2,//炼金点击次数
        PEOPLE_COUNT = 3//当前中的民工数
    }
    public class GamePlayerManager : Tools.XSingleton<GamePlayerManager>, IPresist
    {
        
        //user data
        public const string PLAYER_DATA_PATH = "_PALYER_DATA_.json";
        //资源生产技术开放
        public const string PLAYER_PRODUCE_OPEN = "_PLAYER_PRODUCE_OPEN.json";
        private Dictionary<int, int> PlayerData { set; get; }

        private Dictionary<int, ProducePrisitData> ProduceOpenState { set; get; }

        public GamePlayerManager()
        {
            PlayerData = new Dictionary<int, int>();
            ProduceOpenState = new Dictionary<int, ProducePrisitData>();
        }

        public int this[PlayDataKeys key]
        {
            get
            {
                int value;
                if (PlayerData.TryGetValue((int)key, out value)) return value;
                return -1;
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
            PlayerData = new Dictionary<int, int>();
            if (list != null)
            {
                foreach (var i in list)
                {
                    PlayerData.Add(i.Key, i.Value);
                }
            }
            var data = Tools.PresistTool.LoadJson<List<ProducePrisitData>>(PLAYER_PRODUCE_OPEN);
            ProduceOpenState = new Dictionary<int, ProducePrisitData>();
            if (data!=null)
            {
                foreach (var i in data)
                {
                    if (ProduceOpenState.ContainsKey(i.ProduceID)) continue;
                    ProduceOpenState.Add(i.ProduceID, i);
                }
            }
        }

        public void Presist()
        {
            var list = PlayerData.Select(t => new PlayerData { Key = t.Key, Value = t.Value }).ToList();
            var json = JsonTool.Serialize(list);
            GameAppliaction.Singleton.SaveFile(PLAYER_DATA_PATH, json, false);
            
            var produceList = ProduceOpenState.Select(t => t.Value).ToList();
            var produceJson = JsonTool.Serialize(produceList);
            GameAppliaction.Singleton.SaveFile(PLAYER_PRODUCE_OPEN, produceJson, false);
        }

        public List<ExcelConfig.ResourcesProduceConfig> OpenProduceConfigs()
        {
            var configs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.ResourcesProduceConfig>();
            var openeds = configs.Where(t => {
                ProducePrisitData result ;
                if(ProduceOpenState.TryGetValue(t.ID, out result))
                {
                    return result.IsOpen;
                }
                return false;
            })
            .ToList();

            return openeds;
        }

        public void OpenProduceById(int id)
        {
            if(this.ProduceOpenState.ContainsKey(id))
            {
                this.ProduceOpenState[id].IsOpen = true; ;
            }else
            {
                this.ProduceOpenState.Add(id, new ProducePrisitData { IsOpen = true, PeopleNum = 0, ProduceID = id });
            }
            //保存
            Presist();
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
                var produceLevel = DataManagers.GamePlayerManager.Singleton[DataManagers.PlayDataKeys.PRODUCE_LEVEL];
                if (produceLevel <= 0)
                {
                    this[DataManagers.PlayDataKeys.PRODUCE_LEVEL] = 1;
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
            var currentTime = this[PlayDataKeys.PRODUCE_CLICK_TIMES];
            if (currentTime < 0) currentTime = 0;
            currentTime+=time;
            this[PlayDataKeys.PRODUCE_CLICK_TIMES] = currentTime;
        }

    }

    public class ProducePrisitData
    {
        [JsonName("P")]
        public int ProduceID { set; get; }

        [JsonName("S")]
        public bool IsOpen { set; get; }
        [JsonName("N")]
        public int PeopleNum { set; get; }
    }
    public class PlayerData
    {
        public int Key { set; get; }
        public int Value { set; get; }
    }
}
