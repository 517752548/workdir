using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Controllors
{
    public class ArmyPlayerControllor :GControllor
    {
        public ArmyPlayerControllor(GPerception per) : base(per) { }
        public override GAction GetAction(GObject current)
        {
            var army = current as Elements.BattleArmy;
            var per = Perception as States.BattlePerception;
            var state = per.State as States.BattleState;
            int lastIndex =state.Render.GetTapIndex();
            if (lastIndex < 0) return GAction.Empty;
            var battle = per.GetBattle();
            if (battle.State == Elements.BattleStateType.End) return GAction.Empty;

            for (var i = 0; i < army.Soldiers.Count; i++)
            {
                if (lastIndex != i) continue; 
                var s = army.Soldiers[i];
                if (s.AttackCdTime + (s.SkillConfig.SkillCd / 1000f) < Time.time)
                {

                    var enemy = per.GetEnemy(army);
                    if (enemy == null) continue;
                    state.Render.ReleaseTapIndex();
                    return new Actions.AttackAction(current, Perception, s, enemy);
                }
            }
            return GAction.Empty;
        }
    }
}
