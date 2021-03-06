﻿using Assets.Scripts.Combat.Simulate;
using ExcelConfig;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

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
            var state = this.Perception.State as States.BattleState;
            monster.Camp = ArmyCamp.Monster;
            monster.Soldiers.Add(new Soldier { ConfigID = battleConfig.NpcID, Num = 1 });
            state.Render.ShowBattleName(battleConfig);
            var monsterArmy = new Elements.BattleArmy(controllor, monster);
            Perception.State.AddElement(monsterArmy);
            battle.BattleIndex++;
			if (!string.IsNullOrEmpty(battleConfig.Name))
            {
                state.Render.ShowDialog(battleConfig);
                //UI.UIControllor.Singleton.ShowBattleDialog(battleConfig);
            }
            

            if(App.GameAppliaction.BattleDebug)
            {
                GameDebug.Log("ADD Monster:" + battleConfig.Name);
                var mons = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(battleConfig.NpcID);
                var skill = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(mons.SkillID);
                GameDebug.Log("Monster:" + mons.ToDebugString());
                GameDebug.Log("MonsterSkill:" + skill.ToDebugString());
            }

            state.Render.ShowMonster(monsterArmy);
        }

        public BattleConfig BattleID { get; set; }
    }
}
