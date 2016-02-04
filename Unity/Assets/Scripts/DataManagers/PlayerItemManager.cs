using Assets.Scripts.App;
using Assets.Scripts.UI;
using ExcelConfig;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Proto;

namespace Assets.Scripts.DataManagers
{

	public class ShopPersistData
	{
		[JsonName("i")]
		public int entry{set;get;}
		[JsonName("n")]
		public int count{ set; get; }
	}

	public class ScrectShopData
	{

		public ScrectShopData()
		{
			ShopData = new List<ShopPersistData> ();
		}
		[JsonName("id")]
		public int ShopID{set;get;}

		[JsonIgnore]
		public Dictionary<int, int> Items{ set; get; }
		 
		[JsonName("item")]
		public List<ShopPersistData> ShopData{ 
			set{ 
				Items = new Dictionary<int, int> ();
				foreach (var i in value) {
					if(Items.ContainsKey(i.entry))continue;
					Items.Add (i.entry, i.count);
				}
			} 
			get{
				return Items.Select (t => new ShopPersistData{ entry = t.Key, count = t.Value }).ToList ();
			} 
		}

		public  void AddItem(int entry,int count)
		{
			if (Items == null)
				Items = new Dictionary<int, int> ();
			if (Items.ContainsKey (entry))
				Items [entry] += count;
			else
				Items.Add (entry, count);
		}

		public int GetBuyCount(int entry)
		{
			if (Items.ContainsKey (entry))
				return Items [entry]; 
			return 0;
		}
	}


    public class PlayerItemManager : Tools.XSingleton<PlayerItemManager>, IPresist
    {

        public PlayerItemManager()
        {
            _items = new Dictionary<int, PlayerGameItem>();
        }
        private Dictionary<int, PlayerGameItem> _items { set; get; }
		private Dictionary<int,PlayerGameItem> _packageItems = new Dictionary<int, PlayerGameItem> ();

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
		public const string _PACKAGE_SAVE_FILE= "_PLAY_PACKAGE.json";

		public const string _GOLD_SHOP_DATA_FILE="_GOLD_SHOP_BUY_COUNT.json";
		public const string _COIN_SHOP_DATA_FILE="_COIN_SHOP_BUY_COUNT.json";

		public const string _SRECT_SHOP_DATA_FILE="_SCRECT_SHOP_COUNT.json";

		private Dictionary<int,int> _coinShop = new Dictionary<int, int>();
		private Dictionary<int,int> _goldShop = new Dictionary<int, int>();

		private Dictionary<int,ScrectShopData> _screctShop = new Dictionary<int, ScrectShopData>();

        public void Load()
        {
            this._items = new Dictionary<int, PlayerGameItem>();
            var list = Tools.PresistTool.LoadJson<List<PlayerGameItem>>(_ITEM_SAVE_FILE_);
			if (list != null) {
				foreach (var i in list) {
					if (_items.ContainsKey (i.ConfigID))
						continue;
					_items.Add (i.ConfigID, i);
				}
			}
			_packageItems.Clear ();
			var pack = Tools.PresistTool.LoadJson<List<PlayerGameItem>> (_PACKAGE_SAVE_FILE);
			if (pack != null) {
				foreach (var i in pack) {
					this._packageItems.Add (i.ConfigID, i);
				}
			}

			_coinShop.Clear ();
			var coinData = Tools.PresistTool.LoadJson<List<ShopPersistData>> (_COIN_SHOP_DATA_FILE);
			if (coinData != null) {
				foreach (var i in coinData) 
				{
					_coinShop.Add (i.entry, i.count);
				}
			}
			_goldShop.Clear ();
			var goldShop = Tools.PresistTool.LoadJson<List<ShopPersistData>> (_GOLD_SHOP_DATA_FILE);
			if (goldShop != null) {
				foreach (var i in goldShop) {
					_goldShop.Add (i.entry, i.count);
				}
			}

			_screctShop.Clear ();
			var screctShop = Tools.PresistTool.LoadJson<List<ScrectShopData>> (_SRECT_SHOP_DATA_FILE);
			if (screctShop != null) {
				foreach (var  i in screctShop) {
					_screctShop.Add (i.ShopID, i);
				}
			}
        }

		public int GetGoldShopCount(ExcelConfig.StoreDataConfig config)
		{
			if (this._goldShop.ContainsKey (config.ID))
				return this._goldShop [config.ID];
			return 0;
		}

		public void BuyGoldShopItem(ExcelConfig.StoreDataConfig config)
		{
			if (this._goldShop.ContainsKey (config.ID)) {
				this._goldShop [config.ID] += 1;
			} else {
				this._goldShop.Add (config.ID, 1);
			}
		}

		public int GetCoinShopCount(ExcelConfig.DimondStoreConfig config)
		{
			if (this._coinShop.ContainsKey (config.ID))
				return this._coinShop [config.ID];
			return 0;
		}

