using Assets.Scripts.Combat.Battle.Elements;
using Assets.Scripts.Combat.Simulate;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Actions
{
    public class AttackAction : GAction
    {
        public AttackAction(GObject obj, GPerception per, BattleSoldier soldier, BattleArmy enemy)
            : base(obj, per)
        {
            Soldier = soldier;
            Enemy = enemy;
        }

        public BattleArmy Enemy { private set; get; }
        public BattleSoldier Soldier { private set; get; }
        public override void DoAction()
        {
            var per = this.Perception as States.BattlePerception;
            var battle = per.GetBattle();
            var cur = Obj as BattleArmy;
            Soldier.AttackCdTime = Time.time;
            var skill = Soldier.SkillConfig.MainEffectType;
            //SkillDamageType
            //SkillEffectTaget
            BattleArmy target = null;
            switch ((SkillEffectTaget)Soldier.SkillConfig.MainEffectTarget)
            {
                case SkillEffectTaget.Enemy:
                    target = Enemy;
                    GameDebug.LogDebug("Target:Enemy!");
                    break;
                case SkillEffectTaget.OwnerTeam:
                    target = this.Obj as BattleArmy;
                    GameDebug.LogDebug("Target:Owner!");
                    break;
            }

            DamageResult result = null;

            switch ((SkillDamageType)Soldier.SkillConfig.MainEffectType)
            {
                case SkillDamageType.Cure:
                    target.CalHp(Soldier.SkillConfig.MainEffectNumber);//加血
                    GameDebug.LogDebug("Effect:Cure!");
                    break;
                case SkillDamageType.Damage:
                    result = target.DoAttack(this.Obj as BattleArmy, this.Soldier, -Soldier.SkillConfig.MainEffectNumber);//掉血
                    GameDebug.LogDebug("Effect:Damage D:" + result.Damage + " Miss:" + result.IsMiss + " dead:" + result.IsDead + " ismult:" + result.Mult);
                    break;
            }

            BattleArmy effectTarget = null;
            switch ((SkillEffectTaget)Soldier.SkillConfig.StatusTarget)
            {
                case SkillEffectTaget.Enemy:
                    effectTarget = Enemy;
                    GameDebug.LogDebug("target:enemy!");
                    break;
                case SkillEffectTaget.OwnerTeam:
                    effectTarget = this.Obj as BattleArmy;
                    GameDebug.LogDebug("target:owne!");
                    break;
            }

            switch ((SkillEffectType)Soldier.SkillConfig.StatusType)
            {
                case SkillEffectType.Hot:
                    //加护盾
                    //治疗
                    //effectTarget.AddAppendHP(Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1));
                    GameDebug.LogDebug("Adddef: num !" + Soldier.SkillConfig.Pars1);
                    break;
                case SkillEffectType.Dot:
                    GameDebug.LogDebug("dot: num !" + Soldier.SkillConfig.Pars1 + " " + Soldier.SkillConfig.Pars2 + " " + Soldier.SkillConfig.Pars3);
                    battle.DoDotEffect(Obj as BattleArmy, target, this.Soldier,
                        Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars2),
                        Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars3),
                        Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1));
                    break;
                case SkillEffectType.Giddy:
                    //眩晕
                    battle.DoGiddyEeffect(Obj as BattleArmy, Soldier, target, Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1));
                    GameDebug.LogDebug("giddy: num !" + Soldier.SkillConfig.Pars1);
                    break;
                case SkillEffectType.ReduceDamage:
                    GameDebug.LogDebug("ReduceDamage: num !" + Soldier.SkillConfig.Pars1);
                    battle.DoReduceDamage(Obj as BattleArmy, Soldier, target, Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1), Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars2));
                    break;
                case SkillEffectType.Cure:
                    GameDebug.LogDebug("SuckBlood: num !" + Soldier.SkillConfig.Pars1);
                    target.CalHp(Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1));//加血
                    break;
            }

            if (result != null)
            {
                var state = this.Perception.State as States.BattleState;
                state.Render.OnAttack(result, cur);
            }

			var battleState = this.Perception.State as States.BattleState;
			battleState.WaitForSeconds (0.3f);
        }


        //战斗计算在这里
    }
}
