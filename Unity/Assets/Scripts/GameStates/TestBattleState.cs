using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Combat.Battle.Elements;
using Assets.Scripts.Combat.Battle.States;
using ExcelConfig;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Windows;

namespace Assets.Scripts.GameStates
{
    public class TestBattleState : App.GameState
    {
        public void StartBattle(int battleID, int index = 0)
        {
            if (PlayerSoliders == null) return;
            var ui = UIManager.Singleton.GetUIWindow<UIBattle>();
            if(ui==null)
            {
                ui = UIBattle.Show();
            }
            var battleGroup = ExcelToJSONConfigManager.Current.GetConfigByID<BattleGroupConfig>(battleID);
            if (BState != null)
            {
                BState.OnExit();
            }
            BState = new BattleState(battleID, ui, index, PlayerSoliders.ToList(), BattleCallBack);
            BState.Start();
        }

        public void BattleCallBack(BattleResult result)
        {
            if (result.Winner == Proto.ArmyCamp.Player)
            {
                   //doto
            }
        }

        public int[] PlayerSoliders { set; get; }

        private BattleState BState;

        public override void OnTick()
        {
            base.OnTick();
            if (BState != null)
            {
                BState.OnTick();
                if (BState == null) return;
                if (BState.NeedEnd)
                {
                    BState.OnExit();
                    BState = null;
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            UIControllor.Singleton.HideAllUI();
        }
    }
}
