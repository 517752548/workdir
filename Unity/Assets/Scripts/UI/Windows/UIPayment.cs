using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.DataManagers;
using Assets.Scripts.PersistStructs;

namespace Assets.Scripts.UI.Windows
{
    partial class UIPayment
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                Template.Bt_itemName.OnMouseClick((s, e) =>
                {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }
            public Action<ItemGridTableModel> OnClick;
            public void SetDrag(bool canDrag)
            {
                var d = this.Item.Root.GetComponent<UIDragScrollView>();
                d.enabled = canDrag;
            }

            private PaymentData _Data { get; set; }

            public PersistStructs.PaymentData Data
            {
                get { return _Data; }
                set
                {
                    _Data = value;
                    Template.s_price.spriteName = Data.SpriteName;
                    Template.lb_desc.text = Data.Des;
                }
            }
        }

        

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
            bt_return.OnMouseClick((s, e) => {
                HideWindow();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            UIControllor.Singleton.ShowOrHideMessage(false);
            var data = DataManagers.GamePlayerManager.Singleton.PaymentData;
            ItemGridTableManager.Count = data.Count;
            int index = 0;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.OnClick = OnClick;
                i.Model.Data = data[index];
                i.Model.SetDrag(ItemGridTableManager.Count >= 5);
                index ++;
            }
            lb_address.text = LanguageManager.Singleton["UI_Payment_address_Url"];
            lb_qq.text = LanguageManager.Singleton["UI_Payment_address_qq"];
        }

        private void OnClick(ItemGridTableModel obj)
        {
            last = obj;
  
            UIMessageBox.ShowMessage(LanguageManager.Singleton["UI_Payment_pay"], 
                string.Format(LanguageManager.Singleton["UI_Payment_paymessage"],obj.Data.Des), OnBuy, null);
        }

        private ItemGridTableModel last;

        private void OnBuy()
        {
            GamePlayerManager.Singleton.DoPayment(last.Data);
            UITipDrawer.Singleton.DrawNotify("Buy OK");
            
        }
        public override void OnHide()
        {
            base.OnHide();
            UIControllor.Singleton.ShowOrHideMessage(true);
        }
    }
}