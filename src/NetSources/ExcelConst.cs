/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
    public enum MapEventType
    {
        /// <summary>
        /// ������
        /// </summary>
        BronPos=1,
        /// <summary>
        /// �����
        /// </summary>
        ReLivePos=2,
        /// <summary>
        /// ���������
        /// </summary>
        PKEnterPos=3,
        /// <summary>
        /// ��ͼ�ڵ�
        /// </summary>
        ScrectShopPos=4,
        /// <summary>
        /// ���ر���
        /// </summary>
        ChestPos=5,
        /// <summary>
        /// ��վ������
        /// </summary>
        RechargePos=6,
        /// <summary>
        /// �̶�ս����
        /// </summary>
        BattlePos=7,
        /// <summary>
        /// ����ɢ���
        /// </summary>
        RandomChestPos=8,
        /// <summary>
        /// ���
        /// </summary>
        GoldPos=9,
        /// <summary>
        /// ����¼���
        /// </summary>
        RandomEvnetPos=10,
        /// <summary>
        /// �²㴫�͵�
        /// </summary>
        GoToNextLvlPos=11,
        /// <summary>
        /// �سǵ�
        /// </summary>
        GoHomePos=12,

    }
    public enum ItemType
    {
        /// <summary>
        /// �ɾ�
        /// </summary>
        Achievement=1,
        /// <summary>
        /// ����
        /// </summary>
        Materials=2,
        /// <summary>
        /// ͼֽ
        /// </summary>
        Diagram=3,
        /// <summary>
        /// �س�
        /// </summary>
        RecallScroll=4,
        /// <summary>
        /// Ӷ��������
        /// </summary>
        Indenture=5,
        /// <summary>
        /// �츳��
        /// </summary>
        Book=6,
        /// <summary>
        /// ��Ұ�
        /// </summary>
        GoldPackage=7,
        /// <summary>
        /// ����
        /// </summary>
        Chest=8,
        /// <summary>
        /// ������
        /// </summary>
        Cost=9,
        /// <summary>
        /// ����Կ��
        /// </summary>
        ChestKey=10,
        /// <summary>
        /// ʱЧ��
        /// </summary>
        TimeToUse=11,
        /// <summary>
        /// ��������չ����
        /// </summary>
        Tools=12,

    }
    public enum BuildingType
    {
        /// <summary>
        /// ũ��
        /// </summary>
        Crop=1,
        /// <summary>
        /// ���
        /// </summary>
        House=2,
        /// <summary>
        /// �����
        /// </summary>
        Food=3,
        /// <summary>
        /// ľ�ĳ�
        /// </summary>
        Wood=4,
        /// <summary>
        /// ��г�
        /// </summary>
        Clothing=5,
        /// <summary>
        /// ��Ҥ
        /// </summary>
        Ming=6,
        /// <summary>
        /// ������
        /// </summary>
        Explore=7,

    }
    public enum SkillDamageType
    {
        /// <summary>
        /// ����
        /// </summary>
        Cure=1,
        /// <summary>
        /// �˺�
        /// </summary>
        Damage=2,

    }
    public enum SkillTargetType
    {
        /// <summary>
        /// �Լ�
        /// </summary>
        Owner=1,
        /// <summary>
        /// �ѷ�
        /// </summary>
        OwnerTeam=2,
        /// <summary>
        /// �з�
        /// </summary>
        Enemy=3,

    }
    public enum SkillEffectTaget
    {
        /// <summary>
        /// �Լ�
        /// </summary>
        Owner=1,
        /// <summary>
        /// �ѷ�
        /// </summary>
        OwnerTeam=2,
        /// <summary>
        /// �з�
        /// </summary>
        Enemy=3,

    }
    public enum SkillEffectType
    {
        /// <summary>
        /// �����˺�
        /// </summary>
        Dot=1,
        /// <summary>
        /// ���ٹ���
        /// </summary>
        ReduceDamage=2,
        /// <summary>
        /// ѣ��
        /// </summary>
        Giddy=3,
        /// <summary>
        /// ��Ѫ
        /// </summary>
        SuckBlood=4,
        /// <summary>
        /// ����
        /// </summary>
        AddDef=5,

    }
    public enum AchievementEventType
    {
        /// <summary>
        /// �̲����ĵ�ָ����
        /// </summary>
        GoldCost=1,
        /// <summary>
        /// ������ָ���ȼ�
        /// </summary>
        BuildLevel=2,
        /// <summary>
        /// ָ���齨����ָ���ȼ�
        /// </summary>
        AllBuildLevel=3,
        /// <summary>
        /// ̽���ȵ�ָ����
        /// </summary>
        Explore=4,
        /// <summary>
        /// ���ֽ�����ָ���ȼ�
        /// </summary>
        ArmyLevel=5,
        /// <summary>
        /// ���ָ����
        /// </summary>
        PlaySkill=6,
        /// <summary>
        /// ���ĸ�����ָ������
        /// </summary>
        CostFood=7,
        /// <summary>
        /// ɱ��boss
        /// </summary>
        KillBoss=8,
        /// <summary>
        /// ����
        /// </summary>
        ShareGame=9,
        /// <summary>
        /// ɱ��ָ��������
        /// </summary>
        KillMonster=10,
        /// <summary>
        /// �������ص�ͼ
        /// </summary>
        EnterMap=11,
        /// <summary>
        /// ����ָ����������
        /// </summary>
        OpenChest=12,
        /// <summary>
        /// �ɹ���������
        /// </summary>
        DefenceEnemy=13,

    }
    public enum RandomEventTickType
    {
        /// <summary>
        /// ��½��Ϸ
        /// </summary>
        WhenLoginGame=1,
        /// <summary>
        /// �̶�ʱ��ˢ�´���
        /// </summary>
        FixTime=2,
        /// <summary>
        /// �̶��¼�
        /// </summary>
        FixEvent=3,

    }
    public enum PlaySkillType
    {
        /// <summary>
        /// ���ּ�����cd
        /// </summary>
        NOFristCD=1,
        /// <summary>
        /// ̽�����Ľ���
        /// </summary>
        ExploreCostLower=2,
        /// <summary>
        /// ̽���ɼ���Χ����
        /// </summary>
        ExploreViewDis=3,
        /// <summary>
        /// ��������
        /// </summary>
        AttAppend=4,

    }
    public enum StoreUnlockType
    {
        /// <summary>
        /// �������
        /// </summary>
        None=-1,
        /// <summary>
        /// ָ�������ﵽָ���ȼ�����
        /// </summary>
        BuildGetTargetLevel=1,
        /// <summary>
        /// ָ����ͼ�ﵽָ��̽���Ƚ���
        /// </summary>
        ExploreGetTarget=2,

    }
}