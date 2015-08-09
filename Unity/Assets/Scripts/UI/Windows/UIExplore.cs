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

        public override void InitModel()
        {
            base.InitModel();
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