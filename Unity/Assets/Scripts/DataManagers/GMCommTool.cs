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
            switch(args[0])
            {
                case "makeitem":
                    PlayerItemManager.Singleton.AddItem(int.Parse(args[1]), int.Parse(args[2]));
                    break;
                case "makeallitem":
                    var makeCount = int.Parse(args[1]);
                    var itemconfigs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.ItemConfig>();
                    foreach(var i in itemconfigs)
                    {
                        PlayerItemManager.Singleton.AddItem(i.ID, makeCount);
                    }
                    break;
                case "openui":
                    var ui = args[1];
                    var typeUI = typeof(UIManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(UIWindow))).FirstOrDefault(t => t.Name == ui);
                    var type = typeof(UIManager);
                    var method = type.GetMethod("CreateOrGetWindow");
                    var w= method.MakeGenericMethod(typeUI).Invoke(UIManager.Singleton,null) as UIWindow;
                    w.ShowWindow();
                    break;
                case "iammaster":
                    int masterNum = 100000;
                    var mitemconfigs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.ItemConfig>();
                    foreach (var i in mitemconfigs)
                    {
                        PlayerItemManager.Singleton.AddItem(i.ID, masterNum);
                    }
                    GamePlayerManager.Singleton.AddGold(masterNum);
                    GamePlayerManager.Singleton.AddCoin(masterNum);
                    break;
                case "reset":
                    App.GameAppliaction.Singleton.ResetPlayData();
                    
                    break;

            }

            UI.UIManager.Singleton.UpdateUIData();
            Debug.Log(comm); 
        }

       
    }
}
