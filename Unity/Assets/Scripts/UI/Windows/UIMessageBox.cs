using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIMessageBox
    {

        public override void InitModel()
        {
            base.InitModel();
            bt_ok.OnMouseClick((s, e) => {           
                if (OK != null)
                    OK();
                HideWindow();
            });

            c_close.OnMouseClick((s, e) => {

                if (Cancel != null)
                    Cancel();
                HideWindow();
            });

            bt_cancel.OnMouseClick((s, e) =>
            {
                if (Cancel != null)
                    Cancel();
                HideWindow();
            });


            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        private Action OK;
        private Action Cancel;

        public static void ShowMessage(string ok, string message, Action clickOK, Action clickCancel)
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIMessageBox>();
            ui.ShowWindow();
            ui.OK = clickOK;
            ui.Cancel = clickCancel;
            ui.lb_Message.text = message;
            ui.lb_Title.text = ok;
        }
    }
}