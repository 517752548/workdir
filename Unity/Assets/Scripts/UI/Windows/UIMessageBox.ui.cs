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


        public UILabel lb_Message;
        public UIButton bt_ok;
        public UIWidget c_close;




        public override void InitTemplate()
        {
            base.InitTemplate();
            lb_Message = FindChild<UILabel>("lb_Message");
            bt_ok = FindChild<UIButton>("bt_ok");
            c_close = FindChild<UIWidget>("c_close");


        }       
    }
}