using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIGoToExplore")]
    partial class UIGoToExplore : UIAutoGenWindow
    {


        public UILabel lb_title;
        public UIButton bt_close;
        public UIButton bt_armySet;
        public UIButton bt_armySetinfo;
        public UIButton bt_heroSet;
        public UIButton bt_heroSetinfo;
        public UIButton bt_go;
        public UIButton bt_goinfo;




        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_title = FindChild<UILabel>("lb_title");
            bt_close = FindChild<UIButton>("bt_close");
            bt_armySet = FindChild<UIButton>("bt_armySet");
            bt_armySetinfo = FindChild<UIButton>("bt_armySetinfo");
            bt_heroSet = FindChild<UIButton>("bt_heroSet");
            bt_heroSetinfo = FindChild<UIButton>("bt_heroSetinfo");
            bt_go = FindChild<UIButton>("bt_go");
            bt_goinfo = FindChild<UIButton>("bt_goinfo");


        }       
    }
}