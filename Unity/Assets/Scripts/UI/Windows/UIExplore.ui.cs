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


        public UILabel lb_title;
        public UIButton bt_close;




        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_title = FindChild<UILabel>("lb_title");
            bt_close = FindChild<UIButton>("bt_close");


        }
        public static UIExplore Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIExplore>();
            ui.ShowWindow();
            return ui;
        }
    }
}