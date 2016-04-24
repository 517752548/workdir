using Assets.Scripts.UI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameStates
{
    class CastleState:App.GameState
    {
        public override void OnEnter()
		{
			base.OnEnter ();
			UI.UIControllor.Singleton.HideAllUI ();
			UI.UIControllor.Singleton.ShowOrHideMessage (true);

			var ui = UI.UIManager.Singleton.CreateOrGetWindow<UI.Windows.UICastlePanel> ();
			ui.ShowWindow ();

			if (DataManagers.GuideManager.Singleton.CurrentStep == Assets.Scripts.DataManagers.GuideStep.Completed)
			{
				DataManagers.RandomEventManager.Singleton.TryToRandom ();
			}
		}

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
