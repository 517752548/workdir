using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIAchievement")]
    partial class UIAchievement : UIAutoGenWindow
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


        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIButton bt_return;
        public UILabel lb_point;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_return = FindChild<UIButton>("bt_return");
            lb_point = FindChild<UILabel>("lb_point");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIAchievement Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIAchievement>();
            ui.ShowWindow();
            return ui;
        }
    }
}