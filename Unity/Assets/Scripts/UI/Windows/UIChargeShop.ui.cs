using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIChargeShop")]
    partial class UIChargeShop : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton Bt_itemName;
            public UILabel lb_s_cost;
            public UIButton bt_add;
            public UIButton bt_sub;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");
                lb_s_cost = FindChild<UILabel>("lb_s_cost");
                bt_add = FindChild<UIButton>("bt_add");
                bt_sub = FindChild<UIButton>("bt_sub");

            }
        }


        public UIGrid ItemGrid;
        public UIButton bt_close;
        public UIButton bt_Ok;
        public UILabel lb_cost;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_close = FindChild<UIButton>("bt_close");
            bt_Ok = FindChild<UIButton>("bt_Ok");
            lb_cost = FindChild<UILabel>("lb_cost");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIChargeShop Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIChargeShop>();
            ui.ShowWindow();
            return ui;
        }
    }
}