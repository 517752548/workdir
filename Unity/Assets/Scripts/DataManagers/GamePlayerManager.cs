﻿using Assets.Scripts.App;
using Assets.Scripts.UI;
using ExcelConfig;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Proto;
using Assets.Scripts.UI.Windows;

namespace Assets.Scripts.DataManagers
{
	public enum BattleControlMode
	{
		AUTO = 0,
		PLAYER = 1
	}

	public class ProducePrisitData
	{
		[JsonName ("P")]
		public int ProduceID { set; get; }

		[JsonName ("S")]
		public bool IsOpen { set; get; }

		[JsonName ("N")]
		public int PeopleNum { set; get; }
	}

	public class PlayerData
	{
		[JsonName ("K")]
		public int Key { set; get; }

		[JsonName ("V")]
		public int Value { set; get; }
	}
	/// <summary>
	/// 储存玩家信心
	/// </summary>
	public enum PlayDataKeys
	{
		PRODUCE_LEVEL = 1,//玩家的炼金等级
		PRODUCE_CLICK_TIMES = 2,//炼金点击次数
		PEOPLE_COUNT = 3,//当前中的民工数
		PRODUCE_TIME = 4,//上次计算时间
		PACKAGE_SIZE = 5,//背包大小
		TEAM_SIZE = 6,//队伍上线
		EXPLORE_VALUE = 7,//探索度
		PLAYER_GOLD = 8,//金币
		PLAYER_COIN = 9,//钻石
		PLAYER_CURRENT_MAP = 10,//当前地图
		PLAYER_CURREN_POS = 11,//当前地图所在坐标
		PLAYER_LAST_FOOD =12,//当前所带食物
		PLAYER_BATTLE_MODE = 13,//当前战斗模式
		PLAYER_ACHIEVEMENT_POINT = 14,//成就点
		MUSIC_OFF = 15,//音乐
		EFFECT_MUSIC = 16,//音效
		FoodChargeAppendValue = 17,//干粮加血附加值
		GoldProduceAppendValue = 18,//点金附加值
		CalProuduceTime = 19,//游戏产量计算时间
		WheatProduceAppend = 20,//小麦产量附加值
		ProductOffLineTimeAppend =21, //离线产量计算时间
		NoCDWhenBattleStart = 22 ,//战斗开始没有cd时间
		GuideStep = 23, //引导
		RandomEventTime = 24 ,//上次随机事件时间
		ExploreView = 25,//当前视野
		TeamHPAppend = 26,//血量附加
		TeamDamage = 27 ,//伤害附加
		TeamDamageSub = 28,//队伍伤害减免
		TeamPrecision = 29,//命中
		TeamJouk = 30,//闪避
		GiveFoodTimes = 31,//空干粮补满
	}

	public class GamePlayerManager : Tools.XSingleton<GamePlayerManager>, IPresist
	{
		private const int CELL = 10000;

		private static DateTime TimeZero = new DateTime (1970, 1, 1, 0, 0, 0);
		//user data
		public const string PLAYER_DATA_PATH = "_PALYER_DATA_.json";
		//资源生产技术开放
		public const string PLAYER_PRODUCE_OPEN = "_PLAYER_PRODUCE_OPEN.json";
		//探索过的地图


		public const string PAYMENT_PATH = "/PaymentData.json";

		public const string PLAYER_SKILLS = "_PLAYER_SKILL.json"; //No completed

		private Dictionary<int, int> PlayerData { set; get; }

		private Dictionary<int, ProducePrisitData> ProduceOpenState { set; get; }



		/// <summary>
		/// 从x，y到index
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static int PosXYToIndex (int x, int y)
		{
			return x + y * CELL;
		}

		/// <summary>
		/// 从index 到x，y
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public static Vector2 IndexToPos (int index)
		{
			return new Vector2 (index % CELL, (index - (index % CELL)) / CELL);
		}

		public GamePlayerManager ()
		{
			PlayerData = new Dictionary<int, int> ();
			ProduceOpenState = new Dictionary<int, ProducePrisitData> ();
		}

		public int this [PlayDataKeys key] {
			get {
				int value;
				if (PlayerData.TryGetValue ((int)key, out value))
					return value;
				return -1;
			}
			set {
				PlayerData [(int)key] = value;          
			}
		}



