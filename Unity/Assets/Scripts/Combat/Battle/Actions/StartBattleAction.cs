using Assets.Scripts.Combat.Battle.States;
using Assets.Scripts.Combat.Simulate;
using Assets.Scripts.Tools;
using ExcelConfig;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.Actions
{
    public class StartBattleAction : GAction
    {


        public StartBattleAction(GObject current, States.BattlePerception per)
            : base(current, per)
        {
            // TODO: Complete member initialization
        }
        public override void DoAction()
        {
            var mode = DataManagers.GamePlayerManager.Singleton.ControlMode;
            var state = this.Perception.State as BattleState;

            GControllor controllor;
            if (mode == DataManagers.BattleControlMode.AUTO)
                controllor = new Controllors.ArmyControllor(this.Perception);
            else
                controllor = new Controllors.ArmyPlayerControllor(Perception);

            var player = new Army();
            player.Camp = ArmyCamp.Player;
            player.Soldiers = new List<Soldier>();
            var soldier = state.PlayerSoldiers;
            foreach (var i in soldier)
                player.Soldiers.Add(new Soldier { ConfigID = i, Num = 1 });

            var playerArmy = new Elements.BattleArmy(controllor, player);
            Perception.State.AddElement(playerArmy);
            var battle = this.Obj as Battle.Elements.BattleEl;
            battle.State = Elements.BattleStateType.Battling;
            //var state = this.Perception.State as States.BattleState;
            state.Render.ShowPlayer(playerArmy);
            //SHOW Battle UI 
            if (App.GameAppliaction.BattleDebug)
            {
                GameDebug.LogDebug("ADD Player:");
                foreach (var i in soldier)
                {
                    var mons = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(i);
                    var skill = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(mons.SkillID);
                    GameDebug.Log("Monster:" + mons.ToDebugString());
                    GameDebug.Log("MonsterSkill:" + skill.ToDebugString());
                }
            }
        }
    }
}
