
/*
#############################################
       
       *此代码为工具自动生成
       *请勿单独修改该文件

#############################################
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelConfig;
namespace ExcelConfig
{

    /// <summary>
    /// 制作表
    /// </summary>
    [ConfigFile("MakeConfig.json","MakeConfig")]
    public class MakeConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 需要道具
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String RequireItems { set; get; }
        
        /// <summary>
        /// 获得道具
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String RewardItems { set; get; }

    }

    /// <summary>
    /// 功能对应产出表
    /// </summary>
    [ConfigFile("FunctionConfig.json","FunctionConfig")]
    public class FunctionConfig:JSONConfigBase    {
        
        /// <summary>
        /// 功能Key
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String FunctionKey { set; get; }
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Name { set; get; }
        
        /// <summary>
        /// 产出 1:1|2:2
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String Produce { set; get; }

    }

    /// <summary>
    /// 英雄数据表
    /// </summary>
    [ConfigFile("ShopConfig.json","ShopConfig")]
    public class ShopConfig:JSONConfigBase    {
        
        /// <summary>
        /// 获得道具
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int Item { set; get; }
        
        /// <summary>
        /// 需要道具
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int RequireItem { set; get; }
        
        /// <summary>
        /// 需要数量
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int RequireNum { set; get; }
        
        /// <summary>
        /// 是否默认打开 0=N 1=Y
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int IsOpen { set; get; }

    }

    /// <summary>
    /// 士兵表
    /// </summary>
    [ConfigFile("SoldierConfig.json","SoldierConfig")]
    public class SoldierConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 技能名称
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String SkillName { set; get; }
        
        /// <summary>
        /// 最大血量
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int HPMax { set; get; }
        
        /// <summary>
        /// 伤害
        /// </summary>
        [ExcelConfigColIndex(4)]
        public int Damage { set; get; }
        
        /// <summary>
        /// 防御力
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int Defence { set; get; }
        
        /// <summary>
        /// 攻击类型
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int AttackType { set; get; }
        
        /// <summary>
        /// 攻击间隔毫秒
        /// </summary>
        [ExcelConfigColIndex(7)]
        public int AttackSpeed { set; get; }
        
        /// <summary>
        /// 士兵星级
        /// </summary>
        [ExcelConfigColIndex(8)]
        public int Star { set; get; }

    }

    /// <summary>
    /// 士兵进阶表
    /// </summary>
    [ConfigFile("SoldierLevelUpConfig.json","SoldierLevelUpConfig")]
    public class SoldierLevelUpConfig:JSONConfigBase    {
        
        /// <summary>
        /// 进阶前 -1为不需要士兵直接召唤
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int OldSoldierID { set; get; }
        
        /// <summary>
        /// 进阶后
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int NewSoldierID { set; get; }
        
        /// <summary>
        /// 需求材料 1:1|2:2
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String NeedItem { set; get; }

    }

    /// <summary>
    /// 常量数据表
    /// </summary>
    [ConfigFile("ConstValuesConfig.json","ConstValuesConfig")]
    public class ConstValuesConfig:JSONConfigBase    {
        
        /// <summary>
        /// 炼金生产出道具的编号
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int PRODUCE_ITEM_ID { set; get; }
        
        /// <summary>
        /// 资源生产计算间隔时间（秒）
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int RESOURCES_PRODUCE_TIME { set; get; }
        
        /// <summary>
        /// 收集间隔时间（秒）
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int COLLECT_TIME { set; get; }

    }

    /// <summary>
    /// 建筑表
    /// </summary>
    [ConfigFile("BuildingConfig.json","BuildingConfig")]
    public class BuildingConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 描述
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String Description { set; get; }

    }

    /// <summary>
    /// 建筑升级表
    /// </summary>
    [ConfigFile("BuildingLevelConfig.json","BuildingLevelConfig")]
    public class BuildingLevelConfig:JSONConfigBase    {
        
        /// <summary>
        /// 建筑编号
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int BuildID { set; get; }
        
        /// <summary>
        /// 等级
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int Level { set; get; }
        
        /// <summary>
        /// 道具列表
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String LevelUpRequire { set; get; }
        
        /// <summary>
        /// 升级触发事件
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String LevelUpEvent { set; get; }
        
        /// <summary>
        /// 参数
        /// </summary>
        [ExcelConfigColIndex(5)]
        public String LevelUpParams { set; get; }

    }

    /// <summary>
    /// 英雄数据表
    /// </summary>
    [ConfigFile("ProduceLevelUpConfig.json","ProduceLevelUpConfig")]
    public class ProduceLevelUpConfig:JSONConfigBase    {
        
        /// <summary>
        /// 点击次数
        /// </summary>
        [ExcelConfigColIndex(1)]
        public int ProduceCount { set; get; }
        
        /// <summary>
        /// 产出值
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int ProduceValue { set; get; }
        
        /// <summary>
        /// 对应的功能ID
        /// </summary>
        [ExcelConfigColIndex(3)]
        public int FunctionID { set; get; }
        
        /// <summary>
        /// 时间 
        /// </summary>
        [ExcelConfigColIndex(4)]
        public float CdTime { set; get; }

    }

    /// <summary>
    /// 资源产量表
    /// </summary>
    [ConfigFile("ResourcesProduceConfig.json","ResourcesProduceConfig")]
    public class ResourcesProduceConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 单位消耗资源
        /// </summary>
        [ExcelConfigColIndex(2)]
        public String CostItems { set; get; }
        
        /// <summary>
        /// 单位收获
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String RewardItems { set; get; }
        
        /// <summary>
        /// 说明描述
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String Description { set; get; }

    }

    /// <summary>
    /// 道具材料
    /// </summary>
    [ConfigFile("ItemConfig.json","ItemConfig")]
    public class ItemConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 最大上限
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int MaxCount { set; get; }
        
        /// <summary>
        /// 说明
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String Comment { set; get; }

    }

    /// <summary>
    /// 随机事件表
    /// </summary>
    [ConfigFile("RandomEventConfig.json","RandomEventConfig")]
    public class RandomEventConfig:JSONConfigBase    {
        
        /// <summary>
        /// 名称
        /// </summary>
        [ExcelConfigColIndex(1)]
        public String Name { set; get; }
        
        /// <summary>
        /// 触发类型
        /// </summary>
        [ExcelConfigColIndex(2)]
        public int Type { set; get; }
        
        /// <summary>
        /// 介绍
        /// </summary>
        [ExcelConfigColIndex(3)]
        public String Dialog1 { set; get; }
        
        /// <summary>
        /// 需要消耗材料
        /// </summary>
        [ExcelConfigColIndex(4)]
        public String NeedItem { set; get; }
        
        /// <summary>
        /// 事件触发概率
        /// </summary>
        [ExcelConfigColIndex(5)]
        public int EventProbability { set; get; }
        
        /// <summary>
        /// 万分比概率
        /// </summary>
        [ExcelConfigColIndex(6)]
        public int Probability { set; get; }
        
        /// <summary>
        /// 成功
        /// </summary>
        [ExcelConfigColIndex(7)]
        public String DialogOK { set; get; }
        
        /// <summary>
        /// 失败
        /// </summary>
        [ExcelConfigColIndex(8)]
        public String DialogFaiuler { set; get; }
        
        /// <summary>
        /// 成功奖励
        /// </summary>
        [ExcelConfigColIndex(9)]
        public String RewardItem { set; get; }
        
        /// <summary>
        /// 奖励士兵
        /// </summary>
        [ExcelConfigColIndex(10)]
        public String RewardSoidler { set; get; }

    }

 }
