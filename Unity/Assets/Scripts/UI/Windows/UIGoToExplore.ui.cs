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


        public UIButton bt_left;
        public UIButton bt_right;
        public UILabel lb_package_size_lb;
        public UILabel lb_package_size;
        public UILabel lb_army_size_lb;
        public UILabel lb_army_size;
        public UIButton bt_go;
        public UIButton bt_achievement;
        public UIButton bt_rank;
        public UIGrid ItemGrid;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_left = FindChild<UIButton>("bt_left");
            bt_right = FindChild<UIButton>("bt_right");
            lb_package_size_lb = FindChild<UILabel>("lb_package_size_lb");
            lb_package_size = FindChild<UILabel>("lb_package_size");
            lb_army_size_lb = FindChild<UILabel>("lb_army_size_lb");
            lb_army_size = FindChild<UILabel>("lb_army_size");
            bt_go = FindChild<UIButton>("bt_go");
            bt_achievement = FindChild<UIButton>("bt_achievement");
            bt_rank = FindChild<UIButton>("bt_rank");
            ItemGrid = FindChild<UIGrid>("ItemGrid");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }       
    }
}