		public void BuyCoinShopItem(ExcelConfig.DimondStoreConfig config)
		{
			if (this._coinShop.ContainsKey (config.ID)) {
				this._coinShop [config.ID]+=1;
			} else {
				this._coinShop.Add (config.ID, 1);
			}
		}

		public bool BuyScrectShopItem(ExcelConfig.SecretStoreConfig config)
		{
			ScrectShopData shop;
			if (!_screctShop.TryGetValue (config.Store_id, out shop)) {
				shop = new ScrectShopData {ShopID = config.Store_id };
				_screctShop.Add (shop.ShopID, shop);
			} 

			if (config.Max_purchase_times > 0) {
				if (config.Max_purchase_times <= shop.GetBuyCount (config.ID))
					return false;
			}
			switch ((Proto.EmployCostCurrent)config.current_type) 
			{
			case EmployCostCurrent.Coin:
				if (!(GamePlayerManager.Singleton.Coin >= config.Sold_price)) {
					UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["BUY_ITEM_NO_ENOUGH_COIN"]);
					return false;
				}
				GamePlayerManager.Singleton.SubCoin (config.Sold_price);
				PlayerItemManager.Singleton.AddItem (config.item_id, 1);
				//COST_COIN_REWARD_ITEM
				UIControllor.Singleton.ShowMessage(
					string.Format(LanguageManager.Singleton["COST_COIN_REWARD_ITEM"],
						config.Sold_price, config.item_name, 1));
				break;
			case EmployCostCurrent.Gold:
				if (!(GamePlayerManager.Singleton.Gold >= config.Sold_price)) {
					UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["BUY_ITEM_NO_ENOUGH_GOLD"]);
					return false;
				}
				GamePlayerManager.Singleton.SubGold (config.Sold_price);
				PlayerItemManager.Singleton.AddItem (config.item_id, 1);
				UIControllor.Singleton.ShowMessage(
					string.Format(LanguageManager.Singleton["COST_GOLD_REWARD_ITEM"],
						config.Sold_price, config.item_name, 1));
				break;
			}

			shop.AddItem (config.ID, 1);
			return true;
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

		public int GetScrectShopCount(int shopID, int entry)
		{
			ScrectShopData shop;
			if (_screctShop.TryGetValue (shopID, out shop)) {
				return shop.GetBuyCount (entry);
			}
			return 0;
		}
        public void Presist()
        {
            var items = _items.Values.ToList();
			var pack = _packageItems.Values.ToList ();
			var goldBuy = _goldShop.Select (t => new ShopPersistData{ entry = t.Key, count = t.Value }).ToList ();
			var coinBuy = _coinShop.Select (t => new ShopPersistData{ entry = t.Key, count = t.Value }).ToList ();
			var screctBuy = _screctShop.Values.ToList ();
            Tools.PresistTool.SaveJson(items, _ITEM_SAVE_FILE_);
			Tools.PresistTool.SaveJson (pack, _PACKAGE_SAVE_FILE);
			Tools.PresistTool.SaveJson (goldBuy, _GOLD_SHOP_DATA_FILE);
			Tools.PresistTool.SaveJson (coinBuy, _COIN_SHOP_DATA_FILE);
			Tools.PresistTool.SaveJson (screctBuy, _SRECT_SHOP_DATA_FILE);
        }

        public void Reset()
        {
            _items.Clear();
			_packageItems.Clear ();
			_coinShop.Clear ();
			_goldShop.Clear ();
            Presist();
        }

        internal Proto.Item AddItem(int itemID, int diff)
        {
            var item = this[itemID];
            if (diff <= 0) return null;
            var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(itemID);

			switch ((Proto.ItemType)config.Category) {
			case ItemType.Indenture:
				UITipDrawer.Singleton.DrawNotify (config.Desription);
				break;
			case ItemType.Book:
				var skillID = Tools.UtilityTool.ConvertToInt (config.Pars1);
				GamePlayerManager.Singleton.AddPlayerSkill (skillID);
				UITipDrawer.Singleton.DrawNotify (config.Desription);
				break;
			case Proto.ItemType.GoldPackage:
				var gold = Tools.UtilityTool.ConvertToInt (config.Pars1);
				GamePlayerManager.Singleton.AddGold (gold * diff);
				return new Proto.Item{ Entry = itemID, Num =0, Diff =0 };
			case Proto.ItemType.Diagram:
				if(GetItemCount(itemID)==0){
					var makeConfig = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>
						(Tools.UtilityTool.ConvertToInt (config.Pars1));
					UI.UITipDrawer.Singleton.DrawNotify (string.Format(LanguageManager.Singleton["YOU_CAN_MAKE"],makeConfig.Name));
					}
				break;
			case Proto.ItemType.Tools:
				var itemToolType = (ToolItemType)Tools.UtilityTool.ConvertToInt (config.Pars1);

				switch (itemToolType) {
				case ToolItemType.AddFoodChargeValue:
					GamePlayerManager.Singleton.AddFoodChargeAppend (Tools.UtilityTool.ConvertToInt (config.Pars2));
					UITipDrawer.Singleton.DrawNotify (config.Desription);
					break;
				case ToolItemType.AddGoldProduce:
					GamePlayerManager.Singleton.AddGoldProduceAppend (Tools.UtilityTool.ConvertToInt (config.Pars2));
					UITipDrawer.Singleton.DrawNotify (config.Desription);
					break;
				case ToolItemType.AddPackageSize:
					GamePlayerManager.Singleton.AddPackageSize (Tools.UtilityTool.ConvertToInt (config.Pars2));
					UITipDrawer.Singleton.DrawNotify (config.Desription);
					break;
				case ToolItemType.AddProducesOffTime:
					var times = Tools.UtilityTool.ConvertToInt (config.Pars2);
					GamePlayerManager.Singleton.SetOfflineMaxTime (times);
					UITipDrawer.Singleton.DrawNotify (config.Desription);
					break;
				case ToolItemType.AddWheatProduce: //No support
					
					break;
				case ToolItemType.CalProuduceTime:
					var timeOfProduce = Tools.UtilityTool.ConvertToInt (config.Pars2);
					GamePlayerManager.Singleton.SetRewardTime (timeOfProduce);
					UITipDrawer.Singleton.DrawNotify (config.Desription);
					break;
				default:
					break;
				}
				break;
			}

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
			var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(config.ItemId);
            var price = config.Sold_price;
            var gold = GamePlayerManager.Singleton.Gold;
            if (gold < price)
            {
                UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["BUY_ITEM_NO_ENOUGH_GOLD"]);
                return false;
            }

