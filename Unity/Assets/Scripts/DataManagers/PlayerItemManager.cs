using Assets.Scripts.Appliaction;
using ExcelConfig;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataManagers
{
    public class PlayerItemManager : Tools.XSingleton<PlayerItemManager>
    {

        public PlayerItemManager()
        {
            _items = new Dictionary<int, PlayerGameItem>();

        }
        private Dictionary<int, PlayerGameItem> _items { set; get; }

        public PlayerGameItem this[int key]
        {
            get
            {
                PlayerGameItem item;
                if (_items.TryGetValue(key, out item)) return item;
                return null;
            }
        }

        public int GetItemCount(int entry)
        {
            var item = this[entry];
            if (item == null) return 0;
            return item.Num;
        }

        public const string _ITEM_SAVE_FILE_ = "_ITEM_.json";
        public void Load()
        {
            this._items = new Dictionary<int, PlayerGameItem>();
            var json = GameAppliaction.Singleton.ReadFile(_ITEM_SAVE_FILE_);
            if (!string.IsNullOrEmpty(json))
            {
                var list = JsonTool.Deserialize<List<PlayerGameItem>>(json);
                foreach (var i in list)
                {
                    if (_items.ContainsKey(i.ConfigID)) continue;
                    _items.Add(i.ConfigID, i);
                }
            }
        }

        public void Presist()
        {
            var items = _items.Values.ToList();
            var json = JsonTool.Serialize(items);
            GameAppliaction.Singleton.SaveFile(_ITEM_SAVE_FILE_, json, false);
        }

        internal Proto.Item AddItem(int itemID, int diff)
        {
            var item = this[itemID];
            if(diff<=0) return null;
            var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(itemID);
            if (item == null)
            {
                item = new PlayerGameItem
                {
                    Config = config,
                    ConfigID = itemID,
                    Num = diff
                };
                this._items.Add(itemID, item);
                return new Proto.Item { Diff = diff, Entry = itemID, Num = diff };
            }
            else
            {
                item.Num += diff;
                return new Proto.Item { Diff = diff, Num = item.Num, Entry = itemID };
            }
        }

        internal List<PlayerGameItem> GetAllItems()
        {
            return this._items.Values.ToList();
        }
    }

    public class PlayerGameItem 
    {
        [JsonName("N")]
        public int Num { set; get; }

        private ItemConfig config;
        
        [JsonIgnore]
        public ItemConfig Config
        {
            set { config = value; }
            get
            {
                if(config==null)
                {
                    config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(ConfigID);
                }
                return config;
            }
        }
        [JsonName("C")]
        public int ConfigID { set; get; }
        public PlayerGameItem()
        {

        }

        public PlayerGameItem(int configID, int num)
        {
            Num = num;
            ConfigID = configID;
        }
    }
}
