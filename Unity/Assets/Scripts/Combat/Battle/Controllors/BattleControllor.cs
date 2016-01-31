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
		public BattleControllor (GPerception per) : base (per)
		{
		}


		//private bool IsPause = false;

		public override GAction GetAction (GObject current)
		{

			var battle = current as Elements.BattleEl;
			var per = Perception as States.BattlePerception;
			battle.TickEffect ();

			var state = per.State as States.BattleState;
			if (state.Render.Cancel) {
				return new Actions.EndBattleAction (current, per) { Winner = Proto.ArmyCamp.Monster };
			}
			switch (battle.State) {
			case Elements.BattleStateType.NOStart:
				return new Actions.StartBattleAction (current, per);
			case Elements.BattleStateType.Battling:
				if (per.HaveDeadArmy ()) {
					if (!per.PlayerDead ()) 
					{
						battle.ProcessDrop ();
						if (battle.BattleIndex < battle.Battles.Length) {
							if(battle.BattleIndex>0)
							  state.WaitForSeconds (3);
							
							battle.State = Assets.Scripts.Combat.Battle.Elements.BattleStateType.AddMonster;
							return GAction.Empty;
						}
					} 
					battle.State = Elements.BattleStateType.End;
					TimeToEnd = Time.time + 1.5f;
				}
				return GAction.Empty;
			case Elements.BattleStateType.AddMonster:
				battle.State = Assets.Scripts.Combat.Battle.Elements.BattleStateType.Battling;
				return new Actions.AddMonsterAction (current, 
					per,
					battle.Battles [battle.BattleIndex]
				);
				//break;
			case Elements.BattleStateType.End:
				if (TimeToEnd > Time.time)
					return GAction.Empty;
				if (per.PlayerDead ()) {   //玩家死亡
					return new Actions.EndBattleAction (current, Perception) { 
						Winner = Proto.ArmyCamp.Monster ,
						PlayerDead = true};
				} else {

					return new Actions.EndBattleAction (current, per) { Winner = Proto.ArmyCamp.Player ,
						PlayerDead = false};
				}
                    
			default:
				return GAction.Empty;
			}

		}



		public float TimeToEnd { get; set; }
	}
}
