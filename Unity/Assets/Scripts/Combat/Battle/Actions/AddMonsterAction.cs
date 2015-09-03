using Assets.Scripts.Combat.Simulate;
using ExcelConfig;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.Actions
{
    class AddMonsterAction :GAction
    {

        public AddMonsterAction(GObject el, GPerception per,BattleConfig battleID):base(el,per)
        {
            BattleID = battleID;
        }
        public override void DoAction()
        {
            var controllor = new Controllors.ArmyControllor(this.Perception);
            var battleConfig = BattleID;
            var monster = new Army();
            var battle = this.Obj as Elements.BattleEl;

            monster.Camp = ArmyCamp.Monster;
            monster.Soldiers.Add(new Soldier { ConfigID = battleConfig.NpcID, Num = 1 });

            var monsterArmy = new Elements.BattleArmy(controllor, monster);
            Perception.State.AddElement(monsterArmy);
            battle.BattleIndex++;
            if (!string.IsNullOrEmpty(battleConfig.Dialog))
            {
                UI.UIControllor.Singleton.ShowBattleDialog(battleConfig);
                //showDialog 
                battle.State = Elements.BattleStateType.WaitDialog;
            }
            else {
                battle.State = Elements.BattleStateType.Battling;
            }

            GameDebug.Log("ADD Monster:"+ battleConfig.Name);
        }

        public BattleConfig BattleID { get; set; }
    }
}
