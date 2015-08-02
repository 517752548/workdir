using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIPayment")]
    partial class UIPayment : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton Bt_itemName;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");

            }
        }


        public UIButton bt_close;
        public UILabel lb_address;
        public UILabel lb_qq;
        public UILabel lb_title;
        public UIPanel PackageView;
        public UIGrid ItemGrid;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            lb_address = FindChild<UILabel>("lb_address");
            lb_qq = FindChild<UILabel>("lb_qq");
            lb_title = FindChild<UILabel>("lb_title");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }       
    }
}