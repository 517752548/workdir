using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIArmyLevelUp")]
    partial class UIArmyLevelUp : UIAutoGenWindow
    {
        public class LevelUpGridTableTemplate : TableItemTemplate
        {
            public LevelUpGridTableTemplate(){}
            public UILabel lb_hp;
            public UILabel lb_attack;
            public UILabel lb_skill;
            public UILabel lb_speed;
            public UITexture formicon;
            public UIGrid formStartGrid;
            public UISprite formjob;
            public UILabel fromName;

            public override void InitTemplate()
            {
                lb_hp = FindChild<UILabel>("lb_hp");
                lb_attack = FindChild<UILabel>("lb_attack");
                lb_skill = FindChild<UILabel>("lb_skill");
                lb_speed = FindChild<UILabel>("lb_speed");
                formicon = FindChild<UITexture>("formicon");
                formStartGrid = FindChild<UIGrid>("formStartGrid");
                formjob = FindChild<UISprite>("formjob");
                fromName = FindChild<UILabel>("fromName");

            }
        }


        public UIButton bt_close;
        public UILabel lb_Message;
        public UIButton bt_ok;
        public UIButton bt_cancel;
        public UIGrid LevelUpGrid;


        public UITableManager<AutoGenTableItem<LevelUpGridTableTemplate, LevelUpGridTableModel>> LevelUpGridTableManager = new UITableManager<AutoGenTableItem<LevelUpGridTableTemplate, LevelUpGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            lb_Message = FindChild<UILabel>("lb_Message");
            bt_ok = FindChild<UIButton>("bt_ok");
            bt_cancel = FindChild<UIButton>("bt_cancel");
            LevelUpGrid = FindChild<UIGrid>("LevelUpGrid");

            LevelUpGridTableManager.InitFromGrid(LevelUpGrid);

        }
        public static UIArmyLevelUp Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIArmyLevelUp>();
            ui.ShowWindow();
            return ui;
        }
    }
}