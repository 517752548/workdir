using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataManagers
{
    public class GMCommTool : Tools.XSingleton<GMCommTool>
    {
        public void ExecuteGMComm(string comm)
        {
            var args = comm.Split(' ');
            if (args.Length == 0) return;
            switch (args[0])
            {
                case "makeitem"://创建道具
                    PlayerItemManager.Singleton.AddItem(int.Parse(args[1]), int.Parse(args[2]));
                    break;
                case "makeallitem"://创建所有道具
                    var makeCount = int.Parse(args[1]);
                    var itemconfigs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.ItemConfig>();
                    foreach (var i in itemconfigs)
                    {
                        PlayerItemManager.Singleton.AddItem(i.ID, makeCount);
                    }
                    break;
                case "openui"://打开一个UI
                    var ui = args[1];
                    var typeUI = typeof(UIManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(UIWindow))).FirstOrDefault(t => t.Name == ui);
                    var type = typeof(UIManager);
                    var method = type.GetMethod("CreateOrGetWindow");
                    var w = method.MakeGenericMethod(typeUI).Invoke(UIManager.Singleton, null) as UIWindow;
                    w.ShowWindow();
                    break;
                case "iammaster"://管理员账号
                    int masterNum = 100000;
                    var mitemconfigs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.ItemConfig>();
                    foreach (var i in mitemconfigs)
                    {
                        PlayerItemManager.Singleton.AddItem(i.ID, masterNum);
                    }
                    GamePlayerManager.Singleton.AddGold(masterNum);
                    GamePlayerManager.Singleton.AddCoin(masterNum);
                    break;
                case "reset"://重置数据
                    App.GameAppliaction.Singleton.ResetPlayData();
                    break;
                case "addgold"://添加金币
                    int gold = int.Parse(args[1]);
                    GamePlayerManager.Singleton.AddGold(gold);
                    break;
                case "addcoin"://添加钻石
                    int coin = int.Parse(args[1]);
                    GamePlayerManager.Singleton.AddGold(coin);
                    break;
                case "deadhero"://让英雄死亡
                    if (args.Length >= 2)
                    {
                        DataManagers.PlayerArmyManager.Singleton.Dead(int.Parse(args[1]));
                    }
                    else
                    {
                        foreach (var i in DataManagers.PlayerArmyManager.Singleton.GetAllSoldier())
                        {
                            DataManagers.PlayerArmyManager.Singleton.Dead(i.SoldierID);
                        }
                    }
                    break;
                case "buyhero"://购买英雄不不管条件是否满足
                    var heroconfig = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.HeroConfig>( int.Parse(args[1]));
                    DataManagers.PlayerArmyManager.Singleton.DoAdd(heroconfig);
                    break;
			case "clearmap":
				DataManagers.PlayerMapManager.Singleton.Reset ();

				break;
                default: return;

            }

            UI.UIManager.Singleton.UpdateUIData();
            Debug.Log(comm);
        }
    }
}