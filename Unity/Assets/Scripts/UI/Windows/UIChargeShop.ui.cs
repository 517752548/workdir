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
            public UILabel lb_cost;
            public UISprite s_coin;
            public UIButton bt_info;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");
                lb_cost = FindChild<UILabel>("lb_cost");
                s_coin = FindChild<UISprite>("s_coin");
                bt_info = FindChild<UIButton>("bt_info");

            }
        }


        public UIGrid ItemGrid;
        public UIButton bt_close;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_close = FindChild<UIButton>("bt_close");

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