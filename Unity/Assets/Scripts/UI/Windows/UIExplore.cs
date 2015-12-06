using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.GameStates;

namespace Assets.Scripts.UI.Windows
{
    partial class UIExplore
    {
        public UIExplore()
        {
            NoCollider = true;
        }
        public override void InitModel()
        {
            base.InitModel();

            bt_close.OnMouseClick((s, e) => {
                var state = App.GameAppliaction.Singleton.Current as ExploreState;
                if (state == null) return;
                UIMessageBox.ShowMessage(LanguageManager.Singleton["UIEXPLORE_EXIT_TITLE"],
                    LanguageManager.Singleton["UIEXPLORE_EXIT_MESSGAE"],
                    () => {
                        state.JoinCastle(true);
                    }, null);

            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            var state = App.GameAppliaction.Singleton.Current as ExploreState;
            if (state == null) return;
            this.lb_title.text =state.Config.Name;
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}