            GamePlayerManager.Singleton.SubGold(price);

			PlayerItemManager.Singleton.AddItem(config.ItemId, 1);
			PlayerItemManager.Singleton.BuyGoldShopItem (config);
			UIControllor.Singleton.ShowMessage(
				string.Format(LanguageManager.Singleton["COST_GOLD_REWARD_ITEM"],
					price, itemconfig.Name, 1));
            return true;
        }


		public bool BuyItemUseCoin(ExcelConfig.DimondStoreConfig config)
		{
			var entry = Tools.UtilityTool.ConvertToInt (config.ItemKey);
			var itemconfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(
				entry);
			var price = config.Sold_price;
			var coin = GamePlayerManager.Singleton.Coin;
			if (coin < price)
			{
				UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["BUY_ITEM_NO_ENOUGH_COIN"]);
				return false;
			}

			GamePlayerManager.Singleton.SubCoin(price);

			PlayerItemManager.Singleton.AddItem(entry, 1);
			PlayerItemManager.Singleton.BuyCoinShopItem (config);
			UIControllor.Singleton.ShowMessage(
				string.Format(LanguageManager.Singleton["COST_COIN_REWARD_ITEM"],
					price, itemconfig.Name, 1));
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
				var sb = new StringBuilder ();
                foreach (var i in rewardItems)
                {
                    AddItem(i.Config.ID, i.Num);
					sb.Append(
						string.Format(LanguageManager.Singleton["REWARD_ITEM"], i.Config.Name, i.Num));
                }
				UIControllor.Singleton.ShowMessage (sb.ToString ());
                return true;
            }

        }
			
		public int CurrentSize{ get{
				int count = 0;
				foreach (var i in _packageItems) {
					count += i.Value.Num;
				}
				return count;
			}}

		public bool AddItemIntoPack(int entry,int num)
		{
			if (num <= 0)
				return false;
			if (CurrentSize+num > GamePlayerManager.Singleton.PackageSize)
				return false;
			if (_packageItems.ContainsKey (entry)) {
				_packageItems [entry].Num += num;
			} else {
				_packageItems.Add (entry, new PlayerGameItem(entry,num));
			}
			return true;
		}

		public bool CalItemFromPack(int entry,int num)
		{
			if (num <= 0)
				return false;
			if (_packageItems.ContainsKey (entry)) {
				if (_packageItems [entry].Num >= num) {
					_packageItems [entry].Num -= num; return true;
				}

			}
			return false;
		}

		public int GetFoodNum()
		{
			int foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;
			if (_packageItems.ContainsKey (foodEntry))
				return _packageItems [foodEntry].Num;
			return 0;
		}
			

		public List<PlayerGameItem> PackageToList{ get { return _packageItems.Values.ToList (); } }

		public void JoinCastle()
		{
			if (_packageItems.Count == 0)
				return;
			
			StringBuilder sb = new StringBuilder ();

			foreach (var i in _packageItems) {
				this.AddItem (i.Key, i.Value.Num);
				sb.Append( string.Format (LanguageManager.Singleton ["REWARD_ITEM"], 
					i.Value.Config.Name, i.Value.Num));
			}

			_packageItems.Clear ();
			var str = string.Format (LanguageManager.Singleton ["GET_FROM_EXPLORE"], sb.ToString ());
			//UI.UITipDrawer.Singleton.DrawNotify(str);
			UI.UIControllor.Singleton.ShowMessage (str);
		}

		public void EmptyPackage()
		{
			_packageItems.Clear ();
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
