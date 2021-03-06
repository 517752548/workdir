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
				return;
                //if (Cancel != null)
                //    Cancel();
                //HideWindow();
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

        public static void ShowMessage(string title, string message, Action clickOK, Action clickCancel )
        {
			ShowMessage (title,
				message,
				LanguageManager.Singleton ["Message_OK"],
				LanguageManager.Singleton ["Message_Cancel"],
				clickOK,
				clickCancel);
        }

		public static void ShowMessage(string title,
			string message,
			string okTile,
			string cancelTitle,
			Action clickOK,
			Action clickCancel, 
			bool onlyOk = false)
		{

			var ui = UIManager.Singleton.CreateOrGetWindow<UIMessageBox> ();
			ui.ShowWindow ();
			ui.OK = clickOK;
			ui.Cancel = clickCancel;
			ui.lb_Message.text = message;
			ui.lb_Title.text = title;
			ui.bt_ok.Text (okTile);
			ui.bt_cancel.Text (cancelTitle);
			var p = ui.bt_ok.transform.localPosition;
			ui.bt_ok.transform.localPosition = new UnityEngine.Vector3 (onlyOk ? 0 : -120.34f, p.y, p.z);
		   	ui.bt_cancel.ActiveSelfObject (!onlyOk);

		}


    }
}