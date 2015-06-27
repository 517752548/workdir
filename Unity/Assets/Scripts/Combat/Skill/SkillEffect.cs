using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Combat.Battle.Elements;

namespace Assets.Scripts.Combat.Skill
{
    public class SkillEffect
    {
        public SkillEffect(BattleSoldier soldierSources, BattleSoldier soldierTarget)
        {
            Sources = soldierSources;
            Target = soldierTarget;
        }

        public BattleSoldier Sources { private set; get; }
        public BattleSoldier Target { private set; get; }
    }
}
