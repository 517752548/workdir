using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UITavern")]
    partial class UITavern : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UITexture icon;
            public UILabel lb_name;
            public UISprite s_job;
            public UIGrid StartGrid;
            public UILabel lb_skill;
            public UILabel lb_speed;
            public UILabel lb_hp;
            public UILabel lb_attack;
            public UISprite s_lock;
            public UIButton Bt_Emp;

            public override void InitTemplate()
            {
                icon = FindChild<UITexture>("icon");
                lb_name = FindChild<UILabel>("lb_name");
                s_job = FindChild<UISprite>("s_job");
                StartGrid = FindChild<UIGrid>("StartGrid");
                lb_skill = FindChild<UILabel>("lb_skill");
                lb_speed = FindChild<UILabel>("lb_speed");
                lb_hp = FindChild<UILabel>("lb_hp");
                lb_attack = FindChild<UILabel>("lb_attack");
                s_lock = FindChild<UISprite>("s_lock");
                Bt_Emp = FindChild<UIButton>("Bt_Emp");

            }
        }


        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIButton bt_close;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_close = FindChild<UIButton>("bt_close");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UITavern Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UITavern>();
            ui.ShowWindow();
            return ui;
        }
    }
}