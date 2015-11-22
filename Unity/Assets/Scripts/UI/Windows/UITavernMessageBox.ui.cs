using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//Auto gen code can't rewrite .
//send to email:54249636@qq.com for help
namespace Assets.Scripts.UI.Windows
{
    [UIWindow("UITavernMessageBox")]
    partial class UITavernMessageBox : UIAutoGenWindow
    {


        public UILabel lb_job;
        public UILabel lb_Message;
        public UIButton bt_ok;
        public UIButton bt_cancel;
        public UILabel lb_Title;
        public UILabel lb_cost;
        public UIWidget c_close;




        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_job = FindChild<UILabel>("lb_job");
            lb_Message = FindChild<UILabel>("lb_Message");
            bt_ok = FindChild<UIButton>("bt_ok");
            bt_cancel = FindChild<UIButton>("bt_cancel");
            lb_Title = FindChild<UILabel>("lb_Title");
            lb_cost = FindChild<UILabel>("lb_cost");
            c_close = FindChild<UIWidget>("c_close");


        }
        public static UITavernMessageBox Show()
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UITavernMessageBox>();
            ui.ShowWindow();
            return ui;
        }
    }
}