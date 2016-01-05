using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIStructureBuilding")]
    partial class UIStructureBuilding : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UILabel lb_cost;
            public UISprite s_bg;
            public UITexture IconBuild;
            public UISprite LeveCan;
            public UIButton bt_info;
            public UILabel lb_lvl;
            public UILabel lb_name;

            public override void InitTemplate()
            {
                lb_cost = FindChild<UILabel>("lb_cost");
                s_bg = FindChild<UISprite>("s_bg");
                IconBuild = FindChild<UITexture>("IconBuild");
                LeveCan = FindChild<UISprite>("LeveCan");
                bt_info = FindChild<UIButton>("bt_info");
                lb_lvl = FindChild<UILabel>("lb_lvl");
                lb_name = FindChild<UILabel>("lb_name");

            }
        }


        public UIButton bt_close;
        public UIPanel PackageView;
        public UIGrid ItemGrid;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIStructureBuilding Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIStructureBuilding>();
            ui.ShowWindow();
            return ui;
        }
    }
}