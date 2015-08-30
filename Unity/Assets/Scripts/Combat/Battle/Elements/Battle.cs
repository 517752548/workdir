using Assets.Scripts.Combat.Simulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Combat.Battle.Elements
{
    public class BattleEl : GObject
    {

        public BattleEl(Controllors.BattleControllor battleControllor) : base(battleControllor) { }
        public BattleEl(Controllors.BattleControllor battleControllor, int battleGroupID)
            : this(battleControllor)
        {
            // TODO: Complete member initialization
            var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.BattleGroupConfig>(battleGroupID);
            var battleIDs = Tools.UtilityTool.SplitIDS(config.BattleIds);//.Split("|");
            var listBattle = ExcelConfig.ExcelToJSONConfigManager.Current
                .GetConfigs<ExcelConfig.BattleConfig>((t) =>
                {
                    return battleIDs.Contains(t.ID);
                });
            BattleGroup = config;
            Battles = listBattle;
        }

        public ExcelConfig.BattleGroupConfig BattleGroup { private set; get; }
        public ExcelConfig.BattleConfig[] Battles { private set; get; }

        public int CurrentIndex = 0;

        private List<BattleEffect> Bufs = new List<BattleEffect>();



        private void AddBuf(BattleEffect buf)
        {
            Bufs.Add(buf);
            buf.OnCreate();
        }
        /// <summary>
        /// 眩晕
        /// </summary>
        /// <param name="source"></param>
        /// <param name="soilder"></param>
        /// <param name="target"></param>
        /// <param name="time"></param>
        internal void DoGiddyEeffect(BattleArmy source, BattleSoldier soilder, BattleArmy target, int time)
        {
            var gi = new GiddyEffect(source, target, soilder, time);
            AddBuf(gi);
        }

        internal void DoReduceDamage(BattleArmy battleArmy, BattleSoldier Soldier, BattleArmy target, int attack, int durtion)
        {
            var rd = new ReduceDamageEffect(battleArmy, target, Soldier, durtion, attack);
            AddBuf(rd);
        }

        internal void DoDotEffect(BattleArmy source, BattleArmy target, BattleSoldier solider,  int durtion, int tick,int damage)
        {
            var dot = new DotEffect(source, target, solider, durtion, tick, damage);
            AddBuf(dot);
        }

        internal void TickEffect()
        {
            for (var i = 0; i < Bufs.Count; i++) {
                var t = Bufs[i];
                if (t.DoTick())
                {
                    _deeeffect.Enqueue(t);
                }
            }

            while (_deeeffect.Count > 0)
            {
                var temp = _deeeffect.Dequeue();
                temp.OnDestory();
                Bufs.Remove(temp);
            }
        }

        private Queue<BattleEffect> _deeeffect = new Queue<BattleEffect>();
    }

    public abstract class BattleEffect
    {

        public BattleEffect(BattleArmy source, BattleArmy target, BattleSoldier soilder, int durtion)
        {
            Source = source;
            Target = target;
            Soldier = soilder;
            Durtion = (float)durtion / 1000f;
            TickTime = -1;
            LastTickTime = StartTime = Time.time;
             
        }

        public float Durtion { set; get; }

        public float TickTime { set; get; }

        public BattleSoldier Soldier { private set; get; }

        public BattleArmy Source { private set; get; }

        public BattleArmy Target { private set; get; }

        public virtual void OnTick() { }

        public virtual void OnCreate() { }

        public virtual void OnDestory() { }

        private float StartTime = 0;

        private float LastTickTime = 0;

        public bool DoTick()
        {
            var time = Time.time;
            if (TickTime > 0 && time > LastTickTime + TickTime)
            {
                LastTickTime = time;
                OnTick();
            }
            return (time > StartTime + Durtion) ;
        }
    }


    public class GiddyEffect : BattleEffect
    {
        public GiddyEffect(BattleArmy source, BattleArmy target, BattleSoldier soilder, int durtion) : base(source, target, soilder, durtion) { }
        public override void OnCreate()
        {
            base.OnCreate();
            Target.LockFlag(ActionState.CAN_ATTACK);
        }

        public override void OnDestory()
        {
            base.OnDestory();
            Target.UnlockFlag(ActionState.CAN_ATTACK);
        }
    }

    public class ReduceDamageEffect : BattleEffect
    {
        public ReduceDamageEffect(BattleArmy source, BattleArmy target, BattleSoldier soilder, int durtion, int attack)
            : base(source, target, soilder, durtion)
        {
            Attack = attack;
        }
        public int Attack { private set; get; }

        public override void OnCreate()
        {
            base.OnCreate();
            Target.ReduceDamage += Attack;
        }

        public override void OnDestory()
        {
            base.OnDestory();
            Target.ReduceDamage -= Attack;
        }
    }


    public class DotEffect : BattleEffect
    {
        public DotEffect(BattleArmy source, BattleArmy target, BattleSoldier soilder, int durtion, int tick, int damage)
            : base(source, target, soilder, durtion)
        {
            Damage = damage;
            TickTime = tick;
        }

        public override void OnTick()
        {
            base.OnTick();
            Target.DoAttack(this.Source, this.Soldier, Damage);
        }

        public int Damage { get; set; }
    }
}
