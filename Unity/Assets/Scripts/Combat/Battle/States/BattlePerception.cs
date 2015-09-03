using Assets.Scripts.Combat.Battle.Elements;
using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.States
{
    public class BattlePerception :GPerception
    {
        public BattlePerception(GState state) : base(state)
        { }

        public bool HaveDeadArmy()
        {
            bool have = false;
            int count = 0;
            State.Each<BattleArmy>((el) => { 
               if(el.IsDead)
               {
                   have = true;
                   return true;
               }
               count++;
               return false;
            });
            if (count == 1) return true;
            return have;
        }

        public bool PlayerDead()
        {
            bool have = false;
            State.Each<BattleArmy>((el) =>
            {
                if (el.IsDead && el.Camp == Proto.ArmyCamp.Player)
                {
                    have = true;
                    return true;
                }
                return false;
            });
            return have;
        }
        

        public BattleArmy GetEnemy(BattleArmy el)
        {
            BattleArmy enemy =null;
            State.Each<BattleArmy>((item) => {
                if(item.Camp != el.Camp)
                {
                    enemy = item;
                    return true;
                }
                return false;
            });
            return enemy;
        }

        internal BattleEl GetBattle()
        {
            BattleEl el =null;
            State.Each<BattleEl>((item) => { el = item; return true; });
            return el;
        }
    }
}