		public void Load ()
		{
			var list = Tools.PresistTool.LoadJson<List<PlayerData>> (PLAYER_DATA_PATH);
			PlayerData = new Dictionary<int, int> ();
			if (list != null) {
				foreach (var i in list) {
					PlayerData.Add (i.Key, i.Value);
				}
			}
			var data = Tools.PresistTool.LoadJson<List<ProducePrisitData>> (PLAYER_PRODUCE_OPEN);
			ProduceOpenState = new Dictionary<int, ProducePrisitData> ();
			if (data != null) {
				foreach (var i in data) {
					if (ProduceOpenState.ContainsKey (i.ProduceID))
						continue;
					ProduceOpenState.Add (i.ProduceID, i);
				}
			}

		}

		public void Presist ()
		{
			var list = PlayerData.Select (t => new PlayerData { Key = t.Key, Value = t.Value }).ToList ();
			Tools.PresistTool.SaveJson (list, PLAYER_DATA_PATH);

			var produceList = ProduceOpenState.Select (t => t.Value).ToList ();
			Tools.PresistTool.SaveJson (produceList, PLAYER_PRODUCE_OPEN);
		}

		public void Reset ()
		{
			PlayerData.Clear ();
			ProduceOpenState.Clear ();
			Presist ();
		}

		public int EventLastTime{
			get{ 
				var eventTime = this [PlayDataKeys.RandomEventTime];
				if (eventTime <= 0) {
					SetEventTimeTo (10 * 60);
				}

				return this [PlayDataKeys.RandomEventTime];
			}

			set{
				this [PlayDataKeys.RandomEventTime] = value;
			}
		}

		public bool IsEventTimeout{ 
			get {
				var ticks = DateTime.UtcNow - TimeZero;
				if (ticks.TotalSeconds > this.EventLastTime) 
				{
					return true;
				}
				return false;
			} 
		}

		public void SetEventTimeTo(int pastOfSeconds)
		{
			var ticks = DateTime.UtcNow - TimeZero; 
			var seconds = (int)ticks.TotalSeconds;
			EventLastTime = seconds + pastOfSeconds;

		}

		#region 生产相关

		public int People {
			get {
				return Mathf.Max (0, this [PlayDataKeys.PEOPLE_COUNT]);
			}
		}

		public int BusyPeople {
			get {
				var busy = 0;
				foreach (var i in ProduceOpenState) {
					busy += i.Value.PeopleNum;
				}
				return busy;
			}
		}

		public List<ExcelConfig.ResourcesProduceConfig> OpenProduceConfigs ()
		{
			var configs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.ResourcesProduceConfig> ();
			var openeds = configs.Where (t => {
				ProducePrisitData result;
				if (ProduceOpenState.TryGetValue (t.ID, out result)) {
					return result.IsOpen;
				}
				return false;
			})
            .ToList ();

			return openeds;
		}

		public void OpenProduceById (int id)
		{
			if (this.ProduceOpenState.ContainsKey (id)) {
				this.ProduceOpenState [id].IsOpen = true;
				;
			} else {
				this.ProduceOpenState.Add (id, new ProducePrisitData { IsOpen = true, PeopleNum = 0, ProduceID = id });
			}

			var name = string.Empty;

			var config = ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ResourcesProduceConfig> (id);
			if (config != null) {
				name = config.Name;
			}
			GameDebug.LogDebug (string.Format (LanguageManager.Singleton ["Build_event_open_produce"], name));
			//保存
			Presist ();
		}

		/// <summary>
		/// 附加的血量
		/// </summary>
		/// <value>The food charge append.</value>
		public int FoodChargeAppend{
			get{ 
				var value = this [PlayDataKeys.FoodChargeAppendValue];
				if (value <= 0)
					value = 0;
				return value;
			}
		}

		public void AddFoodChargeAppend(int value)
		{
			var cur = this.FoodChargeAppend;
			this [PlayDataKeys.FoodChargeAppendValue] = cur + value;
		}

		public void AddGoldProduceAppend(int append)
		{
			var goldAppend = this [PlayDataKeys.GoldProduceAppendValue];
			if (goldAppend <= 0) {
				goldAppend = 0;
			}
			this [PlayDataKeys.GoldProduceAppendValue] = goldAppend + append;
		}

		public int GoldProduceAppend {
			get {
				var goldAppend = this [PlayDataKeys.GoldProduceAppendValue];
				if (goldAppend <= 0) {
					goldAppend = 0;
				}

				return goldAppend;
			}
		}

