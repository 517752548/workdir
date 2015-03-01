using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Controllors
{
    class ArmyControllor: GControllor
    {
        public ArmyControllor(GPerception per):base(per)
        { }

        public override GAction GetAction(GObject current)
        {
            var army = current as Elements.BattleArmy;
            var per = Perception as States.BattlePerception;
            foreach(var i in army.Soldiers)
            {
                 if(i.AttackCdTime+(i.Config.AttackSpeed/1000f) <Time.time)
                 {
                     var enemy = per.GetEnemy(army);
                     if (enemy == null) continue;
                     return new Actions.AttackAction(current, Perception, i,enemy);
                 }
            }
            return GAction.Empty;
        }
    }
}
