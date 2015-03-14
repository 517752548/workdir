using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Actions
{
    public class EndBattleAction :GAction
    {
        public EndBattleAction(GObject obj, GPerception per)
            :base(obj,per)
        { }
        public override void DoAction()
        {

            Appliaction.GameAppliaction.Singleton.GoToExplore(-1);
            //ShowBattle result 
            UI.UITipManager.Singleton.DrawNotify(string.Format("BattleEnd"));
            //throw new NotImplementedException();
            
        }
    }
}
