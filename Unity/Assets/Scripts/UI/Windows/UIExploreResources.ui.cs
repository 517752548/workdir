using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIExploreResources")]
    partial class UIExploreResources : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton bt_item;
            public UILabel lb_name;
            public UIButton bt_add;
            public UIButton bt_cal;
            public UILabel Label;
            public UIButton bt_info;

            public override void InitTemplate()
            {
                bt_item = FindChild<UIButton>("bt_item");
                lb_name = FindChild<UILabel>("lb_name");
                bt_add = FindChild<UIButton>("bt_add");
                bt_cal = FindChild<UIButton>("bt_cal");
                Label = FindChild<UILabel>("Label");
                bt_info = FindChild<UIButton>("bt_info");

            }
        }


        public UILabel lb_title;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIButton bt_close;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_title = FindChild<UILabel>("lb_title");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_close = FindChild<UIButton>("bt_close");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIExploreResources Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIExploreResources>();
            ui.ShowWindow();
            return ui;
        }
    }
}