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
            public UILabel Label;
            public UILabel lb_cal;
            public UILabel lb_add;
            public UILabel lb_num;

            public override void InitTemplate()
            {
                Label = FindChild<UILabel>("Label");
                lb_cal = FindChild<UILabel>("lb_cal");
                lb_add = FindChild<UILabel>("lb_add");
                lb_num = FindChild<UILabel>("lb_num");

            }
        }
        public class RewardItemGridTableTemplate : TableItemTemplate
        {
            public RewardItemGridTableTemplate(){}
            public UILabel lb_name;
            public UILabel lb_count;

            public override void InitTemplate()
            {
                lb_name = FindChild<UILabel>("lb_name");
                lb_count = FindChild<UILabel>("lb_count");

            }
        }


        public UIButton bt_left;
        public UIButton bt_right;
        public UIButton bt_produce;
        public UILabel lb_time;
        public UILabel lb_name_lb;
        public UILabel lb_people;
        public UIGrid ItemGrid;
        public UIGrid RewardItemGrid;
        public UILabel lb_timeLimit;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();
        public UITableManager<AutoGenTableItem<RewardItemGridTableTemplate, RewardItemGridTableModel>> RewardItemGridTableManager = new UITableManager<AutoGenTableItem<RewardItemGridTableTemplate, RewardItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_left = FindChild<UIButton>("bt_left");
            bt_right = FindChild<UIButton>("bt_right");
            bt_produce = FindChild<UIButton>("bt_produce");
            lb_time = FindChild<UILabel>("lb_time");
            lb_name_lb = FindChild<UILabel>("lb_name_lb");
            lb_people = FindChild<UILabel>("lb_people");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            RewardItemGrid = FindChild<UIGrid>("RewardItemGrid");
            lb_timeLimit = FindChild<UILabel>("lb_timeLimit");

            ItemGridTableManager.InitFromGrid(ItemGrid);
            RewardItemGridTableManager.InitFromGrid(RewardItemGrid);

        }       
    }
}