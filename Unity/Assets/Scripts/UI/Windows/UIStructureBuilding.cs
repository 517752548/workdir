using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;
using Assets.Scripts.DataManagers;

namespace Assets.Scripts.UI.Windows
{
    partial class UIStructureBuilding
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                this.Template.bt_info.OnMouseClick((s, e) =>
                {
                    if (!need) return;
                    if (Build == null) return;
                    UIControllor.Singleton.ShowMessage(LanguageManager.ReplaceEc(Build.Config.Describe), 10);
                });

                this.Template.Bt_itemName.OnMouseClick((s, e) =>
                {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }
            public PlayerBuild Build { private set; get; }
            internal bool SetBuild(PlayerBuild build)
            {
                Build = build;
                if (Build == null) return false;
                var nextConfig = DataManagers.BuildingManager.Singleton.GetConfig(Build.BuildID, build.Level + 1);
                Template.Bt_itemName.Text(Build.Config.Name + (Build.Level > 0 ? " " + Build.Level : ""));
                var sb = new StringBuilder();
                if (Build.Config.CostGold > 0)
                {
                    sb.AppendLine(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Gold"], Build.Config.CostGold));
                }
                var costItems = UtilityTool.SplitKeyValues(Build.Config.CostItems);
                foreach (var i in costItems)
                {
                    var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                    if (item == null) continue;
                    sb.Append(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Item"], item.Name, i.Value));
                }
                var require = sb.ToString();
                need = !string.IsNullOrEmpty(Build.Config.Describe);
                Template.lb_cost.text =
                   (nextConfig == null ? LanguageManager.Singleton["UIStructureBuilding_Cost_MaxLevel"] : (string.IsNullOrEmpty(require) ?
                    LanguageManager.Singleton["UIStructureBuilding_Cost_None"] : require));
                return true;
            }

            public Action<ItemGridTableModel> OnClick;
            public void SetDrag(bool canDrag)
            {
                var d = this.Item.Root.GetComponent<UIDragScrollView>();
                d.enabled = canDrag;
            }
            private bool need = false;

        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) =>
            {
                HideWindow();
            });
        }

        public override void OnShow()
        {
            base.OnShow();

            OnUpdateUIData();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            var dic = new HashSet<int>();
            var builds = ExcelToJSONConfigManager.Current.GetConfigs<BuildingConfig>();

            foreach (var i in builds)
            {
                if (dic.Contains(i.BuildingId)) continue;
                dic.Add(i.BuildingId);
            }

            //var buildingConfigs = DataManagers.BuildingManager.Singleton.GetConstructBuildingsList();
            ItemGridTableManager.Count = dic.Count;
            int index = 0;
            foreach (var b in dic)
            {
                var i = DataManagers.BuildingManager.Singleton[b];

                ItemGridTableManager[index].Model.SetBuild(i);
                ItemGridTableManager[index].Model.SetDrag(dic.Count >= 8);
                ItemGridTableManager[index].Model.OnClick = OnClickItem;
                index++;
            }
        }

        private void OnClickItem(ItemGridTableModel obj)
        {
            if (DataManagers.BuildingManager.Singleton.ConstructBuild(obj.Build.BuildID, obj.Build.Level+1))
            {
                UIManager.Singleton.UpdateUIData();
            }
        }

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}