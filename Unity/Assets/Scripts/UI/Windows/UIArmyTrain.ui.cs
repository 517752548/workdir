using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIArmyTrain")]
    partial class UIArmyTrain : UIAutoGenWindow
    {
        public class ItemGridTableTemplate : TableItemTemplate
        {
            public ItemGridTableTemplate(){}
            public UIButton bt_item;
            public UILabel lb_armyName;
            public UILabel lb_skill;
            public UILabel lb_hp;
            public UILabel lb_speed;
            public UILabel lb_damage;

            public override void InitTemplate()
            {
                bt_item = FindChild<UIButton>("bt_item");
                lb_armyName = FindChild<UILabel>("lb_armyName");
                lb_skill = FindChild<UILabel>("lb_skill");
                lb_hp = FindChild<UILabel>("lb_hp");
                lb_speed = FindChild<UILabel>("lb_speed");
                lb_damage = FindChild<UILabel>("lb_damage");

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