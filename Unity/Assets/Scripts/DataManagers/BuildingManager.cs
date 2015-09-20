using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;
using Assets.Scripts.UI;
using Proto;

namespace Assets.Scripts.DataManagers
{
    public class BuildingManager : Tools.XSingleton<BuildingManager>, IPresist
    {
        public const string BUILDING_LIST_PATH = "BUILDING_LIST.json";//已经建造的建筑

        private Dictionary<int, PlayerBuild> _ConstructBuildings = new Dictionary<int, PlayerBuild>();

        public void Load()
        {
            _ConstructBuildings.Clear();
            var data = Tools.PresistTool.LoadJson<List<PlayerBuild>>(BUILDING_LIST_PATH);
            if (data != null)
            {
                foreach (var i in data)
                {
                    if (_ConstructBuildings.ContainsKey(i.BuildID)) continue;
                    _ConstructBuildings.Add(i.BuildID, i);
                }
            }
        }

        public void Presist()
        {
            Tools.PresistTool.SaveJson(_ConstructBuildings.Values.ToList(), BUILDING_LIST_PATH);
        }

        public bool IsBuild(int id)
        {
            if (_ConstructBuildings.ContainsKey(id)) return true;
            return false;
        }

        public List<PlayerBuild> GetConstructBuildingsList()
        {
            //根据类别    
            return this._ConstructBuildings.Values.ToList();
        }

        /// <summary>
        /// 建筑
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool ConstructBuild(int buildId, int level)
        {
            GameDebug.Log("BuildID:" + buildId + " Level:" + level);
            var build = GetConfig(buildId, level);
            //var old = GetConfig(buildId, level - 1);
            if (build == null )
            {
                return false; //MaxLevel
            }

            //UI_Struct_Need_Build

            switch ((BuildingUnlockType)build.UnlockType)
            {
                case BuildingUnlockType.NeedBuild:
                    var needBuildID = Tools.UtilityTool.ConvertToInt(build.UnlockParms1);
                    var needbuildConfig = ExcelToJSONConfigManager.Current.GetConfigByID<BuildingConfig>(needBuildID);
                    if (!HaveBuild(needbuildConfig.BuildingId, needbuildConfig.Level))
                    {
                        UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["UI_Struct_Need_Build"],
                            needbuildConfig.Name, needbuildConfig.Level+1));
                        return false;
                    }
                    break;
            }

