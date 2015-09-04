using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.Controllors
{
    public class BattleControllor :GControllor
    {
        public BattleControllor(GPerception per):base(per)
        { }

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
                    }
                    return GAction.Empty;
                default:
                    return GAction.Empty;
            }

        }

        
    }
}
