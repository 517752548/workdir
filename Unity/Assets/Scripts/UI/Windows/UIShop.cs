using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

namespace Assets.Scripts.UI.Windows
{
    partial class UIShop
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                this.Template.bt_item.OnMouseClick((s, e) => {
                    if (OnBtClick == null) return;
                    OnBtClick(this.Config);
                });
            }

            public Action<ShopConfig> OnBtClick;

            private ExcelConfig.ShopConfig _Config { get; set; }
            public ExcelConfig.ShopConfig Config
            {
                get
                {
                    return _Config;
                }
                set {
                    _Config = value;
                    
                    var itemRequire = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(_Config.RequireItem);
                    var itemReward = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(_Config.Item);
                    this.Template.lb_itemName.text = itemReward.Name;
                    this.Template.lb_NeedItems.text = 
                        LanguageManager.Singleton[
                        DataManagers.PlayerItemManager.Singleton.GetItemCount(itemRequire.ID)>= Config.RequireNum? 
                        "UIShop_Require_Item_Not_Enough":"UIShop_Require_Item_Enough"]  
                        +string.Format(LanguageManager.Singleton["UIShop_Require_Item"], itemRequire.Name, _Config.RequireNum);
                }
            }

            public void SetDrag(bool flag)
            {
                var dragComp = this.Item.Root.GetComponent<UIDragScrollView>();
                if (dragComp == null) return;
                dragComp.enabled = flag;
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            this.bt_left.OnMouseClick((s, e) => {
                this.HideWindow();
                var ui = UIManager.Singleton.CreateOrShowWindow<UICastlePanel>();
                ui.ShowWindow();
            });
            this.bt_right.ActiveSelfObject(false);
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            var items = ExcelToJSONConfigManager.Current.GetConfigs<ShopConfig>();
            int index = 0;
            this.ItemGridTableManager.Count = items.Length;
            foreach(var i in this.ItemGridTableManager)
            {
                i.Model.Config = items[index];
                i.Model.OnBtClick = BuyItem;
                i.Model.SetDrag(items.Length >= 10);
                index++;
            } 
        }

        public void BuyItem(ShopConfig config)
        {
            //doBy
            DataManagers.PlayerItemManager.Singleton.BuyItem(config);
            OnUpdateUIData();
        }
        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}