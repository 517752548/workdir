using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIMapList")]
    partial class UIMapList : UIAutoGenWindow
    {
        public class MapGridTableTemplate : TableItemTemplate
        {
            public MapGridTableTemplate(){}
            public UILabel lb_name;

            public override void InitTemplate()
            {
                lb_name = FindChild<UILabel>("lb_name");

            }
        }


        public UISprite s_bagRoot;
        public UIGrid MapGrid;
        public UIButton bt_hide;


        public UITableManager<AutoGenTableItem<MapGridTableTemplate, MapGridTableModel>> MapGridTableManager = new UITableManager<AutoGenTableItem<MapGridTableTemplate, MapGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            s_bagRoot = FindChild<UISprite>("s_bagRoot");
            MapGrid = FindChild<UIGrid>("MapGrid");
            bt_hide = FindChild<UIButton>("bt_hide");

            MapGridTableManager.InitFromGrid(MapGrid);

        }
        public static UIMapList Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIMapList>();
            ui.ShowWindow();
            return ui;
        }
    }
}