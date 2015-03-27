using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;

namespace Assets.Scripts.DataManagers
{
    public class BuildingManager : Tools.XSingleton<BuildingManager>
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

        public void Save()
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

        public bool ConstructBuild(int entry, int level)
        {
            var build = ExcelToJSONConfigManager.Current.GetConfigByID<BuildingConfig>(entry);
            var levelConfig = ExcelToJSONConfigManager.Current.FirstConfig<BuildingLevelConfig>((t) => {
                if (t.BuildID == entry && t.Level == level) return true;
                return false;
            });
            if(build ==null || levelConfig ==null)
            {
                return false; //MaxLevel
            }
            var needs = PlayerItemManager.SplitFormatItemData( levelConfig.LevelUpRequire);
            bool have = true;
            var sb = new StringBuilder();
            sb.Append(LanguageManager.Singleton["NOT_ENOUGHT"]);
            foreach(var i in needs)
            {
                if(i[1]> PlayerItemManager.Singleton.GetItemCount(i[0]))
                {
                    have = false;
                    var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i[0]);
                    if(itemConfig==null) continue;

                    string name = itemConfig.Name;
                    sb.Append(string.Format(
                        LanguageManager.Singleton["Need_Item_Count"],
                        name, i[1] - PlayerItemManager.Singleton.GetItemCount(i[0])));
                }
               
            }

            if(have)
            {
                foreach(var i in needs)
                {
                    PlayerItemManager.Singleton.CalItem(i[0],i[1]);
                }
                PlayerBuild buildData ;
                if(_ConstructBuildings.TryGetValue(entry,out buildData))
                {
                    buildData.Level = level;
                }else{
                    _ConstructBuildings.Add(entry, new PlayerBuild{ BuildID = entry, Level = level});
                }
                BuildEvent(levelConfig);
            }
            else
            {
                UI.UITipDrawer.Singleton.DrawNotify(sb.ToString());
            }

            return have;
        }

        private bool BuildEvent(BuildingLevelConfig levelConfig)
        {
            return false;
        }

        public PlayerBuild this[int entry]
        {
            get {
                PlayerBuild buildData;
                if (_ConstructBuildings.TryGetValue(entry, out buildData))
                {
                    return buildData;
                }
                return null;
            }
        }
    }

    public class PlayerBuild
    {
        [JsonName("B")]
        public int BuildID { set; get; }
        [JsonName("L")]
        public int Level { set; get; }

        private ExcelConfig.BuildingConfig _Config { set; get; }
        [JsonIgnore]
        public ExcelConfig.BuildingConfig Config
        {
            get
            {
                if (_Config == null)
                {
                    _Config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.BuildingConfig>(BuildID);
                }
                return _Config;
            }
        }
        private ExcelConfig.BuildingLevelConfig _LevelConfig;
        [JsonIgnore]
        public ExcelConfig.BuildingLevelConfig LevelConfig
        {
            get
            {
                if (_LevelConfig == null)
                {
                    _LevelConfig = ExcelConfig.ExcelToJSONConfigManager.Current.FirstConfig<ExcelConfig.BuildingLevelConfig>(t =>
                    {
                        return t.BuildID == BuildID && t.Level == Level;
                    });
                }
                return _LevelConfig;
            }
        }

        private ExcelConfig.BuildingLevelConfig _NextLevelConfig;
        [JsonIgnore]
        public ExcelConfig.BuildingLevelConfig NextLevelConfig
        {
            get
            {
                if (_NextLevelConfig == null) {
                    _NextLevelConfig = ExcelConfig.ExcelToJSONConfigManager.Current.FirstConfig<ExcelConfig.BuildingLevelConfig>(t =>
                    {
                        return t.BuildID == BuildID && t.Level == Level+1;
                    });
                }
                return _NextLevelConfig;
            }
        }
    }
}
