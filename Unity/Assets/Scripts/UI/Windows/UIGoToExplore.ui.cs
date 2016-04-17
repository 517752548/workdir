using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIGoToExplore")]
    partial class UIGoToExplore : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UITexture icon;
            public UISprite s_death;
            public UILabel lb_name;
            public UISprite s_job;
            public UIGrid StartGrid;
            public UILabel lb_skill;
            public UILabel lb_speed;
            public UILabel lb_hp;
            public UILabel lb_attack;
            public UISprite s_lock;

            public override void InitTemplate()
            {
                icon = FindChild<UITexture>("icon");
                s_death = FindChild<UISprite>("s_death");
                lb_name = FindChild<UILabel>("lb_name");
                s_job = FindChild<UISprite>("s_job");
                StartGrid = FindChild<UIGrid>("StartGrid");
                lb_skill = FindChild<UILabel>("lb_skill");
                lb_speed = FindChild<UILabel>("lb_speed");
                lb_hp = FindChild<UILabel>("lb_hp");
                lb_attack = FindChild<UILabel>("lb_attack");
                s_lock = FindChild<UISprite>("s_lock");

            }
        }


        public UIButton bt_close;
        public UIButton bt_go;
        public UIButton bt_add;
        public UIButton bt_cal;
        public UILabel lb_foodvalue;
        public UIButton bt_info;
        public UISprite bt_foodInfo;
        public UILabel lb_food_name;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UILabel lb_packageSize;
        public UILabel lb_herolb;
        public UIToggle to_xian;
        public UIToggle to_fo;
        public UIToggle to_yao;
        public UIToggle to_ming;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            bt_go = FindChild<UIButton>("bt_go");
            bt_add = FindChild<UIButton>("bt_add");
            bt_cal = FindChild<UIButton>("bt_cal");
            lb_foodvalue = FindChild<UILabel>("lb_foodvalue");
            bt_info = FindChild<UIButton>("bt_info");
            bt_foodInfo = FindChild<UISprite>("bt_foodInfo");
            lb_food_name = FindChild<UILabel>("lb_food_name");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            lb_packageSize = FindChild<UILabel>("lb_packageSize");
            lb_herolb = FindChild<UILabel>("lb_herolb");
            to_xian = FindChild<UIToggle>("to_xian");
            to_fo = FindChild<UIToggle>("to_fo");
            to_yao = FindChild<UIToggle>("to_yao");
            to_ming = FindChild<UIToggle>("to_ming");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIGoToExplore Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIGoToExplore>();
            ui.ShowWindow();
            return ui;
        }
    }
}