            var needs = Tools.UtilityTool.SplitKeyValues(build.CostItems,build.CostItemCounts);
            bool have = true;
            var sb = new StringBuilder();
            sb.Append(LanguageManager.Singleton["NOT_ENOUGHT"]);
            var gold = GamePlayerManager.Singleton.Gold;
            if (gold < build.CostGold)
            {
                UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["Build_NOT_ENOUGHT_GOLD"]);
                return false;
            }
            foreach (var i in needs)
            {
                if (i.Value > PlayerItemManager.Singleton.GetItemCount(i.Key))
                {
                    have = false;
                    var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                    if (itemConfig == null) continue;

                    string name = itemConfig.Name;
                    sb.Append(string.Format(
                        LanguageManager.Singleton["Need_Item_Count"],
                        name, i.Value - PlayerItemManager.Singleton.GetItemCount(i.Key)));
                }
            }

            if (have)
            {
                if (!string.IsNullOrEmpty(build.BuildSuccessMessage))
                    UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.ReplaceEc(build.BuildSuccessMessage));
                foreach (var i in needs)
                {
                    PlayerItemManager.Singleton.SubItem(i.Key, i.Value);
                }

                if (build.CostGold > 0)
                    GamePlayerManager.Singleton.SubGold(build.CostGold);
                AddOrlevelUPBuild(build.BuildingId, level);
                BuildEvent(build);
            }
            else
            {
                UI.UITipDrawer.Singleton.DrawNotify(sb.ToString());
            }

            return have;
        }

        private bool AddOrlevelUPBuild(int buildId, int level)
        {
            PlayerBuild buildData;
            if (_ConstructBuildings.TryGetValue(buildId, out buildData))
            {
                buildData.Level = level;
            }
            else
            {
                _ConstructBuildings.Add(buildId, new PlayerBuild { BuildID = buildId, Level = level });
            }
            return true;
        }

        public BuildingConfig GetConfig(int buildId, int level)
        {
            var build = ExcelToJSONConfigManager.Current.FirstConfig<BuildingConfig>((t) =>
            {
                return t.BuildingId == buildId && t.Level == level;
            });
            return build;
        }

        private bool BuildEvent(BuildingConfig levelConfig)
        {
            //ADD_PEOPLE
            //PRODUCE
            switch (levelConfig.ConstructEvent)
            {
                case "unlock_material":
                case "unlock_material_1":
                case "unlock_material_2":
                case "unlock_material_3":
                case "unlock_material_4":
                case "unlock_material_5":
                case "product_wheat": //生产道具 
                case "supply_up":
                    GamePlayerManager.Singleton.OpenProduceById(Tools.UtilityTool.ConvertToInt(levelConfig.Pars1));
                    break;
                case "add_companion": //设置出战人数
                    GamePlayerManager.Singleton.SetTeamSize(Tools.UtilityTool.ConvertToInt(levelConfig.Pars1));
                    GameDebug.LogDebug(string.Format(LanguageManager.Singleton["Build_event_add_companion"], GamePlayerManager.Singleton[PlayDataKeys.TEAM_SIZE]));
                    break;
                case "add_population":
                    GamePlayerManager.Singleton[PlayDataKeys.PEOPLE_COUNT] = Tools.UtilityTool.ConvertToInt(levelConfig.Pars1);
                    GameDebug.LogDebug(string.Format(LanguageManager.Singleton["Build_event_add_people"], GamePlayerManager.Singleton[PlayDataKeys.PEOPLE_COUNT]));
                    break;
            }

            return true;
        }


        public PlayerBuild this[int entry]
        {
            get
            {
                PlayerBuild buildData;
                if (_ConstructBuildings.TryGetValue(entry, out buildData))
                {
                    return buildData;
                }

                buildData = new PlayerBuild()
                {
                    BuildID = entry,
                    Level = 0
                };
                return buildData;
            }
        }

        public int GetBuildLevel(int entry)
        {
            var build = this[entry];
            if (build == null) return 0;
            return build.Level;
        }

        public bool HaveBuild(int buildId, int level)
        {
            PlayerBuild buildData;
            if (_ConstructBuildings.TryGetValue(buildId, out buildData))
            {
                return buildData.Level >= level;
            }
            return false;

        }

        public void Reset()
        {
            _ConstructBuildings.Clear();
            Presist();
        }

    }

    public class PlayerBuild
    {
        [JsonName("B")]
        public int BuildID { set; get; }
        [JsonName("L")]
        public int Level { set { _level = value; _Config = null; _NextLevelConfig = null; } get { return _level; } }
        private int _level;
        private BuildingConfig _Config { set; get; }

        [JsonIgnore]
        public string Name { get {
            var next = NextLevelConfig;
            if (_level == 0) return NextLevelConfig.Name;
            return Config.Name;
        } }
        [JsonIgnore]
        public string Describe
        {
            get
            {
                if (_level == 0) return NextLevelConfig.Describe;
                return Config.Describe;
            }
        }
        [JsonIgnore]
        public BuildingConfig Config
        {
            get
            {
                if (_Config == null)
                {
                    _Config = BuildingManager.Singleton.GetConfig(BuildID, Level);
                }
                return _Config;
            }
        }


        private BuildingConfig _NextLevelConfig;
        [JsonIgnore]
        public BuildingConfig NextLevelConfig
        {
            get
            {

                if (_NextLevelConfig == null)
                {
                    _NextLevelConfig = BuildingManager.Singleton.GetConfig(BuildID, Level + 1);
                }

                return _NextLevelConfig;
            }
        }

        
    }
}