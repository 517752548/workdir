using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;

namespace Assets.Scripts.UI.Windows
{
    partial class UIPayment
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                Template.Bt_itemName.OnMouseClick((s, e) => {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }
            public Action<ItemGridTableModel> OnClick;
        }

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            ItemGridTableManager.Count = 5;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.OnClick = OnClick;
            }
        }

        private void OnClick(ItemGridTableModel obj)
        {
            last = obj;
            UIMessageBox.ShowMessage(LanguageManager.Singleton["UI_Payment_pay"], 
                string.Format(LanguageManager.Singleton["UI_Payment_paymessage"],6,100), OnBuy, null);
        }

        private ItemGridTableModel last;

        private void OnBuy()
        {
            UITipDrawer.Singleton.DrawNotify("Buy OK");
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}