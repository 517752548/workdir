/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
   /// <summary>
    /// 地图事件
    /// </summary>
    public enum MapEventType
    {
        /// <summary>
        /// 默认
        /// </summary>
        None=0,
        /// <summary>
        /// 出生点
        /// </summary>
        BronPos=1,
        /// <summary>
        /// 复活点
        /// </summary>
        ReLivePos=2,
        /// <summary>
        /// 竞技场入口
        /// </summary>
        PKEnterPos=3,
        /// <summary>
        /// 地图黑店
        /// </summary>
        ScrectShopPos=4,
        /// <summary>
        /// 神秘宝藏
        /// </summary>
        ChestPos=5,
        /// <summary>
        /// 驿站补给点
        /// </summary>
        RechargePos=6,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos=7,
        /// <summary>
        /// 宝箱散落点
        /// </summary>
        RandomChestPos=8,
        /// <summary>
        /// 金矿
        /// </summary>
        GoldPos=9,
        /// <summary>
        /// 随机事件点
        /// </summary>
        RandomEvnetPos=10,
        /// <summary>
        /// 下层传送点
        /// </summary>
        GoToNextLvlPos=11,
        /// <summary>
        /// 回城点
        /// </summary>
        GoHomePos=12,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos2=13,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos3=14,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos4=15,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos5=16,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos6=17,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos7=18,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos8=19,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos9=20,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos10=21,
        /// <summary>
        /// 固定战斗点
        /// </summary>
        BattlePos11=22,

    }
   /// <summary>
    /// 道具类型
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// 默认
        /// </summary>
        None=0,
        /// <summary>
        /// 成就
        /// </summary>
        Achievement=1,
        /// <summary>
        /// 材料
        /// </summary>
        Materials=2,
        /// <summary>
        /// 图纸
        /// </summary>
        Diagram=3,
        /// <summary>
        /// 回城
        /// </summary>
        RecallScroll=4,
        /// <summary>
        /// 佣兵卖身契
        /// </summary>
        Indenture=5,
        /// <summary>
        /// 天赋书
        /// </summary>
        Book=6,
        /// <summary>
        /// 金币包
        /// </summary>
        GoldPackage=7,
        /// <summary>
        /// 保险
        /// </summary>
        Chest=8,
        /// <summary>
        /// 补给类
        /// </summary>
        Cost=9,
        /// <summary>
        /// 保险钥匙
        /// </summary>
        ChestKey=10,
        /// <summary>
        /// 时效性
        /// </summary>
        TimeToUse=11,
        /// <summary>
        /// 功能性扩展道具
        /// </summary>
        Tools=12,

    }
   /// <summary>
    /// 建筑类型
    /// </summary>
    public enum BuildingType
    {
        /// <summary>
        /// 农田
        /// </summary>
        Crop=1,
        /// <summary>
        /// 民居
        /// </summary>
        House=2,
        /// <summary>
        /// 面点铺
        /// </summary>
        Food=3,
        /// <summary>
        /// 木材厂
        /// </summary>
        Wood=4,
        /// <summary>
        /// 绸缎厂
        /// </summary>
        Clothing=5,
        /// <summary>
        /// 矿窑
        /// </summary>
        Ming=6,
        /// <summary>
        /// 演武厅
        /// </summary>
        Explore=7,

    }
   /// <summary>
    /// 建筑解锁条件
    /// </summary>
    public enum BuildingUnlockType
    {
        /// <summary>
        /// 无需条件
        /// </summary>
        NONE=1,
        /// <summary>
        /// 需要前置
        /// </summary>
        NeedBuild=2,

    }
   /// <summary>
    /// 伤害类型
    /// </summary>
    public enum SkillDamageType
    {
        /// <summary>
        /// 伤害
        /// </summary>
        Damage=1,
        /// <summary>
        /// 治疗
        /// </summary>
        Cure=2,

    }
   /// <summary>
    /// 技能目标类型
    /// </summary>
    public enum SkillTargetType
    {
        /// <summary>
        /// 友方
        /// </summary>
        OwnerTeam=1,
        /// <summary>
        /// 敌方
        /// </summary>
        Enemy=2,

    }
   /// <summary>
    /// 技能效果目标类型
    /// </summary>
    public enum SkillEffectTaget
    {
        /// <summary>
        /// 友方
        /// </summary>
        OwnerTeam=1,
        /// <summary>
        /// 敌方
        /// </summary>
        Enemy=2,

    }
   /// <summary>
    /// 技能效果类型
    /// </summary>
    public enum SkillEffectType
    {
        /// <summary>
        /// 直接治疗
        /// </summary>
        Cure=1,
        /// <summary>
        /// HOT 持续治疗
        /// </summary>
        Hot=2,
        /// <summary>
        /// 持续伤害
        /// </summary>
        Dot=3,
        /// <summary>
        /// 减少攻击
        /// </summary>
        ReduceDamage=4,
        /// <summary>
        /// 眩晕
        /// </summary>
        Giddy=5,

    }
   /// <summary>
    /// 成就达成条件类型
    /// </summary>
    public enum AchievementEventType
    {
        /// <summary>
        /// 盘缠消耗到指定量
        /// </summary>
        GoldCost=1,
        /// <summary>
        /// 建筑到指定等级
        /// </summary>
        BuildLevel=2,
        /// <summary>
        /// 指定组建筑到指定等级
        /// </summary>
        AllBuildLevel=3,
        /// <summary>
        /// 探索度到指定数
        /// </summary>
        Explore=4,
        /// <summary>
        /// 兵种进化到指定等级
        /// </summary>
        ArmyLevel=5,
        /// <summary>
        /// 获得指定天
        /// </summary>
        PlaySkill=6,
        /// <summary>
        /// 消耗干粮达指定数量
        /// </summary>
        CostFood=7,
        /// <summary>
        /// 杀死boss
        /// </summary>
        KillBoss=8,
        /// <summary>
        /// 分享
        /// </summary>
        ShareGame=9,
        /// <summary>
        /// 杀死指定怪物数
        /// </summary>
        KillMonster=10,
        /// <summary>
        /// 进入神秘地图
        /// </summary>
        EnterMap=11,
        /// <summary>
        /// 开启指定数量宝箱
        /// </summary>
        OpenChest=12,
        /// <summary>
        /// 成功抵御入侵
        /// </summary>
        DefenceEnemy=13,

    }
   /// <summary>
    /// 随机事件类型
    /// </summary>
    public enum RandomEventTickType
    {
        /// <summary>
        /// 登陆游戏
        /// </summary>
        WhenLoginGame=1,
        /// <summary>
        /// 固定时间刷新触发
        /// </summary>
        FixTime=2,
        /// <summary>
        /// 固定事件
        /// </summary>
        FixEvent=3,

    }
   /// <summary>
    /// 玩家技能类型
    /// </summary>
    public enum PlaySkillType
    {
        /// <summary>
        /// 首轮技能无cd
        /// </summary>
        NOFristCD=1,
        /// <summary>
        /// 探索消耗降低
        /// </summary>
        ExploreCostLower=2,
        /// <summary>
        /// 探索可见范围增加
        /// </summary>
        ExploreViewDis=3,
        /// <summary>
        /// 修正属性
        /// </summary>
        AttAppend=4,

    }
   /// <summary>
    /// 商店解锁条件类型
    /// </summary>
    public enum StoreUnlockType
    {
        /// <summary>
        /// 无需解锁
        /// </summary>
        None=-1,
        /// <summary>
        /// 指定建筑达到指定等级解锁
        /// </summary>
        BuildGetTargetLevel=1,
        /// <summary>
        /// 指定地图达到指定探索度解锁
        /// </summary>
        ExploreGetTarget=2,

    }
   /// <summary>
    /// 雇佣条件类型
    /// </summary>
    public enum EmployCondtionType
    {
        /// <summary>
        /// 
        /// </summary>
        None=-1,
        /// <summary>
        /// 通关副本   （1|2|3）
        /// </summary>
        CompleteMap=1,
        /// <summary>
        /// 获得成就（1|2|3）
        /// </summary>
        GetAchievement=2,
        /// <summary>
        /// 获得道具 （1：1|2：3|3：1）
        /// </summary>
        GetItem=3,

    }
   /// <summary>
    /// 雇佣消耗货币类型
    /// </summary>
    public enum EmployCostCurrent
    {
        /// <summary>
        /// 消耗金币
        /// </summary>
        Gold=1,
        /// <summary>
        /// 消耗钻石
        /// </summary>
        Coin=2,

    }
   /// <summary>
    /// 
    /// </summary>
    public enum MakeItemUnlockType
    {
        /// <summary>
        /// 无需条件
        /// </summary>
        NONE=0,
        /// <summary>
        /// 需要蓝图
        /// </summary>
        NeedScroll=1,

    }
   /// <summary>
    /// 
    /// </summary>
    public enum HeroJob
    {
        /// <summary>
        /// 仙
        /// </summary>
        Xian=1,
        /// <summary>
        /// 佛
        /// </summary>
        Fo=4,
        /// <summary>
        /// 妖
        /// </summary>
        Yao=2,
        /// <summary>
        /// 冥
        /// </summary>
        Ming=3,

    }
}