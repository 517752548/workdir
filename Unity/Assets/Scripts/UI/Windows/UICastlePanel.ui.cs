using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UICastlePanel")]
    partial class UICastlePanel : UIAutoGenWindow
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


        public UIButton bt_contruct;
        public UIButton bt_make;
        public UIButton bt_train;
        public UIButton bt_produce;
        public UIButton bt_battle;
        public UIButton bt_bar;
        public UISprite char1;
        public UISprite char2;
        public UISprite char3;
        public UISprite char4;
        public UIPanel Title;
        public UILabel lb_title;
        public UITexture PlayerIcon;
        public UIButton bt_market;
        public UIButton bt_Coin;
        public UILabel lb_gold;
        public UIButton bt_gold;
        public UIButton bt_Package;
        public UISprite s_bagRoot;
        public UIGrid BagGrid;
        public UIButton bt_hide;
        public FingherEvent fingerEvent;


        public UITableManager<AutoGenTableItem<BagGridTableTemplate, BagGridTableModel>> BagGridTableManager = new UITableManager<AutoGenTableItem<BagGridTableTemplate, BagGridTableModel>>();


        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_contruct = FindChild<UIButton>("bt_contruct");
            bt_make = FindChild<UIButton>("bt_make");
            bt_train = FindChild<UIButton>("bt_train");
            bt_produce = FindChild<UIButton>("bt_produce");
            bt_battle = FindChild<UIButton>("bt_battle");
            bt_bar = FindChild<UIButton>("bt_bar");
            char1 = FindChild<UISprite>("char1");
            char2 = FindChild<UISprite>("char2");
            char3 = FindChild<UISprite>("char3");
            char4 = FindChild<UISprite>("char4");
            Title = FindChild<UIPanel>("Title");
            lb_title = FindChild<UILabel>("lb_title");
            PlayerIcon = FindChild<UITexture>("PlayerIcon");
            bt_market = FindChild<UIButton>("bt_market");
            bt_Coin = FindChild<UIButton>("bt_Coin");
            lb_gold = FindChild<UILabel>("lb_gold");
            bt_gold = FindChild<UIButton>("bt_gold");
            bt_Package = FindChild<UIButton>("bt_Package");
            s_bagRoot = FindChild<UISprite>("s_bagRoot");
            BagGrid = FindChild<UIGrid>("BagGrid");
            bt_hide = FindChild<UIButton>("bt_hide");
            fingerEvent = FindChild<FingherEvent>("fingerEvent");

            BagGridTableManager.InitFromGrid(BagGrid);

        }
        public static UICastlePanel Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UICastlePanel>();
            ui.ShowWindow();
            return ui;
        }
    }
}