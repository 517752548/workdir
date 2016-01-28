using Assets.Scripts.Tools;
using ExcelConfig;
using org.vxwo.csharp.json;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataManagers
{
    public class PlayerArmyManager : Tools.XSingleton<PlayerArmyManager>, IPresist
    {
        public const string PLAYER_ARMY_FILE = "_PLAYER_ARMY.json";
        public const string PLAYER_EMPLOY_SOLDIER = "_PLAYER_EMPLOY_ARMY.json";
        public const string GO_TO_EXPLORE_TEAM = "_PALYER_GO_TO_EXPLORE_TEAM.json";

        private Dictionary<int, bool> Soldiers = new Dictionary<int, bool>();
        private HashSet<int> _explore_team = new HashSet<int>();
        private HashSet<int> _employSoldier = new HashSet<int>();

        public void Presist()
        {
            var list = new List<PlayerSoldier>();
            foreach (var i in Soldiers)
            {
                list.Add(new PlayerSoldier { IsAlive = i.Value, SoldierID = i.Key });
            }
            Tools.PresistTool.SaveJson(list, PLAYER_ARMY_FILE);
            var employList = new List<int>();
            foreach (var i in _employSoldier)
            {
                employList.Add(i);
            }
            Tools.PresistTool.SaveJson(employList, PLAYER_EMPLOY_SOLDIER);
            Tools.PresistTool.SaveJson(_explore_team.Select(t => t).ToList(), GO_TO_EXPLORE_TEAM);
        }

        public void Load()
        {

            Soldiers.Clear();
            var list = Tools.PresistTool.LoadJson<List<PlayerSoldier>>(PLAYER_ARMY_FILE);

            foreach (var i in list)
            {
                Soldiers.Add(i.SoldierID, i.IsAlive);
            }


            _employSoldier.Clear();
            var employList = Tools.PresistTool.LoadJson<List<int>>(PLAYER_EMPLOY_SOLDIER);

            foreach (var i in employList)
            {
                _employSoldier.Add(i);
            }

            _explore_team.Clear();
            var exploreList = Tools.PresistTool.LoadJson<List<int>>(GO_TO_EXPLORE_TEAM);

            foreach (var i in exploreList)
            {
                _explore_team.Add(i);
            }
        }
       

        public void Reset()
        {
            Soldiers.Clear();
            _employSoldier.Clear();
            _explore_team.Clear();
            Presist();

            // throw new NotImplementedException();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public List<int> GetTeam()
        {
            return _explore_team.Select(t => t).ToList();
        }

        public void SetTeam(List<int> team)
        {
            _explore_team.Clear();
            foreach (var i in team)
            {
                _explore_team.Add(i);
            }
        }

		public void RemoveSoldierInTeam(int soldier)
		{
			_explore_team.Remove (soldier);
		}

        public bool HaveEmployHero(HeroConfig hero)
        {
            return _employSoldier.Contains(hero.ID);
        }

        public List<PlayerSoldier> GetAllSoldier()
        {
            var list = new List<PlayerSoldier>();
            foreach (var i in Soldiers)
            {
                list.Add(new PlayerSoldier { IsAlive = i.Value, SoldierID = i.Key });
            }
            return list;
        }

        public bool BuyHero(HeroConfig hero)
        {
            if (!CanEmployHero(hero)) return false;
            var price = hero.recruit_price;

            switch ((EmployCostCurrent)hero.recruit_current_type)
            {
                case EmployCostCurrent.Coin:
                    if (GamePlayerManager.Singleton.Coin >= price)
                    {
                        GamePlayerManager.Singleton.SubCoin(price);
                    }
                    else
                    {
                        UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["BUY_HERO_NOT_ENOUGH_COIN"]);
                        return false;
                    }
                    break;
                case EmployCostCurrent.Gold:
                    if (GamePlayerManager.Singleton.Gold >= price)
                    {
                        GamePlayerManager.Singleton.SubGold(price);
                    }
                    else
                    {
                        UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["BUY_HERO_NOT_ENOUGH_GOLD"]);
                        return false;
                    }
                    break;
            }
           
            DoAdd(hero);
            return true;
        }


        public void DoAdd(HeroConfig hero)
        {
             //已经召唤了
            if (this._employSoldier.Contains(hero.ID)) return;

            this.Soldiers.Add(hero.recruit_id, true);
            _employSoldier.Add(hero.ID);
            //获得英雄
            var monster = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(hero.recruit_id);
            UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["BuyHeroSuccess"], monster.Name));
        }

        public bool CanEmployHero(HeroConfig hero)
        {
            if (this.Soldiers.ContainsKey(hero.recruit_id)) return false;
            switch ((EmployCondtionType)hero.recruit_condition)
            {
                case EmployCondtionType.CompleteMap:
                    //探索完成指定地图
                    var mapID = Tools.UtilityTool.ConvertToInt(hero.recruit_para);
				if (!GamePlayerManager.Singleton.CompleteMap(new List<int>{mapID}))
                    {
                        return false;
                    }
                    break;
                case EmployCondtionType.GetAchievement:
                    if (!GamePlayerManager.Singleton.HaveGetAchievement(UtilityTool.SplitIDS(hero.recruit_para)))
                        return false;
                    break;
                case EmployCondtionType.GetItem:
				    var keyValues = UtilityTool.SplitIDS(hero.recruit_para);
                    foreach (var i in keyValues)
                    {
					  if (!(PlayerItemManager.Singleton.GetItemCount(i)>0)) return false;
                    }
                    return true;
                case EmployCondtionType.None:
                    break;
            }
            return true;
        }

		public string GetEmployHeroRequire(HeroConfig hero)
		{
			switch ((EmployCondtionType)hero.recruit_condition)
			{
			case EmployCondtionType.CompleteMap:
				//探索完成指定地图
				var mapID = Tools.UtilityTool.ConvertToInt (hero.recruit_para);
				var mapConfig = ExcelToJSONConfigManager.Current.GetConfigByID<MapConfig> (mapID);
				return string.Format (LanguageManager.Singleton ["NEED_COMPLETE_MAP"], mapConfig.Name);
				//break;
			case EmployCondtionType.GetAchievement:
				var aches = UtilityTool.SplitIDS (hero.recruit_para);
				var sb = new StringBuilder ();
				foreach (var i in aches) {
					var a = ExcelToJSONConfigManager.Current.GetConfigByID<AchievementConfig> (i);
					if (a == null)
						continue;
					sb.Append (a.Name);
					sb.Append (" ");
				}

				return string.Format (LanguageManager.Singleton ["NEED_GET_ACH"], sb.ToString ());
				//break;
			case EmployCondtionType.GetItem:
				var keyValues = UtilityTool.SplitIDS (hero.recruit_para);
				var sbItem = new StringBuilder ();
				foreach (var i in keyValues)
				{
					var itemConfig = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (i);
					if (itemConfig == null)
						continue;
					sbItem.Append (itemConfig.Name);
					sbItem.Append (" ");

				}
				return string.Format (LanguageManager.Singleton ["NEED_GET_ITEM"], sbItem.ToString ());
			case EmployCondtionType.None:
				break;
			}
			return  LanguageManager.Singleton["CAN_EMPLOY"];
		}

        /// <summary>
        ///  设置图标
        /// </summary>
        /// <param name="formjob"></param>
        /// <param name="monster"></param>
        internal void SetJob(UISprite formjob, MonsterConfig monster)
        {
            var monsterCategory = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterCategoryConfig>(monster.Type);
            formjob.spriteName = monsterCategory.IconName;
        }


        public void SetIcon(UITexture icon, MonsterConfig monster,TypeOfIcon  type = TypeOfIcon.List, bool makePer = true)
        {
            if (icon == null) { Debug.LogError("Icon is null"); return; }
            switch(type)
            {
                case TypeOfIcon.List:
                    icon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("HeroIcon/list/" + monster.ResName);
                    break;
                case TypeOfIcon.LvlUp:
                    icon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("HeroIcon/lvlup/" + monster.ResName);
                    break;
                case TypeOfIcon.MainUI:
                    icon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("HeroIcon/main/" + monster.ResName);
                    break;
                case TypeOfIcon.BattleMax:
                    icon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("HeroIcon/battlemax/" + monster.ResName);
                    break;
                case TypeOfIcon.BattleMin:
                    icon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("HeroIcon/battlemin/" + monster.ResName);
                    break;
            }
            if(makePer)
            icon.MakePixelPerfect();
            
        }
        internal bool LevelUp(PlayerSoldier Soldier)
        {
            if (!Soldiers.ContainsKey(Soldier.SoldierID)) return false;

            var levelup = ExcelConfig.ExcelToJSONConfigManager.Current.FirstConfig<ExcelConfig.MonsterLvlUpConfig>((t => t.OldMonster == Soldier.SoldierID));
            if (levelup == null)
            {
                UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["MAX_LEVEL_SOLDIER"]);
                return false;
            }

            if (levelup.CostGold > 0)
            {
                if (GamePlayerManager.Singleton.Gold < levelup.CostGold)
                {
                    UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["LVLUP_NOT_ENOUGH_GOLD"]);
                    return false;
                }
            }

            var items = Tools.UtilityTool.SplitKeyValues(levelup.CostItems,levelup.CostItemsNumber);
            foreach (var i in items)
            {
                if (PlayerItemManager.Singleton.GetItemCount(i.Key) < i.Value)
                {
                    var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);

                    UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["LVLUP_NOT_ENOUGH_ITEM"], item.Name));
                    return false;
                }
            }

            //扣减金币
            if (levelup.CostGold > 0)
            {
                GamePlayerManager.Singleton.SubCoin(levelup.CostGold);
            }
            //扣减道具
            foreach (var i in items)
            {
                PlayerItemManager.Singleton.SubItem(i.Key, i.Value);
            }

            var monsterOld = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(levelup.OldMonster);
            var monsterLate = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(levelup.LateMonster);

			if (Soldiers.ContainsKey (monsterLate.ID)) {
			
				UI.UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["ID_EXISTS_ERROR"]);
				return false;
			}
				
            Soldiers.Remove(Soldier.SoldierID);

            Soldiers.Add(monsterLate.ID, Soldier.IsAlive);

            UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["LVLUP_SUCCESS"], monsterOld.Name, monsterLate.Name));
            return true;

        }

        /// <summary>
        /// 死亡士兵
        /// </summary>
        /// <param name="soldier"></param>
        public void Dead(int soldier)
        {
            if (Soldiers.ContainsKey(soldier))
                Soldiers[soldier] = false;
        }

		public List<int> DeadAllSoldiersInTeam()
		{
			foreach (var i in _explore_team) {
				Dead (i);
			}
			return _explore_team.ToList ();
			//PlayerItemManager.Singleton.EmptyPackage ();
		}

        /// <summary>
        ///  复活士兵
        /// </summary>
        /// <param name="soldier"></param>
        /// <returns></returns>
        internal bool Relive(int soldier)
        {
            bool isAlive = false;
            if (Soldiers.TryGetValue(soldier, out isAlive))
            {
                if (isAlive) return false;
                var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(App.GameAppliaction.Singleton.ConstValues.ReliveNeedItem);
                if (config == null) return false;
                
                if (PlayerItemManager.Singleton.GetItemCount(config.ID) < 1)
                {
                    UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_GOEXPLORE_Relive_NOT_ENOUGH_ITEM"], config.Name);
                    return false;
                }
                PlayerItemManager.Singleton.SubItem(config.ID, 1);
                Soldiers[soldier] = true;
                var monster = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(soldier);
                UI.UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_GOEXPLORE_Relive_Success"], config.Name, monster.Name);
                return true;
            }
            else
            {
                return false;
            }
        }

		public bool IsAlive(int id)
		{
			bool isAlive = false;
			if (Soldiers.TryGetValue (id, out isAlive)) {
				return isAlive;
			}
			return true;
		}

        /// <summary>
        /// 是否在队伍中
        /// </summary>
        /// <param name="soldierID"></param>
        /// <returns></returns>
        internal bool IsTeam(int soldierID)
        {
            return _explore_team.Contains(soldierID);
        }

    }


    public class PlayerSoldier
    {
        [JsonName("ID")]
        public int SoldierID { set; get; }
        [JsonName("AL")]
        public bool IsAlive { set; get; }
    }

    public enum TypeOfIcon
    {
        MainUI, List, LvlUp,BattleMax, BattleMin
    }
}
