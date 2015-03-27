using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIBuilding")]
    partial class UIBuilding : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton bt_build;
            public UILabel lb_buildName;
            public UILabel lb_NeedItems;
            public UILabel lb_build;

            public override void InitTemplate()
            {
                bt_build = FindChild<UIButton>("bt_build");
                lb_buildName = FindChild<UILabel>("lb_buildName");
                lb_NeedItems = FindChild<UILabel>("lb_NeedItems");
                lb_build = FindChild<UILabel>("lb_build");

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