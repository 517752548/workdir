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
            public UISprite s_price;
            public UILabel lb_desc;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");
                s_price = FindChild<UISprite>("s_price");
                lb_desc = FindChild<UILabel>("lb_desc");

            }
        }


        public UIButton bt_close;
        public UILabel lb_address;
        public UILabel lb_qq;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIButton bt_return;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_close = FindChild<UIButton>("bt_close");
            lb_address = FindChild<UILabel>("lb_address");
            lb_qq = FindChild<UILabel>("lb_qq");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_return = FindChild<UIButton>("bt_return");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIPayment Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIPayment>();
            ui.ShowWindow();
            return ui;
        }
    }
}