using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIMake")]
    partial class UIMake : UIAutoGenWindow
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
        public class TypeItemGridTableTemplate : TableItemTemplate
        {
            public TypeItemGridTableTemplate(){}
            public UIButton Bt_itemName;
            public UIButton bt_info;

            public override void InitTemplate()
            {
                Bt_itemName = FindChild<UIButton>("Bt_itemName");
                bt_info = FindChild<UIButton>("bt_info");

            }
        }


        public UISprite character1;
        public UIButton bt_close;
        public UILabel lb_title;
        public UIPanel PackageView;
        public UIGrid ItemGrid;
        public UIPanel PackageTypeView;
        public UIGrid TypeItemGrid;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();
        public UITableManager<AutoGenTableItem<TypeItemGridTableTemplate, TypeItemGridTableModel>> TypeItemGridTableManager = new UITableManager<AutoGenTableItem<TypeItemGridTableTemplate, TypeItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            character1 = FindChild<UISprite>("character1");
            bt_close = FindChild<UIButton>("bt_close");
            lb_title = FindChild<UILabel>("lb_title");
            PackageView = FindChild<UIPanel>("PackageView");
            ItemGrid = FindChild<UIGrid>("ItemGrid");
            PackageTypeView = FindChild<UIPanel>("PackageTypeView");
            TypeItemGrid = FindChild<UIGrid>("TypeItemGrid");

            ItemGridTableManager.InitFromGrid(ItemGrid);
            TypeItemGridTableManager.InitFromGrid(TypeItemGrid);

        }       
    }
}