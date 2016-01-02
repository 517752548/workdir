using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIShop")]
    partial class UIShop : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton Bt_itemName;
            public UILabel lb_cost;
            public UIButton bt_info;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");
                lb_cost = FindChild<UILabel>("lb_cost");
                bt_info = FindChild<UIButton>("bt_info");

            }
        }
        public class ItemGridCoinTableTemplate : TableItemTemplate
        {
            public ItemGridCoinTableTemplate(){}
            public UIButton Bt_itemName;
            public UILabel lb_cost;
            public UIButton bt_info;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");
                lb_cost = FindChild<UILabel>("lb_cost");
                bt_info = FindChild<UIButton>("bt_info");

            }
        }


        public UIButton bt_close;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIToggle t_gold;
        public UIToggle t_coin;
        public UIPanel PackageViewCoin;
        public UIGrid ItemGridCoin;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();
        public UITableManager<AutoGenTableItem<ItemGridCoinTableTemplate, ItemGridCoinTableModel>> ItemGridCoinTableManager = new UITableManager<AutoGenTableItem<ItemGridCoinTableTemplate, ItemGridCoinTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            t_gold = FindChild<UIToggle>("t_gold");
            t_coin = FindChild<UIToggle>("t_coin");
            PackageViewCoin = FindChild<UIPanel>("PackageViewCoin");
            ItemGridCoin = FindChild<UIGrid>("ItemGridCoin");

            ItemGridTableManager.InitFromGrid(ItemGrid);
            ItemGridCoinTableManager.InitFromGrid(ItemGridCoin);

        }
        public static UIShop Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIShop>();
            ui.ShowWindow();
            return ui;
        }
    }
}