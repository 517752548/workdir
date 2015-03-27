using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            public UIButton bt_item;
            public UILabel lb_itemName;
            public UILabel lb_NeedItems;

            public override void InitTemplate()
            {
                bt_item = FindChild<UIButton>("bt_item");
                lb_itemName = FindChild<UILabel>("lb_itemName");
                lb_NeedItems = FindChild<UILabel>("lb_NeedItems");

            }
        }


        public UIButton bt_left;
        public UIButton bt_right;
        public UIGrid ItemGrid;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_left = FindChild<UIButton>("bt_left");
            bt_right = FindChild<UIButton>("bt_right");
            ItemGrid = FindChild<UIGrid>("ItemGrid");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }       
    }
}