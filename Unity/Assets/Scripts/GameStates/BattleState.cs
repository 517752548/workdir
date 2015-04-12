using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameStates
{
    public class BattleState:Appliaction.GameState
    {
        private Proto.Army player;
        private Proto.Army monster;

        public BattleState(Proto.Army player, Proto.Army monster)
        {
            // TODO: Complete member initialization
            this.player = player;
            this.monster = monster;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            State = new Combat.Battle.States.BattleState(player, monster);
            UI.UIControllor.Singleton.ShowOrHideUIBackground(false);
            UI.UIControllor.Singleton.HideAllUI();
        }

        public override void OnTick()
        {
            base.OnTick();
            State.OnTick();
        }

        public override void OnExit()
        {
            base.OnExit();
            State.OnExit();
        }

        public Combat.Battle.States.BattleState State { private set; get; }
    }
}
