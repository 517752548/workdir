using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIPlaySKill")]
    partial class UIPlaySKill : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton Bt_itemName;
            public UISprite s_completed;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");
                s_completed = FindChild<UISprite>("s_completed");

            }
        }


        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIButton bt_return;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            bt_return = FindChild<UIButton>("bt_return");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }
        public static UIPlaySKill Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIPlaySKill>();
            ui.ShowWindow();
            return ui;
        }
    }
}