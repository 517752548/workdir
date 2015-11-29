using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Controllors
{
    public class BattleControllor :GControllor
    {
        public BattleControllor(GPerception per):base(per)
        { }

        private float timeToEnd = 0f;
        public override GAction GetAction(GObject current)
        {

            var battle = current as Elements.BattleEl;
            var per = Perception as States.BattlePerception;
            battle.TickEffect();

            switch (battle.State)
            {
                case Elements.BattleStateType.NOStart:
                    return new Actions.StartBattleAction(current, per);
                case Elements.BattleStateType.Battling:
                    if (per.HaveDeadArmy())
                    {
                        battle.State = Elements.BattleStateType.End;
                        timeToEnd = Time.time + 1f;
                    }
                    return GAction.Empty;
                case Elements.BattleStateType.End:
                    if (timeToEnd > Time.time) return GAction.Empty;
                    if (per.PlayerDead())
                    {   //玩家死亡
                        return new Actions.EndBattleAction(current, Perception);
                    }
                    else
                    {
                        if (battle.BattleIndex >= battle.Battles.Length)
                        {
                            return new Actions.EndBattleAction(current, per);
                        }
                        else
                        {
                            //创建
                            return new Actions.AddMonsterAction(current, per, battle.Battles[battle.BattleIndex]);
                        }
                    }

                default:
                    return GAction.Empty;
            }

        }

        
    }
}
