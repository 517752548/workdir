using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIPaymentShop
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                Template.Bt_itemName.OnMouseClick((s, e) => {
                    UIMessageBox.ShowMessage(LanguageManager.Singleton["UI_PaymenShop_bt_buy"],
                        string.Format(LanguageManager.Singleton["UI_PaymenShop_buy_item"], "Name", 100), null, null);
                });
            }
            
        }

        public override void InitModel()
        {
            base.InitModel();
            this.bt_close.OnMouseClick((s, e) => {
                this.HideWindow();
            });
            bt_Payment.OnMouseClick((s, e) => {
                UIPayment.Show();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            ItemGridTableManager.Count = 10;
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}