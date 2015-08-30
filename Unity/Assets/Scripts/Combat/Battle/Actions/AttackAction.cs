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
            var cur = Obj as BattleArmy;
            Soldier.AttackCdTime = Time.time;
            var per = this.Perception as States.BattlePerception;

            var battle = per.GetBattle();
            var skill = Soldier.SkillConfig.MainEffectType;
            //SkillDamageType
            //SkillEffectTaget
            BattleArmy target = null;
            switch ((SkillEffectTaget)Soldier.SkillConfig.MainEffectTarget)
            {
                case SkillEffectTaget.Enemy:
                    target = Enemy;
                    break;
                case SkillEffectTaget.OwnerTeam:
                    target = this.Obj as BattleArmy;
                    break;
            }

            DamageResult result = null;

            switch ((SkillDamageType)Soldier.SkillConfig.MainEffectType)
            {
                case SkillDamageType.Cure:
                    target.CalHp(Soldier.SkillConfig.MainEffectNumber);//加血
                    break;
                case SkillDamageType.Damage:
                    result = target.DoAttack(this.Obj as BattleArmy, this.Soldier, -Soldier.SkillConfig.MainEffectNumber);//掉血
                    break;
            }

            BattleArmy effectTarget = null;
            switch ((SkillEffectTaget)Soldier.SkillConfig.StatusTarget)
            {
                case SkillEffectTaget.Enemy:
                    effectTarget = Enemy;
                    break;
                case SkillEffectTaget.OwnerTeam:
                    effectTarget = this.Obj as BattleArmy;
                    break;
            }

            switch ((SkillEffectType)Soldier.SkillConfig.StatusType)
            { 
                case SkillEffectType.AddDef:
                    //加护盾
                    effectTarget.AddAppendHP(Tools.UtilityTool.ConvertToInt( Soldier.SkillConfig.Pars1));
                    break;
                case SkillEffectType.Dot:
                    battle.DoDotEffect(Obj as BattleArmy, target, this.Soldier,
                        Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars2), 
                        Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars3), 
                        Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1));
                    break;
                case SkillEffectType.Giddy:
                    //眩晕
                    battle.DoGiddyEeffect(Obj as BattleArmy, Soldier,target, Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1));
                    break;
                case SkillEffectType.ReduceDamage:

                    battle.DoReduceDamage(Obj as BattleArmy, Soldier, target, Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1), Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars2));
                    break;
                case SkillEffectType.SuckBlood:
                    target.CalHp(Tools.UtilityTool.ConvertToInt(Soldier.SkillConfig.Pars1));//加血
                    break;
            }

            if (result == null) return;

           

            //Debug.Log(cur.Camp.ToString() +" Attack "+Enemy.Camp +" damage "+ damage +" hp="+ Enemy.HP+" Time="+Time.time);
            UI.UITipDrawer.Singleton.DrawNotify(
                cur.Camp.ToString() + " Attack " + Enemy.Camp + " damage " + result.Damage + " hp=" + Enemy.HP + " Time=" + Time.time);

            if (result.IsDead)
                UI.UITipDrawer.Singleton.DrawNotify(Enemy.Camp.ToString() + "Dead");

            //throw new NotImplementedException();
        }

        //战斗计算在这里
    }
}
