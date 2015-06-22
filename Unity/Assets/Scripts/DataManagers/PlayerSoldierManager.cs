using ExcelConfig;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataManagers
{
    public class PlayerSoldierManager : Tools.XSingleton<PlayerSoldierManager>, IPresist
    {
        public const string _SOLDIER_FILE = "_SOLDIER_FILE.json";//保存文件
        private Dictionary<int, PlayerSoldierData> _Soldiers = new Dictionary<int, PlayerSoldierData>();

        public void Presist()
        {
            var list = _Soldiers.Values.ToList();
            Tools.PresistTool.SaveJson(list, _SOLDIER_FILE);
        }

        public void Load()
        {
            var data = Tools.PresistTool.LoadJson<List<PlayerSoldierData>>(_SOLDIER_FILE);
            if (data == null)
            {
                return;
            }
            _Soldiers.Clear();
            foreach (var i in data)
            {
                _Soldiers.Add(i.SoldierID, i);
            }
        }
        /// <summary>
        /// 死亡士兵
        /// </summary>
        /// <param name="soldierID"></param>
        /// <param name="num"></param>
        public void DeadSoldier(int soldierID, int num)
        {

        }
        /// <summary>
        /// 复活士兵 
        /// </summary>
        /// <param name="soldierID"></param>
        /// <param name="num"></param>
        public void ReliveSoldier(int soldierID, int num)
        {
        }

        public int GetSoldierNum(int soldierID)
        {
            var s = this[soldierID];
            if (s == null) return 0;
            return s.Number;
        }

        public bool TrainSoldier(MonsterLvlUpConfig trainConfig)
        {
            //不存在
            var soldierConfig = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(trainConfig.LateMonster);
            if (soldierConfig == null) return false;

            var needSoldier = trainConfig.OldMonster;
            var needResources = PlayerItemManager.SplitFormatItemData(trainConfig.CostItems);
            if (trainConfig.OldMonster > 0)
            {
                if (this.GetSoldierNum(trainConfig.OldMonster) < 1)
                {
                    //按理不能进入
                    return false;
                }
            }
            var sb = new StringBuilder();
            sb.Append(LanguageManager.Singleton["NOT_ENOUGHT"]);
            bool have = true;
            foreach (var i in needResources)
            {
                if (i[1] > PlayerItemManager.Singleton.GetItemCount(i[0]))
                {
                    have = false;
                    var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i[0]);
                    if (itemConfig == null) continue;

                    string name = itemConfig.Name;
                    sb.Append(string.Format(
                        LanguageManager.Singleton["Need_Item_Count"],
                        name, i[1] - PlayerItemManager.Singleton.GetItemCount(i[0])));
                }
            }
            if (!have)
                UI.UITipDrawer.Singleton.DrawNotify(sb.ToString());
            else
            {
                foreach (var i in needResources)
                {
                    PlayerItemManager.Singleton.CalItem(i[0], i[1]);
                }
                if (trainConfig.OldMonster > 0)
                {
                    this[trainConfig.OldMonster].Number -= 1;
                }

                var newSoldier = this[trainConfig.LateMonster];
                if (newSoldier != null)
                    newSoldier.Number += 1;
                else
                {
                    this._Soldiers.Add(trainConfig.LateMonster,
                        new PlayerSoldierData
                        {
                            DeadNum = 0,
                            Number = 1,
                            SoldierID = trainConfig.LateMonster
                        }
                        );
                }


                UI.UITipDrawer.Singleton.DrawNotify(
                    string.Format(LanguageManager.Singleton["TRAIN_SOLDIER"], soldierConfig.Name));
            }
            return have;
        }
        public PlayerSoldierData this[int soldier]
        {
            get
            {
                PlayerSoldierData data;
                if (_Soldiers.TryGetValue(soldier, out data)) return data;
                return null;
            }
        }
        /// <summary>
        /// 当前士兵
        /// </summary>
        public List<PlayerSoldierData> Soldiers
        {
            get
            {
                return _Soldiers.Where(t => t.Value.Number > 0).Select(t => t.Value).ToList();
            }
        }
    }

    public class PlayerSoldierData
    {
        [JsonName("s")]
        public int SoldierID { set; get; }

        [JsonName("n")]
        public int Number { set; get; }

        [JsonName("d")]
        public int DeadNum { set; get; }

        [JsonIgnore]
        public MonsterConfig Config
        {
            get
            {
                return ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(SoldierID);
            }
        }
    }
}
