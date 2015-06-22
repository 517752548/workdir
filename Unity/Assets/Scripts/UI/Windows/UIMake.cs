using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

namespace Assets.Scripts.UI.Windows
{
    partial class UIMake
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                this.Template.bt_item.OnMouseClick((s, e) => 
                {
                    if (OnMake == null) return;
                    OnMake(this.Config);
                });
            }

            private ItemConfig _config;
            public ItemConfig Config
            {
                get
                {
                    return _config;
                }
                set
                {
                    _config = value;
                    var needs = DataManagers.PlayerItemManager.SplitFormatItemData(Config.RequireItems);
                    var rewards = DataManagers.PlayerItemManager.SplitFormatItemData(Config.RewardItems);
                    var needItems = needs.Select(t => new ItemData
                    {
                        Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(t[0]),
                        Num = t[1]
                    }).ToList();
                    var rewardItems = rewards.Select(t => new ItemData
                    {
                        Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(t[0]),
                        Num = t[1]
                    }).ToList();

                    if(rewardItems.Count>0)
                    {
                        this.Template.lb_itemName.text = 
                            string.Format("{0}*{1}",rewardItems[0].Config.Name, rewardItems[0].Num);
                    }
                    if(needItems.Count>0)
                    {
                        var sb = new StringBuilder();
                        foreach(var i in needItems)
                        {
                            sb.Append(string.Format("{0} *{1} ",i.Config.Name,i.Num));
                        }
                        this.Template.lb_NeedItems.text = sb.ToString();
                    }

                }
            }

            public Action<ItemConfig> OnMake;

            internal void SetDrag(bool p)
            {
                var drag = this.Item.Root.GetComponent<UIDragScrollView>();
                if (drag != null)
                    drag.enabled = p;
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

        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            var makes = ExcelToJSONConfigManager.Current.GetConfigs<ItemConfig>();
            int index = 0;
            ItemGridTableManager.Count = makes.Length;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.Config = makes[index];
                i.Model.OnMake = OnMake;
                i.Model.SetDrag(makes.Length > 10);
                index++;
            }
        }

        private void OnMake(ExcelConfig.ItemConfig config)
        {

            DataManagers.PlayerItemManager.Singleton.MakeItem(config);
        }

        public class ItemData
        {
            public ItemConfig Config { set; get; }

            public int Num { set; get; }
        }
    }
}