		public bool IsNoCdWhenBattleBegin{ 
			get { return this [PlayDataKeys.NoCDWhenBattleStart] == 1; } 
			set 
			{
				this [PlayDataKeys.NoCDWhenBattleStart] = value ? 1 : 0;
			} 
		}

		internal float CallProduceGold (out int addNum)
		{
			var goldProduce = App.GameAppliaction.Singleton.ConstValues.GoldProduceLvl1;// GetGoldProduce();
			goldProduce += GoldProduceAppend;
			addNum = goldProduce;
			this.AddGold (goldProduce);
			UITipDrawer.Singleton.DrawNotify (string.Format (LanguageManager.Singleton ["ProduceGoldPreTick"],
				goldProduce));
			AddProduceTimes (1);
			UI.UIManager.Singleton.UpdateUIData ();
			AchievementManager.Singleton.ProduceGold (goldProduce);
			return (float)App.GameAppliaction.Singleton.ConstValues.GoldProduceLvl1CD / 1000f;
		}

		private void AddProduceTimes (int time)
		{
			var currentTime = this [PlayDataKeys.PRODUCE_CLICK_TIMES];
			if (currentTime < 0)
				currentTime = 0;
			currentTime += time;
			this [PlayDataKeys.PRODUCE_CLICK_TIMES] = currentTime;
		}

		public int OfflineMaxTime{
			get{
				var max = this [PlayDataKeys.ProductOffLineTimeAppend];
				if (max <= GameAppliaction.Singleton.ConstValues.OutLineRewTime) {
					return GameAppliaction.Singleton.ConstValues.OutLineRewTime;
				}
				return max;
			}
		}

		public void SetOfflineMaxTime(int time)
		{
			var offMaxTimes = this.OfflineMaxTime;
			this [PlayDataKeys.ProductOffLineTimeAppend] = time;// GameAppliaction.Singleton.ConstValues.OutLineRewTime

		}

		public bool CalProduce ()
		{
			if (TimeToProduce.TotalSeconds > 0)
				return false;

			var timeTickForProduce = TimeSpan.FromMilliseconds (GameAppliaction.Singleton.ConstValues.ProduceRewardTick);

			var times = Mathf.Abs ((float)TimeToProduce.TotalSeconds) / timeTickForProduce.TotalSeconds;

			this [PlayDataKeys.PRODUCE_TIME] = (int)(DateTime.UtcNow - TimeZero).TotalSeconds;

			var dict = new Dictionary<int, Item> ();
			var maxTime = OfflineMaxTime /
			              GameAppliaction.Singleton.ConstValues.ProduceRewardTick;

			if (times > maxTime)
				times = maxTime;
			
			for (var t = times; t > 0; t--) 
			{
				foreach (var i in ProduceOpenState) {
					//不存在
					if (!i.Value.IsOpen || i.Value.PeopleNum <= 0)
						continue;
					
					var produce = ExcelToJSONConfigManager.Current.GetConfigByID<ResourcesProduceConfig> (i.Value.ProduceID);
					if (produce == null)
						continue;
					var requires = Tools.UtilityTool.SplitKeyValues (produce.CostItems, produce.CostItemsNumber);
					var rewards = Tools.UtilityTool.SplitKeyValues (produce.RewardItems, produce.RewardItemsNumber);
					var engough = true;

					foreach (var r in requires) 
					{
						var haveCount = PlayerItemManager.Singleton.GetItemCount (r.Key);
						var need = r.Value * i.Value.PeopleNum;
						Debug.LogFormat ("Have:{0} need:{1}", haveCount, need);
						if (haveCount<need) 
						{
							engough = false;
							break;
						}
					}
					//不够
					if (!engough)
						continue;
					foreach (var r in requires) 
					{
						var diffNum = (r.Value * i.Value.PeopleNum);
						PlayerItemManager.Singleton.SubItem (r.Key, diffNum);
						if (dict.ContainsKey (r.Key)) {				
							dict [r.Key].Diff -= diffNum;// item.Diff;
						} else {
							dict.Add (r.Key, new Item{ Entry = i.Key, Diff = -diffNum });
						}

					}
					foreach (var r in rewards) 
					{
						var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (r.Key);
						if (config == null)
							continue;
						var item = PlayerItemManager.Singleton.AddItem (r.Key, r.Value * i.Value.PeopleNum);
						if (dict.ContainsKey (item.Entry)) {				
							dict [item.Entry].Diff += item.Diff;
						} else {
							dict.Add (item.Entry, item);
						}
					}
				}
			}

			StringBuilder sb = new StringBuilder ();
			bool have = false;
			foreach (var i in dict) {
				if (i.Value.Diff <= 0)
					continue;
				var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i.Key);
				var str = string.Format (LanguageManager.Singleton ["REWARD_ITEM"], 
					          config.Name, i.Value.Diff);
				sb.Append (str);
				if (GameAppliaction.Singleton.Current is GameStates.CastleState)
					UI.UITipDrawer.Singleton.DrawNotify (str);
				have = true;
			}

