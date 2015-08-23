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
        #region 制作条目
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo

                Template.bt_info.OnMouseClick((s, e) => {
                    if (_MakeConfig == null) return;
                    var sb = new StringBuilder();
                    sb.Append(LanguageManager.Singleton["UIMake_Cost_Title"]);
                    if (_MakeConfig.RequireGold > 0)
                    {
                        sb.Append(string.Format(LanguageManager.Singleton["UIMake_Cost_gold"], _MakeConfig.RequireGold));
                    }
                    var costItems = UtilityTool.SplitKeyValues(_MakeConfig.RequireItems);
                    foreach (var i in costItems)
                    {
                        var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                        if (item == null) continue;
                        sb.Append(string.Format(LanguageManager.Singleton["UIMake_Cost_Item"], item.Name, i.Value));
                    }

                    sb.Append(LanguageManager.Singleton["UIMake_Reward_Title"]);
                    var rewardItem = UtilityTool.SplitKeyValues(_MakeConfig.RewardItems);
                    foreach (var i in rewardItem)
                    {
                        var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                        if (item == null) continue;
                        sb.Append(string.Format(LanguageManager.Singleton["UIMake_Cost_Item"], item.Name, i.Value));
                    }

                    UIControllor.Singleton.ShowMessage(sb.ToString(), 10);
                });
            }

            private ExcelConfig.MakeConfig _MakeConfig;

            public ExcelConfig.MakeConfig MakeConfig
            {
                get
                {
                    return _MakeConfig;
                }
                set
                {
                    _MakeConfig = value;
                    Template.Bt_itemName.Text(_MakeConfig.Name);
                    var sb = new StringBuilder();
                    var items = UtilityTool.SplitKeyValues(_MakeConfig.RequireItems);
                    if(_MakeConfig.RequireGold>0)
                    {
                        sb.Append(string.Format(LanguageManager.Singleton["UIMake_Cost_gold"], _MakeConfig.RequireGold));
                    }
                    foreach (var i in items)
                    {
                        var item = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                        if (item == null) continue;
                        sb.Append(string.Format(LanguageManager.Singleton["UIMake_Cost_Item"],item.Name,i.Value));
                    }

                    Template.lb_cost.text = sb.ToString();
                }
            }
        }
        #endregion

        #region 类别
        public class TypeItemGridTableModel : TableItemModel<TypeItemGridTableTemplate>
        {
            public TypeItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                Template.Bt_itemName.OnMouseClick((s, e) => {
                    if (OnClick == null) return;
                    OnClick(this);
                });

                Template.bt_info.OnMouseClick((s, e) => {
                    if (this.Category == null) return;



                    UIControllor.Singleton.ShowMessage(this.Category.Description,10f);
                });
            }

            public Action<TypeItemGridTableModel> OnClick;
            private ExcelConfig.MakeCategoryConfig _Category;
            public ExcelConfig.MakeCategoryConfig Category { get {
                return _Category;
            }
                set {
                    _Category = value;
                    Template.Bt_itemName.Text( value.Name);
                }
            }
        }
        #endregion
        
        public enum ShowTypeName
        {
            Types,
            Info
        }

        private ShowTypeName CurrentType = ShowTypeName.Types;
        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) => {
                 if(CurrentType == ShowTypeName.Info)
                 {
                     ShowTypes();
                 }
                 else { HideWindow(); }
            });
        }
        public override void OnShow()
        {
            base.OnShow();
            ShowTypes();
        }

        private void ShowTypes()
        {
            CurrentType = ShowTypeName.Types;
            PackageTypeView.ActiveSelfObject(true);
            PackageView.ActiveSelfObject(false);

            var configs = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigs<ExcelConfig.MakeCategoryConfig>();
            TypeItemGridTableManager.Count = configs.Length;
            int index = 0;
            foreach (var i in TypeItemGridTableManager)
            {
                i.Model.OnClick = OnClick;
                i.Model.Category = configs[index];
                index++;
            }
        }
        private void OnClick(TypeItemGridTableModel obj)
        {
            ShowType(obj.Category.ID);
        }
        public override void OnHide()
        {
            base.OnHide();
        }
        public void ShowType(int type)
        {
            CurrentType = ShowTypeName.Info;
            PackageTypeView.ActiveSelfObject(false);
            PackageView.ActiveSelfObject(true);
            var makeConfigs = ExcelConfig.ExcelToJSONConfigManager
                .Current.GetConfigs<ExcelConfig.MakeConfig>((t) => {
                if (t.Category == type) return true;
                return false;
            });
            int index = 0;
            ItemGridTableManager.Count = makeConfigs.Length;
            foreach (var i in ItemGridTableManager)
            {
                i.Model.MakeConfig = makeConfigs[index];
                index++;
            }
        }
    }
}