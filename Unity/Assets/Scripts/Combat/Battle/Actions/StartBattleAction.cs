using Assets.Scripts.Combat.Simulate;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.Actions
{
    public class StartBattleAction :GAction
    {


        public StartBattleAction(GObject current, States.BattlePerception per):base(current,per)
        {
            // TODO: Complete member initialization
        }
        public override void DoAction()
        {
            var controllor = new Controllors.ArmyControllor(this.Perception);
            var player = new Army();
            player.Camp = ArmyCamp.Player;
            player.Soldiers = new List<Soldier>();
            player.Soldiers.Add(new Soldier { ConfigID = 1, Num = 10 });
            player.Soldiers.Add(new Soldier { ConfigID = 1, Num = 10 });
            var playerArmy = new Elements.BattleArmy(controllor, player);
            Perception.State.AddElement(playerArmy);
            var battle = this.Obj as Battle.Elements.BattleEl;
            battle.State = Elements.BattleStateType.Battling;
        }
    }
}
