using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIExplore")]
    partial class UIExplore : UIAutoGenWindow
    {
        public class BagGridTableTemplate : TableItemTemplate
        {
            public BagGridTableTemplate(){}
            public UILabel lb_name;
            public UILabel lv_num;

            public override void InitTemplate()
            {
                lb_name = FindChild<UILabel>("lb_name");
                lv_num = FindChild<UILabel>("lv_num");

            }
        }


        public UILabel lb_title;
        public UIButton bt_close;
        public UILabel lb_package;
        public UISprite bt_package;
        public UILabel lb_food;
        public UILabel lb_explorevalue;
        public UILabel lb_vector;
        public UISprite s_bagRoot;
        public UIGrid BagGrid;
        public UIButton bt_hide;


        public UITableManager<AutoGenTableItem<BagGridTableTemplate, BagGridTableModel>> BagGridTableManager = new UITableManager<AutoGenTableItem<BagGridTableTemplate, BagGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_title = FindChild<UILabel>("lb_title");
            bt_close = FindChild<UIButton>("bt_close");
            lb_package = FindChild<UILabel>("lb_package");
            bt_package = FindChild<UISprite>("bt_package");
            lb_food = FindChild<UILabel>("lb_food");
            lb_explorevalue = FindChild<UILabel>("lb_explorevalue");
            lb_vector = FindChild<UILabel>("lb_vector");
            s_bagRoot = FindChild<UISprite>("s_bagRoot");
            BagGrid = FindChild<UIGrid>("BagGrid");
            bt_hide = FindChild<UIButton>("bt_hide");

            BagGridTableManager.InitFromGrid(BagGrid);

        }
        public static UIExplore Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIExplore>();
            ui.ShowWindow();
            return ui;
        }
    }
}