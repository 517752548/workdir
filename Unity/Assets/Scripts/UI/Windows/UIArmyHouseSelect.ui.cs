using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIArmyHouseSelect")]
    partial class UIArmyHouseSelect : UIAutoGenWindow
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
            public UIButton Bt_selected;

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
                Bt_selected = FindChild<UIButton>("Bt_selected");

            }
        }


        public UISprite character1;
        public UISprite character2;
        public UISprite character3;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIButton bt_close;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            character1 = FindChild<UISprite>("character1");
            character2 = FindChild<UISprite>("character2");
            character3 = FindChild<UISprite>("character3");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_close = FindChild<UIButton>("bt_close");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIArmyHouseSelect Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIArmyHouseSelect>();
            ui.ShowWindow();
            return ui;
        }
    }
}