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
        public class formStartGridTableTemplate : TableItemTemplate
        {
            public formStartGridTableTemplate(){}
            public UISprite HSprite;

            public override void InitTemplate()
            {
                HSprite = FindChild<UISprite>("HSprite");

            }
        }


        public UISprite formjob;
        public UILabel fromName;
        public UIButton bt_close;
        public UILabel lb_Message;
        public UIButton bt_ok;
        public UIButton bt_cancel;
        public UITexture T_Icon;
        public UIGrid formStartGrid;
        public UILabel lb_hp;
        public UILabel lb_hp_next;
        public UILabel lb_attack;
        public UILabel lb_attack_next;
        public UILabel lb_skill;
        public UILabel lb_skill_next;
        public UILabel lb_speed;
        public UILabel lb_speed_next;


        public UITableManager<AutoGenTableItem<formStartGridTableTemplate, formStartGridTableModel>> formStartGridTableManager = new UITableManager<AutoGenTableItem<formStartGridTableTemplate, formStartGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            formjob = FindChild<UISprite>("formjob");
            fromName = FindChild<UILabel>("fromName");
            bt_close = FindChild<UIButton>("bt_close");
            lb_Message = FindChild<UILabel>("lb_Message");
            bt_ok = FindChild<UIButton>("bt_ok");
            bt_cancel = FindChild<UIButton>("bt_cancel");
            T_Icon = FindChild<UITexture>("T_Icon");
            formStartGrid = FindChild<UIGrid>("formStartGrid");
            lb_hp = FindChild<UILabel>("lb_hp");
            lb_hp_next = FindChild<UILabel>("lb_hp_next");
            lb_attack = FindChild<UILabel>("lb_attack");
            lb_attack_next = FindChild<UILabel>("lb_attack_next");
            lb_skill = FindChild<UILabel>("lb_skill");
            lb_skill_next = FindChild<UILabel>("lb_skill_next");
            lb_speed = FindChild<UILabel>("lb_speed");
            lb_speed_next = FindChild<UILabel>("lb_speed_next");

            formStartGridTableManager.InitFromGrid(formStartGrid);

        }
        public static UIArmyLevelUp Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIArmyLevelUp>();
            ui.ShowWindow();
            return ui;
        }
    }
}