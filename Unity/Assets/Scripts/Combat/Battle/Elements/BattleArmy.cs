using Assets.Scripts.Combat.Simulate;
using ExcelConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.Elements
{
    public class BattleSoldier
    {
        public Proto.Soldier Soldier { set; get; }
        public ExcelConfig.MonsterConfig Config { set; get; }
        public float AttackCdTime { set; get; }
    }
    public class BattleArmy :GObject
    {
        public BattleArmy(GControllor controllor, Proto.Army army):base(controllor)
        {
            Soldiers = new List<BattleSoldier>();
            Army = army;
            HP =0;
            foreach(var i in Army.Soldiers)
            {
                var so = new BattleSoldier {
                  AttackCdTime = 0,
                  Config = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(i.ConfigID),
                  Soldier = i
                };

                Soldiers.Add(so);
                HP += (so.Config.Hp * i.Num);
            }

            MaxHP = HP;

        }

        public List<BattleSoldier> Soldiers { set; get; }

        public Proto.ArmyCamp Camp { get { return Army.Camp; } }

        public Proto.Army Army { set; get; }

        public int MaxHP {private set; get; }

        public int HP { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        /// <returns>是否死亡</returns>
        public bool CalHp(int hp)
        {
            if(hp>0) //加血
            {
                HP += hp;
                if (HP > MaxHP) HP = MaxHP;
                return false;
            }
            else
            {
                HP += hp;
                if (HP <= 0) HP = 0;
                return HP == 0;
            }
        }
        public bool IsDead { get { return HP == 0; } }
    }
}
