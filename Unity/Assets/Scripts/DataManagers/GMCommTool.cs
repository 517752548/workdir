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

            }

            UI.UIManager.Singleton.OnUpdateUIData();
            Debug.Log(comm);
        }
    }
}
