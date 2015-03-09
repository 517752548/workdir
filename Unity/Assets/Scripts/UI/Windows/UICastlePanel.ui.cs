using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UICastlePanel")]
    partial class UICastlePanel : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UILabel lb_name;
            public UILabel lb_count;

            public override void InitTemplate()
            {
                lb_name = FindChild<UILabel>("lb_name");
                lb_count = FindChild<UILabel>("lb_count");

            }
        }


        public UIButton bt_left;
        public UIButton bt_right;
        public UIButton bt_produce;
        public UIButton bt_contruct;
        public UIButton bt_market;
        public UIButton bt_make;
        public UIButton bt_train;
        public UIButton bt_pay;
        public UIGrid ItemGrid;


        public UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>> ItemGridTableManager = new UITableManager<AutoGenTableItem<ItemGridTableTemplate, ItemGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_left = FindChild<UIButton>("bt_left");
            bt_right = FindChild<UIButton>("bt_right");
            bt_produce = FindChild<UIButton>("bt_produce");
            bt_contruct = FindChild<UIButton>("bt_contruct");
            bt_market = FindChild<UIButton>("bt_market");
            bt_make = FindChild<UIButton>("bt_make");
            bt_train = FindChild<UIButton>("bt_train");
            bt_pay = FindChild<UIButton>("bt_pay");
            ItemGrid = FindChild<UIGrid>("ItemGrid");

            ItemGridTableManager.InitFromGrid(ItemGrid);

        }       
    }
}