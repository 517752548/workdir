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
        public class ProduceListGridTableTemplate : TableItemTemplate
        {
            public ProduceListGridTableTemplate(){}
            public UILabel lb_reward_list;

            public override void InitTemplate()
            {
                lb_reward_list = FindChild<UILabel>("lb_reward_list");

            }
        }


        public UIButton bt_close;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UILabel lb_worker;
        public UILabel lb_reward_time;
        public UIPanel ListViewS;
        public UIGrid ProduceListGrid;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();
        public UITableManager<AutoGenTableItem<ProduceListGridTableTemplate, ProduceListGridTableModel>> ProduceListGridTableManager = new UITableManager<AutoGenTableItem<ProduceListGridTableTemplate, ProduceListGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            lb_worker = FindChild<UILabel>("lb_worker");
            lb_reward_time = FindChild<UILabel>("lb_reward_time");
            ListViewS = FindChild<UIPanel>("ListViewS");
            ProduceListGrid = FindChild<UIGrid>("ProduceListGrid");

            ItemGridTableManager.InitFromGrid(ItemGrid);
            ProduceListGridTableManager.InitFromGrid(ProduceListGrid);

        }
        public static UIProducePanel Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIProducePanel>();
            ui.ShowWindow();
            return ui;
        }
    }
}