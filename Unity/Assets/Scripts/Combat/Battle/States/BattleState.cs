using Assets.Scripts.Combat.Simulate;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.States
{
    public class BattleState :GState
    {
        public BattleState(int battleGroupID)
        {
            var perception = new BattlePerception(this);
            this.Perception = perception;

            var battle = new Elements.BattleEl(new Controllors.BattleControllor(this.Perception), battleGroupID);
            AddElement(battle);
        }
        public override void OnEnter()
        {
            //throw new NotImplementedException();
        }
        public override void OnExit()
        {
            //throw new NotImplementedException();
        }
    }
}
