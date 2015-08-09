using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIArmyHouse")]
    partial class UIArmyHouse : UIAutoGenWindow
    {


        public UISprite character1;
        public UIButton bt_close;
        public UILabel lb_title;
        public UIButton Bt_ArmyLvlUp;
        public UIButton bt_armyLvlUpinfo;
        public UIButton Bt_HouseLvlUp;
        public UIButton bt_HouseLvlUpinfo;




        public override void InitTemplate()
        {
            base.InitTemplate();
            character1 = FindChild<UISprite>("character1");
            bt_close = FindChild<UIButton>("bt_close");
            lb_title = FindChild<UILabel>("lb_title");
            Bt_ArmyLvlUp = FindChild<UIButton>("Bt_ArmyLvlUp");
            bt_armyLvlUpinfo = FindChild<UIButton>("bt_armyLvlUpinfo");
            Bt_HouseLvlUp = FindChild<UIButton>("Bt_HouseLvlUp");
            bt_HouseLvlUpinfo = FindChild<UIButton>("bt_HouseLvlUpinfo");


        }
        public static UIArmyHouse Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIArmyHouse>();
            ui.ShowWindow();
            return ui;
        }
    }
}