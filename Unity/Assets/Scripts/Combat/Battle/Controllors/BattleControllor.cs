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
            if(per.HaveDeadArmy())
            {
                return new Actions.EndBattleAction(current, Perception);
            }
            return GAction.Empty;
        }

        
    }
}
