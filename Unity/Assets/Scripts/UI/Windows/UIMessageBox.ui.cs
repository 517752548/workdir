using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UIMessageBox")]
    partial class UIMessageBox : UIAutoGenWindow
    {


        public UIWidget c_close;
        public UILabel lb_Message;
        public UIButton bt_ok;
        public UIButton bt_cancel;
        public UILabel lb_Title;
        public UISprite Mask;




        public override void InitTemplate()
        {
            base.InitTemplate();
            c_close = FindChild<UIWidget>("c_close");
            lb_Message = FindChild<UILabel>("lb_Message");
            bt_ok = FindChild<UIButton>("bt_ok");
            bt_cancel = FindChild<UIButton>("bt_cancel");
            lb_Title = FindChild<UILabel>("lb_Title");
            Mask = FindChild<UISprite>("Mask");


        }
        public static UIMessageBox Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIMessageBox>();
            ui.ShowWindow();
            return ui;
        }
    }
}