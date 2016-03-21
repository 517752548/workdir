using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UISetting")]
    partial class UISetting : UIAutoGenWindow
    {


        public UILabel lb_idName;
        public UILabel lb_version;
        public UIButton bt_close;
        public UIButton Bt_shard;
        public UIButton Bt_Achievement;
        public UIButton Bt_Language;
        public UIButton Bt_Reset;
        public UIToggle Bt_Music;
        public UIToggle Bt_MusicEffect;
        public UIButton Bt_GameSkill;




        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_idName = FindChild<UILabel>("lb_idName");
            lb_version = FindChild<UILabel>("lb_version");
            bt_close = FindChild<UIButton>("bt_close");
            Bt_shard = FindChild<UIButton>("Bt_shard");
            Bt_Achievement = FindChild<UIButton>("Bt_Achievement");
            Bt_Language = FindChild<UIButton>("Bt_Language");
            Bt_Reset = FindChild<UIButton>("Bt_Reset");
            Bt_Music = FindChild<UIToggle>("Bt_Music");
            Bt_MusicEffect = FindChild<UIToggle>("Bt_MusicEffect");
            Bt_GameSkill = FindChild<UIButton>("Bt_GameSkill");


        }
        public static UISetting Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UISetting>();
            ui.ShowWindow();
            return ui;
        }
    }
}