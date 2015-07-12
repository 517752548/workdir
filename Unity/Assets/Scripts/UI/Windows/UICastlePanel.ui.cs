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


        public UIButton bt_Coin;
        public UIButton bt_gold;
        public UIButton bt_contruct;
        public UIButton bt_market;
        public UIButton bt_make;
        public UIButton bt_train;
        public UIButton bt_produce;
        public UIButton bt_battle;
        public UIButton bt_Package;
        public UIButton bt_bar;
        public UILabel lb_title;
        public UISprite character1;
        public UISprite character2;
        public UISprite character3;
        public UISprite character4;




        public override void InitTemplate()
        {
            base.InitTemplate();
            bt_Coin = FindChild<UIButton>("bt_Coin");
            bt_gold = FindChild<UIButton>("bt_gold");
            bt_contruct = FindChild<UIButton>("bt_contruct");
            bt_market = FindChild<UIButton>("bt_market");
            bt_make = FindChild<UIButton>("bt_make");
            bt_train = FindChild<UIButton>("bt_train");
            bt_produce = FindChild<UIButton>("bt_produce");
            bt_battle = FindChild<UIButton>("bt_battle");
            bt_Package = FindChild<UIButton>("bt_Package");
            bt_bar = FindChild<UIButton>("bt_bar");
            lb_title = FindChild<UILabel>("lb_title");
            character1 = FindChild<UISprite>("character1");
            character2 = FindChild<UISprite>("character2");
            character3 = FindChild<UISprite>("character3");
            character4 = FindChild<UISprite>("character4");


        }       
    }
}