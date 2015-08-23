using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIBag")]
    partial class UIBag : UIAutoGenWindow
    {
        public class GridTableTemplate : TableItemTemplate
        {
            public GridTableTemplate(){}
            public UILabel lb_name;
            public UILabel lv_num;

            public override void InitTemplate()
            {
                lb_name = FindChild<UILabel>("lb_name");
                lv_num = FindChild<UILabel>("lv_num");

            }
        }


        public UIGrid Grid;
        public UIButton bt_close;


        public UITableManager<AutoGenTableItem<GridTableTemplate, GridTableModel>> GridTableManager = new UITableManager<AutoGenTableItem<GridTableTemplate, GridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            Grid = FindChild<UIGrid>("Grid");
            bt_close = FindChild<UIButton>("bt_close");

            GridTableManager.InitFromGrid(Grid);

        }
        public static UIBag Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIBag>();
            ui.ShowWindow();
            return ui;
        }
    }
}