using Assets.Scripts.Combat.Battle.Elements; 
using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Actions
{
    public class AttackAction :GAction
    {
        public AttackAction(GObject obj, GPerception per, BattleSoldier soldier , BattleArmy enemy)
            :base(obj,per)
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
            var damage = Soldier.Config.Damage* Soldier.Soldier.Num;
            var dead = Enemy.CalHp(-damage);
            //Debug.Log(cur.Camp.ToString() +" Attack "+Enemy.Camp +" damage "+ damage +" hp="+ Enemy.HP+" Time="+Time.time);
            UI.UITipDrawer.Singleton.DrawNotify (cur.Camp.ToString() + " Attack " + Enemy.Camp + " damage " + damage + " hp=" + Enemy.HP + " Time=" + Time.time);
            
            if (dead)
                UI.UITipDrawer.Singleton.DrawNotify(Enemy.Camp.ToString() + "Dead");

            //throw new NotImplementedException();
        }
    }
}
