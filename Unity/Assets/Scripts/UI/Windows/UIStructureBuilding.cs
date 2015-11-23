using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;
using Assets.Scripts.DataManagers;
using Proto;
using UnityEngine;

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
                    if (Build == null) return;
                    var next = Build.NextLevelConfig;
                    if (next == null) return;
                    var unlock = string.Empty;
                    switch ((BuildingUnlockType)next.UnlockType)
                    { 
                        case BuildingUnlockType.NeedBuild:
                            var build = Tools.UtilityTool.ConvertToInt(next.UnlockParms1);
                            var buildConfig = ExcelToJSONConfigManager.Current.GetConfigByID<BuildingConfig>(build);
                            unlock = string.Format(LanguageManager.Singleton["UI_Struct_Need_Build"], buildConfig.Name, buildConfig.Level);
                            break;
                    }

                    
                    UIControllor.Singleton.ShowMessage(LanguageManager.ReplaceEc(next.Describe) + '\n' + unlock, 10);
                });

                this.Template.IconBuild.OnMouseClick((s, e) =>
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
                if (nextConfig != null)
                {

                    Template.IconBuild.mainTexture = ResourcesManager.Singleton.LoadResources<Texture2D>("BuildIcons/" + build.NextLevelConfig.Icon);
                    Template.lb_name.text = Build.Name;
                    Template.lb_lvl.text = (Build.Level > 0 ? "" + Build.Level : "0");
                    var sb = new StringBuilder();
                    if (nextConfig.CostGold > 0)
                    {
                        var Color = nextConfig.CostGold <= DataManagers.GamePlayerManager.Singleton.Gold ?
                          LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];
                        sb.Append(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Gold"],
                            string.Format(Color, nextConfig.CostGold)));
                    }
                    var costItems = UtilityTool.SplitKeyValues(nextConfig.CostItems, nextConfig.CostItemCounts);
                    foreach (var i in costItems)
                    {
                        var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                        if (item == null) continue;

                        var Color = PlayerItemManager.Singleton.GetItemCount(i.Key) >= i.Value ?
                         LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];

                        sb.Append(string.Format(LanguageManager.Singleton["UIStructureBuilding_Cost_Item"],
                            item.Name,
                            string.Format(Color, i.Value)));
                    }
                    var require = sb.ToString();
                    Template.lb_cost.text =
                       (nextConfig == null ? LanguageManager.Singleton["UIStructureBuilding_Cost_MaxLevel"] : (string.IsNullOrEmpty(require) ?
                        LanguageManager.Singleton["UIStructureBuilding_Cost_None"] : require));
                }
                else
                {
                    Template.lb_cost.text = LanguageManager.Singleton["UIStructureBuilding_Cost_MaxLevel"];
                    Template.lb_name.text = Build.Name;
                    Template.lb_lvl.text = (Build.Level > 0 ? "" + Build.Level : "0");

                    Template.IconBuild.mainTexture = ResourcesManager.Singleton.LoadResources<Texture2D>("BuildIcons/" + build.Config.Icon);
                }

                var next = Build.NextLevelConfig;
                var disable =next == null;

                Template.bt_info.ActiveSelfObject(!disable);
                return true;
            }

            public Action<ItemGridTableModel> OnClick;
            public void SetDrag(bool canDrag)
            {
                var d = this.Item.Root.GetComponent<UIDragScrollView>();
                d.enabled = canDrag;
            }

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
                if (build.NextLevelConfig != null)
                {
                    switch ((BuildingUnlockType)build.NextLevelConfig.UnlockType)
                    {
                        case BuildingUnlockType.NONE: break;
                        case BuildingUnlockType.NeedBuild:
                            var buildID = Tools.UtilityTool.ConvertToInt(build.NextLevelConfig.UnlockParms1);
                            var buildConfig = ExcelToJSONConfigManager.Current.GetConfigByID<BuildingConfig>(buildID);
                            if (!BuildingManager.Singleton.HaveBuild(buildConfig.BuildingId, buildConfig.Level))
                                continue;
                            break;
                       
                    }
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
                ItemGridTableManager[index].Model.SetDrag(list.Count >= 5);
                ItemGridTableManager[index].Model.OnClick = OnClickItem;
                index++;
            }
        }

        private void OnClickItem(ItemGridTableModel obj)
        {
            if (DataManagers.BuildingManager.Singleton.ConstructBuild(obj.Build.BuildID, obj.Build.Level + 1))
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