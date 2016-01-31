using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.GameStates;
using UnityEngine;
using Assets.Scripts.DataManagers;

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

            bt_close.OnMouseClick((s, e) => 
				{
                var state = App.GameAppliaction.Singleton.Current as ExploreState;
                if (state == null) return;
                UIMessageBox.ShowMessage(LanguageManager.Singleton["UIEXPLORE_EXIT_TITLE"],
                    LanguageManager.Singleton["UIEXPLORE_EXIT_MESSGAE"],
					LanguageManager.Singleton["UIEXPLORE_GOHOME"],
					LanguageManager.Singleton["UIEXPLORE_STILL_HRER"],
                    () => {
                        state.JoinCastle(true);
                    }, null);

            });

            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
			OnUpdateUIData ();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

		public override void OnUpdateUIData ()
		{
			base.OnUpdateUIData ();
			var state = App.GameAppliaction.Singleton.Current as ExploreState;
			if (state == null) return;
			this.lb_title.text =state.Config.Name;
			int foodNum = PlayerItemManager.Singleton.GetFoodNum ();// GamePlayerManager.Singleton.FoodCount;
			int packageCur = foodNum;
			int packageSize = GamePlayerManager.Singleton.PackageSize;
			var totalExplore = DataManagers.PlayerMapManager.Singleton.GetMapTotalExploreValue (state.Config.ID);
			var currentExplore = DataManagers.PlayerMapManager.Singleton.GetMapExploreValue (state.Config.ID);

			lb_explorevalue.text = string.Format (LanguageManager.Singleton ["UI_EXPLORE_EXPLOREVALUE"],
				100f*((float)currentExplore / Mathf.Max (1, totalExplore)));

			lb_food.text = string.Format (LanguageManager.Singleton ["UI_EXPLORE_FOOD"], foodNum);
			lb_package.text = string.Format ("{0}/{1}", packageCur, packageSize);
			
		}
    }
}