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
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                Template.Bt_itemName.OnMouseClick((s, e) =>
                {
                    if (OnItemClick == null) return;
                    OnItemClick(this);
                });
            }

            public Action<ItemGridTableModel> OnItemClick;

            private ExcelConfig.DimondStoreConfig _Config;

            public ExcelConfig.DimondStoreConfig Config
            {
                get
                {
                    return _Config;
                }
                set
                {
                    _Config = value;
                    //_Config.
                }
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

            var shopData = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.DimondStoreConfig>();
             
            ItemGridTableManager.Count =  shopData.Length;
            int index = 0;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.Config = shopData[index];
                i.Model.OnItemClick = OnClickBuy;
                index++;
            }
        }

        public void OnClickBuy(ItemGridTableModel model)
        {
            UIMessageBox.ShowMessage(LanguageManager.Singleton["UI_PaymenShop_bt_buy"],
                      string.Format(LanguageManager.Singleton["UI_PaymenShop_buy_item"], "Name", 100), null, null);
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}