			UIManager.Singleton.UpdateUIData<UI.Windows.UICastlePanel> ();
			if (have)
			   UI.UIControllor.Singleton.ShowMessage(string.Format(LanguageManager.Singleton["PEOPLE_WORK"], sb.ToString ()));
			return true;
		}

		public int TimeRewardTick{
			get{
				var value = this [PlayDataKeys.CalProuduceTime];
				if (value <= 0) {
					return GameAppliaction.Singleton.ConstValues.ProduceRewardTick;
				}

				return value;
			}
		}

		public void SetRewardTime(int time)
		{
			this [PlayDataKeys.CalProuduceTime] = time*1000;
		}

		public TimeSpan TimeToProduce {
			get {
				var timeTickForProduce = TimeSpan.FromMilliseconds (
					TimeRewardTick);
				var time = DateTime.UtcNow;
				var lastTime = TimeZero + TimeSpan.FromSeconds (this [PlayDataKeys.PRODUCE_TIME]);
				if (lastTime > time)
					return TimeSpan.FromSeconds (0);
				return timeTickForProduce - (time - lastTime);
			}
		}

		internal ProducePrisitData GetProduceStateByID (int id)
		{
			ProducePrisitData data;
			if (ProduceOpenState.TryGetValue (id, out data))
				return data;
			return null;
		}

		internal void CalPeopleOnProduce (int id, int num)
		{
			if (ProduceOpenState.ContainsKey (id)) {
				if (ProduceOpenState [id].PeopleNum >= num)
					ProduceOpenState [id].PeopleNum -= num;
			}
		}

		internal void AddPeopleOnProduce (int id, int num)
		{
			var people = this [PlayDataKeys.PEOPLE_COUNT];
			var inwork = BusyPeople;
			if (people <= inwork) {
				UI.UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["NO_PEOPLE_FREE"]);
				return;
			}

