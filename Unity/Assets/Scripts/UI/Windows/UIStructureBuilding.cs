using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;
using Assets.Scripts.DataManagers;
using Proto;

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
                    var unlock = string.Empty;
                    switch ((BuildingUnlockType)Build.Config.UnlockType)
                    { 
                        case BuildingUnlockType.NeedBuild:
                            var build = Tools.UtilityTool.ConvertToInt(Build.Config.UnlockParms1);
                            var buildConfig = ExcelToJSONConfigManager.Current.GetConfigByID<BuildingConfig>(build);
                            unlock = string.Format(LanguageManager.Singleton["UI_Struct_Need_Build"], buildConfig.Name, buildConfig.Level+1);
                            break;
                    }
                    UIControllor.Singleton.ShowMessage(LanguageManager.ReplaceEc(Build.Config.Describe)+'\n'+unlock, 10);
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
                var nextConfig = build.NextLevelConfig;
                Template.Bt_itemName.Text(Build.Config.Name + (Build.Level > 0 ? " " + Build.Level : ""));
                var sb = new StringBuilder();
                if (Build.Config.CostGold > 0)
                {
                    var Color = Build.Config.CostGold <= DataManagers.GamePlayerManager.Singleton.Gold ?
                      LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];
                    sb.AppendLine(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Gold"], 
                        string.Format(Color, Build.Config.CostGold)));
                }
                var costItems = UtilityTool.SplitKeyValues(Build.Config.CostItems);
                foreach (var i in costItems)
                {
                    var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                    if (item == null) continue;

                    var Color = PlayerItemManager.Singleton.GetItemCount(i.Key)>=i.Value ?
                     LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];

                    sb.Append(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Item"], 
                        item.Name, 
                        string.Format(Color,i.Value)));
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

            var list = new List<PlayerBuild>();
            foreach (var i in dic)
            {
                var build = DataManagers.BuildingManager.Singleton[i];
                if (build == null) continue;
                switch ((BuildingUnlockType)build.Config.UnlockType)
                {
                    case BuildingUnlockType.NONE: break;
                    case BuildingUnlockType.NeedBuild:
                        var buildID = Tools.UtilityTool.ConvertToInt(build.Config.UnlockParms1);
                        var buildConfig = ExcelToJSONConfigManager.Current.GetConfigByID<BuildingConfig>(buildID);
                        if (!BuildingManager.Singleton.HaveBuild(buildConfig.BuildingId, buildConfig.Level))
                            continue;
                        break;
                }
                //if (build.NextLevelConfig == null) continue;
                list.Add(build);
            }

            
            //var buildingConfigs = DataManagers.BuildingManager.Singleton.GetConstructBuildingsList();
            ItemGridTableManager.Count = list.Count;
            int index = 0;
            foreach (var i in list)
            {

                ItemGridTableManager[index].Model.SetBuild(i);
                ItemGridTableManager[index].Model.SetDrag(list.Count >= 8);
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