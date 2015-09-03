using Assets.Scripts.Combat.Simulate;
using Assets.Scripts.Tools;
using ExcelConfig;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat.Battle.Elements
{

    public enum ActionState
    {
        NONE = 0,
        CAN_ATTACK = 1, //攻击 
    }
    public class BattleSoldier
    {
        public BattleSoldier(Soldier soldier)
        {
            Config = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(soldier.ConfigID);
            SkillConfig = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(Config.SkillID);
            AttackCdTime = 0;
        }
        public Proto.Soldier Soldier { set; get; }
        public ExcelConfig.MonsterConfig Config { set; get; }
        public float AttackCdTime { set; get; }
        public ExcelConfig.SkillConfig SkillConfig { set; get; }
    }

    public class BattleArmy : GObject
    {
        public BattleArmy(GControllor controllor, Proto.Army army)
            : base(controllor)
        {
            Soldiers = new List<BattleSoldier>();
            Army = army;
            HP = 0;
            foreach (var i in Army.Soldiers)
            {
                var so = new BattleSoldier(i);
                Soldiers.Add(so);
                HP += (so.Config.Hp * i.Num);
            }

            MaxHP = HP;

            foreach (var i in Enum.GetValues(typeof(ActionState)))
            {
                this._lockState.Add((ActionState)i, 0);
            }

        }

        public List<BattleSoldier> Soldiers { set; get; }

        public Proto.ArmyCamp Camp { get { return Army.Camp; } }

        public Proto.Army Army { set; get; }

        public int MaxHP { private set; get; }

        public int HP { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        /// <returns>是否死亡</returns>
        public bool CalHp(int hp)
        {
            if (hp > 0) //加血
            {
                HP += hp;
                if (HP > MaxHP) HP = MaxHP;
                return false;
            }
            else
            {
                //先打护盾
                if (AppendHP > 0)
                {
                    AppendHP += hp;
                    //如果还>0 
                    if (AppendHP >= 0) return false;
                    else { hp = AppendHP; AppendHP = 0; }
                }
                HP += hp;
                if (HP <= 0) HP = 0;

                var isdead = HP == 0;
                if (isdead)
                    this.Enable = false;
                return isdead;
            }
        }
        public bool IsDead { get { return HP == 0; } }

        #region Lock
        private int _State = 0xFFFF;

        public bool CanDoAction(ActionState flag)
        {
            return ((int)flag & _State) == (int)flag;
        }

        public void LockFlag(ActionState flag)
        {
            if (_lockState[flag] == 0)
            {
                _State &= ~(int)flag;
            }
            _lockState[flag]++;
        }

        public void UnlockFlag(ActionState flag)
        {
            if (_lockState[flag] > 0)
            {
                _lockState[flag]--;
                if (_lockState[flag] == 0)
                {
                    _State = (_State | (int)flag);
                }
            }
        }

        private Dictionary<ActionState, int> _lockState = new Dictionary<ActionState, int>();
        #endregion

        public int AppendHP { private set; get; }

        public void AddAppendHP(int hp)
        {
            if (hp < 0) return;
            AppendHP += hp;
        }

        public DamageResult DoAttack(BattleArmy source, BattleSoldier soldier, int damage)
        {
            var result = new DamageResult { Damage = 0, IsMiss = false, Mult = 1, IsDead = false };
            var mult = 1f; //暴击倍数
            var multPro = 0;//暴击概率
            var addRate = 1f; //默认的攻击克制系数 
            var defRate = 1f; //防御减免

            //计算是否命中  

            //玩家攻击的时候计算
            if (source.Camp == Proto.ArmyCamp.Player)
            {
                //来自玩家的伤害
                var category = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterCategoryConfig>(soldier.Config.Type);
                var mainSoilder = this.Soldiers[0];
                if (category.AddCategory == mainSoilder.Config.Type)
                {
                    //克制 修正默认攻击参数
                    addRate = category.AddRate;
                    //设置暴击概率
                    multPro = (int)soldier.Config.CrtPro;
                    //暴击倍数
                    mult = soldier.Config.Mult;
                }
            }

            //玩家受到攻击的时候
            //判断是否暴击
            if(!(multPro > 0 && GRandomer.Probability10000(multPro)))
            {
                mult = 1;//无暴击修正回去
            }

            var resultDamage = damage * mult * addRate * defRate;
            result.Damage = (int)resultDamage;
            result.IsDead = CalHp(result.Damage);
            return result;
        }

        public int ReduceDamage { get; set; }
    }

    public class DamageResult
    {
        public float Mult { set; get; }
        public int Damage { set; get; }
        public bool IsMiss { set; get; }
        public bool IsDead { set; get; }
    }
}