			if (ProduceOpenState.ContainsKey (id))
				ProduceOpenState [id].PeopleNum += num;
		}

		internal int GetProducePeople (int id)
		{
			var data = GetProduceStateByID (id);
			return data == null ? 0 : data.PeopleNum;
		}

		#endregion

		public void AddPlayerSkill(int skillid)
		{
			
		}

		#region 地图探索相关

		/// <summary>
		/// 是否通过地图/完成探索
		/// </summary>
		/// <param name="mapID"></param>
		/// <returns></returns>
		internal bool CompleteMap (List<int> mapID)
		{
			var maps = PlayerMapManager.Singleton.GetCompletedMaps ();
			foreach (var i in mapID) {
				if (!maps.Contains (i))
					return false;
			}
			return true;
		}

		/// <summary>
		/// 为-1 就是初始化
		/// </summary>
		public int CurrentMap {
			get {
				var map = this [PlayDataKeys.PLAYER_CURRENT_MAP];
				//初始化

				if (map == -1) {
					return App.GameAppliaction.Singleton.ConstValues.DefaultMapID;
				}
				return map;
			}
		}

		/// <summary>
		/// 进入地图
		/// </summary>
		/// <param name="map"></param>
		public void JoinMap (int map)
		{
			PlayerMapManager.Singleton.OpenMap (map);
			this [PlayDataKeys.PLAYER_CURRENT_MAP] = map;
			GoPos(null);
		}
			

		/// <summary>
		/// record the last pos 
		/// if input null goto orgin pos.
		/// else record
		/// </summary>
		/// <param name="target"></param>
		public void GoPos (Vector2? target)
		{
			//设置为0 
			if (target == null) {
				this [PlayDataKeys.PLAYER_CURREN_POS] = -1;
				return;
			}
			var x = (int)target.Value.x;
			var y = (int)target.Value.y;
			int index = PosXYToIndex (x, y);


			this [PlayDataKeys.PLAYER_CURREN_POS] = index;
		}

		public Vector2? CurrentPos {
			get {
				var index = this [PlayDataKeys.PLAYER_CURREN_POS];
				if (index == -1) {
					return null;
				} else {
					return IndexToPos (index);
				}
			}
		}

		public int LastFood{
			get { return Mathf.Max (this [PlayDataKeys.PLAYER_LAST_FOOD], 0); } 
			set{this [PlayDataKeys.PLAYER_LAST_FOOD] = value; }
		}



		public int PackageSize {
			get {
				var size = this [PlayDataKeys.PACKAGE_SIZE];
				if (size == -1) {
					this [PlayDataKeys.PACKAGE_SIZE] = App.GameAppliaction.Singleton.ConstValues.DefaultPackageSize;
					return App.GameAppliaction.Singleton.ConstValues.DefaultPackageSize;
				}
				return size;
			}
		}

		public void AddPackageSize(int size)
		{
			var current = this.PackageSize;
			this [PlayDataKeys.PACKAGE_SIZE] = current + size;
			DataManagers.AchievementManager.Singleton.PackageChanged (this.PackageSize);
		}

		//添加
		public bool AddFood (int num)
		{

			var foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;
			if (PlayerItemManager.Singleton.GetItemCount (foodEntry) < num)
				return false;
			if (PlayerItemManager.Singleton.AddItemIntoPack (foodEntry, num)) {
				PlayerItemManager.Singleton.SubItem (foodEntry, num);
				return true;
			}
			return false;
		}
		//减少
		public bool SubFood (int num)
		{
			int foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;

			if (PlayerItemManager.Singleton.CalItemFromPack (foodEntry, num)) {

				PlayerItemManager.Singleton.AddItem (foodEntry, num);
				return true;
			}
			return false;
		}

		/// <summary>
		/// 消耗食物
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public bool CostFood (int num)
		{
			int foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;
			var success = PlayerItemManager.Singleton.CalItemFromPack (foodEntry, num);
			return success;
		}


		public bool FullFood()
		{
			var maxTimes = 3;
			var current = Mathf.Max (0, this [PlayDataKeys.GiveFoodTimes]);
			if (current >= maxTimes)
				return false;
			this [PlayDataKeys.GiveFoodTimes] = current++;
			int foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;
			PlayerItemManager.Singleton.FullByItem (foodEntry);
			return true;
		}

		/// <summary>
		/// 当前可出战人数
		/// </summary>
		public int TeamSize {
			get {
				var size = this [PlayDataKeys.TEAM_SIZE];
				if (size < App.GameAppliaction.Singleton.ConstValues.DefaultTeamSize)
					this [PlayDataKeys.TEAM_SIZE] = App.GameAppliaction.Singleton.ConstValues.DefaultTeamSize;
				return this [PlayDataKeys.TEAM_SIZE];
			}
		}

		/// <summary>
		/// 设置出战人数
		/// </summary>
		/// <param name="size"></param>
		internal void SetTeamSize (int size)
		{
			this [PlayDataKeys.TEAM_SIZE] = size;
		}

		#endregion

		#region 成就

		/// <summary>
		/// 获得指定成就
		/// </summary>
		/// <param name="achievementID"></param>
		/// <returns></returns>
		internal bool HaveGetAchievement (List<int> achievementID)
		{
			return true;
		}

		public int AchievementPoint{ get { return  this [PlayDataKeys.PLAYER_ACHIEVEMENT_POINT] <= 0 ? 0 : this [PlayDataKeys.PLAYER_ACHIEVEMENT_POINT]; } }

		public int AddAchievementPoint (int point)
		{
			if (point <= 0)
				return AchievementPoint;
			
			var pointResult = AchievementPoint + point;
			this [PlayDataKeys.PLAYER_ACHIEVEMENT_POINT] = pointResult;
			return pointResult;
		}

		#endregion

		#region 金币

		/// <summary>
		/// 消耗金币
		/// </summary>
		/// <param name="cost"></param>
		/// <returns></returns>
		internal int SubGold (int cost)
		{
			if (cost < 0)
				return this.Gold;
			Gold = Gold - cost;
			AchievementManager.Singleton.CostGold (cost);
			return Gold;
		}

		/// <summary>
		/// 添加金币
		/// </summary>
		/// <param name="add"></param>
		/// <returns></returns>
		internal int AddGold (int add)
		{
			if (add < 0)
				return this.Gold;
			Gold = Gold + add;
			return Gold;
		}

		public int Gold {
			get {
				var v = this [PlayDataKeys.PLAYER_GOLD];
				if (v <= 0)
					return 0;
				return v;
			}

			private set {
				this [PlayDataKeys.PLAYER_GOLD] = value;
			}
		}

		#endregion

		#region Coin

		/// <summary>
		/// 消耗金币
		/// </summary>
		/// <param name="cost"></param>
		/// <returns></returns>
		internal int SubCoin (int cost)
		{
			if (cost < 0)
				return this.Coin;
			Coin = Coin - cost;
			AchievementManager.Singleton.CostCoin (cost);
			return Coin;
		}

		/// <summary>
		/// 添加金币
		/// </summary>
		/// <param name="add"></param>
		/// <returns></returns>
		internal int AddCoin (int add)
		{
			if (add < 0)
				return this.Coin;
			Coin = Coin + add;
			return Coin;
		}

		public int Coin {
			get {
				var v = this [PlayDataKeys.PLAYER_COIN];
				if (v <= 0)
					return 0;
				return v;
			}

			private set {
				this [PlayDataKeys.PLAYER_COIN] = value;
			}
		}

		#endregion


		public BattleControlMode ControlMode {
			get {
				if (this [PlayDataKeys.PLAYER_BATTLE_MODE] == -1) {
					this [PlayDataKeys.PLAYER_BATTLE_MODE] = (int)BattleControlMode.AUTO;
				}
				if (this [PlayDataKeys.PLAYER_BATTLE_MODE] == (int)BattleControlMode.AUTO)
					return BattleControlMode.AUTO;
				return BattleControlMode.PLAYER;
			}
		}

		public void SetControlMode (BattleControlMode mode)
		{
			this [PlayDataKeys.PLAYER_BATTLE_MODE] = (int)mode;
		}

		public bool IsMusicOn {
			get {
				var value = this [PlayDataKeys.MUSIC_OFF];
				if (value == -1) {
					this [PlayDataKeys.MUSIC_OFF] = 0;
				}
				return	this [PlayDataKeys.MUSIC_OFF] == 0;
			}
		}

		public void MusicState (int state)
		{
			this [PlayDataKeys.MUSIC_OFF] = state;

		}

		public bool EffectOn {
			get { 
				var value = this [PlayDataKeys.EFFECT_MUSIC];
				if (value == -1) {
					this [PlayDataKeys.EFFECT_MUSIC] = 0;
				}
				return this [PlayDataKeys.EFFECT_MUSIC] == 0;
			}
		}

		public void EffectMusicState (int state)
		{
			this [PlayDataKeys.EFFECT_MUSIC] = state;
			SoundManager.Singleton.SetSourceValue (EffectOn ? 1 : 0);
		}

		#region paement

		public List<PersistStructs.PaymentData> PaymentData {
			get {

				var json = Tools.Utility.ReadAStringFile (Tools.Utility.GetStreamingAssetByPath (PAYMENT_PATH));
				if (!string.IsNullOrEmpty (json)) {
					return JsonTool.Deserialize<List<PersistStructs.PaymentData>> (json);
				}

				return new List<PersistStructs.PaymentData> ();
			}
		}


		private bool DoPayment (PersistStructs.PaymentData paymentData)
		{
			this.AddCoin (paymentData.Reward);
			var log = string.Format (LanguageManager.Singleton ["PaymentSuccess"],
				          paymentData.Des, paymentData.Reward);
			UITipDrawer.Singleton.DrawNotify (log);
			UIControllor.Singleton.ShowMessage (log);
			UIManager.Singleton.TryToHide<UI.Windows.UIPayment> ();
			UIManager.Singleton.UpdateUIData ();
			return true;
		}


		public bool DoPaymentBuyKey(string id)
		{
			foreach (var i in PaymentData) {
				if (i.BundleID == id)
				{
					return	DoPayment (i);
					//break;
				}
			}
			return false;
		}
		#endregion

		public int GuideStep
		{
			set { this [PlayDataKeys.GuideStep] = value; }
			get { return this [PlayDataKeys.GuideStep]; } 
		}
	}


}
