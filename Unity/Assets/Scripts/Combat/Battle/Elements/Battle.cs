using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.Elements
{
    public class Battle :GObject
    {

        public Battle(Controllors.BattleControllor battleControllor) : base(battleControllor) { }
        public Battle(Controllors.BattleControllor battleControllor, int battleGroupID):this(battleControllor)
        {
            // TODO: Complete member initialization
            var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.BattleGroupConfig>(battleGroupID);
            var battleIDs = Tools.UtilityTool.SplitIDS( config.BattleIds);//.Split("|");
            var listBattle = ExcelConfig.ExcelToJSONConfigManager.Current
                .GetConfigs<ExcelConfig.BattleConfig>((t) => {
                    return battleIDs.Contains(t.ID);
                });
            BattleGroup = config;
            Battles = listBattle;
        }

        public ExcelConfig.BattleGroupConfig BattleGroup { private set; get; }
        public ExcelConfig.BattleConfig[] Battles { private set; get; }

        public int CurrentIndex = 0;
    }
}
