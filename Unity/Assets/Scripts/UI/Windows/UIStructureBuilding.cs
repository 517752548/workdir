using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

namespace Assets.Scripts.UI.Windows
{
    partial class UIStructureBuilding
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                this.Template.bt_info.OnMouseClick((s, e) => {

                    if (Config == null) return;
                    var sb = new StringBuilder();
                    sb.AppendLine(LanguageManager.Singleton["UIStructureBuilding_Cost_Title"]);
                    if (Config.CostGold > 0)
                    {
                        sb.Append(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Gold"], Config.CostGold));
                    }
                    var costItems = UtilityTool.SplitKeyValues(Config.CostItems);
                    foreach (var i in costItems)
                    {
                        var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                        if (item == null) continue;
                        sb.Append(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Item"], item.Name,i.Value));
                    }

                    UIControllor.Singleton.ShowMessage(sb.ToString(), 10);
                });
            }
            public BuildingConfig Config { private set; get; }
            internal bool SetBuild(List<BuildingConfig> list, int level)
            {
                Config = null;
                foreach (var i in list)
                {
                    if (i.Level == level)
                    {
                        Config = i;
                    }
                }
                if (Config == null) return false;

                Template.Bt_itemName.Text(Config.Name);
                var sb = new StringBuilder();
                if (Config.CostGold > 0)
                {
                    sb.AppendLine(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Gold"], Config.CostGold));
                }
                var costItems = UtilityTool.SplitKeyValues(Config.CostItems);
                foreach (var i in costItems)
                {
                    var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                    if (item == null) continue;
                    sb.Append(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Item"], item.Name, i.Value));
                }
                Template.lb_cost.text = sb.ToString();
                return true;
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            //Write Code here
            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
        }

        public override void OnShow()
        {
            base.OnShow();
            var buildingConfigs = ExcelToJSONConfigManager.Current.GetConfigs<BuildingConfig>();
            var buidingIDs = new Dictionary<int, List<BuildingConfig>>();
            foreach (var i in buildingConfigs)
            {
                if (!buidingIDs.ContainsKey(i.BuildingId))
                {
                    buidingIDs.Add(i.BuildingId, new List<BuildingConfig> { i});
                }
                else
                {
                    buidingIDs[i.BuildingId].Add(i);
                }
            }

            ItemGridTableManager.Count =  buidingIDs.Count;
            int index =0;
            foreach (var i in buidingIDs)
            {
                var isBuild = DataManagers.BuildingManager.Singleton.IsBuild(i.Key);
                int level = isBuild ? DataManagers.BuildingManager.Singleton[i.Key].Level : 1;
                ItemGridTableManager[index].Model.SetBuild(i.Value, level);
                index++;
            }
        }


        public override void OnHide()
        {
            base.OnHide();
        }
    }
}