using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Actions
{
    public class EndBattleAction :GAction
    {
        public EndBattleAction(GObject obj, GPerception per)
            :base(obj,per)
        { }

        public Proto.ArmyCamp Winner { set; get; }

		public bool PlayerDead{ set; get; }
        public override void DoAction()
		{
			//GOTo Explore
			//App.GameAppliaction.Singleton.GoToExplore(-1); 
			//ShowBattle result 
            
			var per = Perception as States.BattlePerception;
			//UI.UITipDrawer.Singleton.DrawNotify(string.Format("BattleEnd"));
			//throw new NotImplementedException();
			var battle = this.Obj as Elements.BattleEl;
			battle.State = Elements.BattleStateType.End;
			var state = per.State as States.BattleState;
			var winner = Winner;


			state.End (new States.BattleResult { 
				Winner = winner, 
				DropList = battle.DropList,
				DropGold = battle.DropGold,
				Dead = PlayerDead
			});
		}
    }
}
