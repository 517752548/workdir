using Assets.Scripts.Appliaction;
using Assets.Scripts.UI;
using ExcelConfig;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

        public static List<int[]> SplitFormatItemData(string format)
        {
            if (format == "-") return new List<int[]>();
            var splits = format.Split('|');
            var produces = new List<int[]>();
            foreach (var i in splits)
            {
                var strSplit = i.Split(':');
                if (strSplit.Length != 2)
                {
                    Debug.LogError(string.Format("Format:{0} error!", format));
                    continue;
                }
                produces.Add(new int[] { Convert.ToInt32(strSplit[0]), Convert.ToInt32(strSplit[1]) });
            }
            return produces;
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

        public int CalItem(int entry,int calValue)
        {
          
            PlayerGameItem item;
            if(_items.TryGetValue(entry,out item))
            {
                if (calValue <= 0) return item.Num;
                item.Num -= calValue;
                return item.Num;
            }

            return 0;
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
            return this._items.Values.Where(t=>t.Num>0).ToList();
        }

        internal bool BuyItem(ShopConfig config)
        {
            var require = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(config.RequireItem);
            var buy = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(config.Item);
            if(require==null)
            {
                Debug.LogError(string.Format("Item:{0} not exists!", config.RequireItem));
                return false;
            }
            var haveCount = GetItemCount(require.ID);
            if(haveCount>= config.RequireNum)
            {
                CalItem(config.RequireItem, config.RequireNum);
                AddItem(config.Item, 1);
                UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["REWARD_ITEM"], buy.Name, 1));
                return true;
            }
            else
            {
                UI.UITipDrawer.Singleton.DrawNotify(
                    string.Format(LanguageManager.Singleton["BUY_ITEM_NO_ENOUGH"],require.Name,config.RequireNum));
                return false;
            }
        }

        internal bool MakeItem(MakeConfig config)
        {
            var needs = DataManagers.PlayerItemManager.SplitFormatItemData(config.RequireItems);
            var rewards = DataManagers.PlayerItemManager.SplitFormatItemData(config.RewardItems);
            var needItems = needs.Select(t => new 
            {
                Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(t[0]),
                Num = t[1]
            }).ToList();
            var rewardItems = rewards.Select(t => new 
            {
                Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(t[0]),
                Num = t[1]
            }).ToList();

            ItemConfig notEnought =null;
 
            foreach(var i in needItems)
            {
                if (i.Num > GetItemCount(i.Config.ID))
                {
                    notEnought = i.Config;
                    break;
                }
            }
            //不够
            if (notEnought != null) 
            {
                UITipDrawer.Singleton.DrawNotify(
                    string.Format(LanguageManager.Singleton["MAKE_NOT_ENOUGHT"],notEnought.Name));
                return false;
            }
            else
            {
                foreach(var i in needItems)
                {
                    CalItem(i.Config.ID, i.Num);
                }

                foreach(var i in rewardItems)
                {
                    AddItem(i.Config.ID, i.Num);
                    UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["REWARD_ITEM"], i.Config.Name, i.Num));
                }
                return true;
            }

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
