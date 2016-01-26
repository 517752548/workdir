using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIBattleResult")]
    partial class UIBattleResult : UIAutoGenWindow
    {
        public class PackageGridTableTemplate : TableItemTemplate
        {
            public PackageGridTableTemplate(){}
            public UILabel lb_name;
            public UILabel lb_vlaue;

            public override void InitTemplate()
            {
                lb_name = FindChild<UILabel>("lb_name");
                lb_vlaue = FindChild<UILabel>("lb_vlaue");

            }
        }
        public class DropGridTableTemplate : TableItemTemplate
        {
            public DropGridTableTemplate(){}
            public UILabel lb_name;
            public UILabel lb_vlaue;

            public override void InitTemplate()
            {
                lb_name = FindChild<UILabel>("lb_name");
                lb_vlaue = FindChild<UILabel>("lb_vlaue");

            }
        }


        public UILabel lb_packageSize;
        public UIButton bt_close;
        public UIButton bt_collectall;
        public UIGrid PackageGrid;
        public UIGrid DropGrid;


        public UITableManager<AutoGenTableItem<PackageGridTableTemplate, PackageGridTableModel>> PackageGridTableManager = new UITableManager<AutoGenTableItem<PackageGridTableTemplate, PackageGridTableModel>>();
        public UITableManager<AutoGenTableItem<DropGridTableTemplate, DropGridTableModel>> DropGridTableManager = new UITableManager<AutoGenTableItem<DropGridTableTemplate, DropGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_packageSize = FindChild<UILabel>("lb_packageSize");
            bt_close = FindChild<UIButton>("bt_close");
            bt_collectall = FindChild<UIButton>("bt_collectall");
            PackageGrid = FindChild<UIGrid>("PackageGrid");
            DropGrid = FindChild<UIGrid>("DropGrid");

            PackageGridTableManager.InitFromGrid(PackageGrid);
            DropGridTableManager.InitFromGrid(DropGrid);

        }
        public static UIBattleResult Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIBattleResult>();
            ui.ShowWindow();
            return ui;
        }
    }
}