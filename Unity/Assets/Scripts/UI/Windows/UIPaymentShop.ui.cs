using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIPaymentShop")]
    partial class UIPaymentShop : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UILabel lb_cost;

            public override void InitTemplate()
            {
                lb_cost = FindChild<UILabel>("lb_cost");

            }
        }


        public UIButton bt_close;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UILabel lb_coinValue;
        public UIButton bt_Payment;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            lb_coinValue = FindChild<UILabel>("lb_coinValue");
            bt_Payment = FindChild<UIButton>("bt_Payment");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIPaymentShop Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIPaymentShop>();
            ui.ShowWindow();
            return ui;
        }
    }
}