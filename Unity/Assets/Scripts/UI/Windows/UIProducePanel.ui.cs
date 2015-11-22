using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIProducePanel")]
    partial class UIProducePanel : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton bt_item;
            public UILabel lb_name;
            public UIButton bt_add;
            public UIButton bt_cal;
            public UILabel Label;

            public override void InitTemplate()
            {
                bt_item = FindChild<UIButton>("bt_item");
                lb_name = FindChild<UILabel>("lb_name");
                bt_add = FindChild<UIButton>("bt_add");
                bt_cal = FindChild<UIButton>("bt_cal");
                Label = FindChild<UILabel>("Label");

            }
        }


        public UISprite BottomInfo;
        public UILabel lb_bt_title;
        public UILabel lb_bt_info;
        public UIButton bt_close;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UILabel lb_worker;
        public UILabel lb_reward_time;
        public UILabel lb_reward_list;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            BottomInfo = FindChild<UISprite>("BottomInfo");
            lb_bt_title = FindChild<UILabel>("lb_bt_title");
            lb_bt_info = FindChild<UILabel>("lb_bt_info");
            bt_close = FindChild<UIButton>("bt_close");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            lb_worker = FindChild<UILabel>("lb_worker");
            lb_reward_time = FindChild<UILabel>("lb_reward_time");
            lb_reward_list = FindChild<UILabel>("lb_reward_list");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIProducePanel Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIProducePanel>();
            ui.ShowWindow();
            return ui;
        }
    }
}