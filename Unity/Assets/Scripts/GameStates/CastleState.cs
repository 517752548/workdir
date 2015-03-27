using Assets.Scripts.UI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameStates
{
    class CastleState:Appliaction.GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var ui = UI.UIManager.Singleton.CreateOrShowWindow<UI.Windows.UICastlePanel>();
            ui.ShowWindow();
        }

        public override void OnExit()
        {
            base.OnExit();
            UICastlePanel ui = UI.UIManager.Singleton.GetUIWindow<UICastlePanel>();
            if (ui != null)
                ui.HideWindow();
        }
    }
}
