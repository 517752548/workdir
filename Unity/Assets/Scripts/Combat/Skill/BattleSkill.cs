using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;
using Assets.Scripts.Combat.Battle.Elements;

namespace Assets.Scripts.Combat.Skill
{
    public class BattleSkill
    {
        public SkillConfig Config { set; get; }

        public BattleSoldier Soldier { set; get; }
        public BattleSkill (BattleSoldier solider, int configID)
        {
            Config = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(configID);
            Soldier = solider;
        }
    }
}
