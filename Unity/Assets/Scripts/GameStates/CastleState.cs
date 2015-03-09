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
    }
}
