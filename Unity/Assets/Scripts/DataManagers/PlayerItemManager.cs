using Assets.Scripts.App;
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
    public class PlayerItemManager : Tools.XSingleton<PlayerItemManager>, IPresist
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

        [System.Obsolete]
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
            var list = Tools.PresistTool.LoadJson<List<PlayerGameItem>>(_ITEM_SAVE_FILE_);
            if (list == null) return;
            foreach (var i in list)
            {
                if (_items.ContainsKey(i.ConfigID)) continue;
                _items.Add(i.ConfigID, i);
            }

        }

        /// <summary>
        /// 消耗道具
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="calValue"></param>
        /// <returns></returns>
        public int SubItem(int entry, int calValue)
        {
            if (calValue < 0) return -1;
            PlayerGameItem item;
            if (_items.TryGetValue(entry, out item))
            {
                    if (calValue <= 0) return item.Num;
                    if (item.Num >= calValue)
                    {
                        item.Num -= calValue;
                    }
                    return item.Num;

            }

            return -1;
        }

        public void Presist()
        {
            var items = _items.Values.ToList();
            Tools.PresistTool.SaveJson(items, _ITEM_SAVE_FILE_);
        }

        public void Reset()
        {
            _items.Clear();
            Presist();
        }

        internal Proto.Item AddItem(int itemID, int diff)
        {
            var item = this[itemID];
            if (diff <= 0) return null;
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
            return this._items.Values.Where(t => t.Num > 0).ToList();
        }

        internal bool BuyItem(ExcelConfig.StoreDataConfig config)
        {
            var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(config.ID);
            var price = config.Sold_price;
            var gold = GamePlayerManager.Singleton.Gold;
            if (gold < price)
            {
                UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["BUY_ITEM_NO_ENOUGH_GOLD"]);
                return false;
            }

            GamePlayerManager.Singleton.SubGold(price);

            PlayerItemManager.Singleton.AddItem(config.ID, 1);
            UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["COST_GOLD_REWARD_ITEM"],price, itemconfig.Name, 1));
            return true;
        }

        internal bool MakeItem(MakeConfig config)
        {
            var needs = Tools.UtilityTool.SplitKeyValues(config.RequireItems,config.RequireItemsNumber);
            var rewards = Tools.UtilityTool.SplitKeyValues(config.RewardItems,config.RewardItemsNumber);
            var needItems = needs.Select(t => new
            {
                Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(t.Key),
                Num = t.Value
            }).ToList();
            var rewardItems = rewards.Select(t => new
            {
                Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(t.Key),
                Num = t.Value
            }).ToList();

            int gold = GamePlayerManager.Singleton.Gold;

            if (gold < config.RequireGold)
            {
                UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["MAKE_NOT_ENOUGHT_GOLD"]);
                return false;
            }

            ItemConfig notEnought = null;

            foreach (var i in needItems)
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
                    string.Format(LanguageManager.Singleton["MAKE_NOT_ENOUGHT"], notEnought.Name));
                return false;
            }
            else
            {
                foreach (var i in needItems)
                {
                    SubItem(i.Config.ID, i.Num);
                }
                if (config.RequireGold > 0)
                    GamePlayerManager.Singleton.SubGold(config.RequireGold);
                foreach (var i in rewardItems)
                {
                    AddItem(i.Config.ID, i.Num);
                    UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["REWARD_ITEM"], i.Config.Name, i.Num));
                }
                return true;
            }

        }

        public void NotifyReward(List<Proto.Item> items)
        {
            foreach (var i in items)
            {
                var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(i.Entry);
                UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["REWARD_ITEM"], config.Name, i.Diff));
            }
            //throw new NotImplementedException();
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
