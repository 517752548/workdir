using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIBattle")]
    partial class UIBattle : UIAutoGenWindow
    {
        public class StarGridTableTemplate : TableItemTemplate
        {
            public StarGridTableTemplate(){}

            public override void InitTemplate()
            {

            }
        }
        public class SkillGridTableTemplate : TableItemTemplate
        {
            public SkillGridTableTemplate(){}
            public UIButton Bt_skill;
            public UISprite job;

            public override void InitTemplate()
            {
                Bt_skill = FindChild<UIButton>("Bt_skill");
                job = FindChild<UISprite>("job");

            }
        }


        public UIButton bt_close;
        public UILabel lb_title;
        public UISprite s_title;
        public Transform MonsterRoot;
        public UISlider HpBar;
        public UISlider SkillBar;
        public UISprite jobicon;
        public UILabel lb_monsterName;
        public UIGrid StarGrid;
        public UITexture Monster_coin;
        public UILabel lb_lvl_monster;
        public Transform daoguangFX;
        public Transform zhuahenFx;
        public UITexture battleground;
        public UISlider PlayerHpBar;
        public UIGrid SkillGrid;
        public UIButton bt_battleMode;
        public UIButton bt_addHp;


        public UITableManager<AutoGenTableItem<StarGridTableTemplate, StarGridTableModel>> StarGridTableManager = new UITableManager<AutoGenTableItem<StarGridTableTemplate, StarGridTableModel>>();
        public UITableManager<AutoGenTableItem<SkillGridTableTemplate, SkillGridTableModel>> SkillGridTableManager = new UITableManager<AutoGenTableItem<SkillGridTableTemplate, SkillGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            lb_title = FindChild<UILabel>("lb_title");
            s_title = FindChild<UISprite>("s_title");
            MonsterRoot = FindChild<Transform>("MonsterRoot");
            HpBar = FindChild<UISlider>("HpBar");
            SkillBar = FindChild<UISlider>("SkillBar");
            jobicon = FindChild<UISprite>("jobicon");
            lb_monsterName = FindChild<UILabel>("lb_monsterName");
            StarGrid = FindChild<UIGrid>("StarGrid");
            Monster_coin = FindChild<UITexture>("Monster_coin");
            lb_lvl_monster = FindChild<UILabel>("lb_lvl_monster");
            daoguangFX = FindChild<Transform>("daoguangFX");
            zhuahenFx = FindChild<Transform>("zhuahenFx");
            battleground = FindChild<UITexture>("battleground");
            PlayerHpBar = FindChild<UISlider>("PlayerHpBar");
            SkillGrid = FindChild<UIGrid>("SkillGrid");
            bt_battleMode = FindChild<UIButton>("bt_battleMode");
            bt_addHp = FindChild<UIButton>("bt_addHp");

            StarGridTableManager.InitFromGrid(StarGrid);
            SkillGridTableManager.InitFromGrid(SkillGrid);

        }
        public static UIBattle Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIBattle>();
            ui.ShowWindow();
            return ui;
        }
    }
}