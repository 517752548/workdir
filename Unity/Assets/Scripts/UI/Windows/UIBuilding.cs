using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

namespace Assets.Scripts.UI.Windows
{
    partial class UIBuilding
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                this.Template.bt_build.OnMouseClick((s, e) =>
               {
                   OnClick(this);
               }, this);
            }

            public Action<ItemGridTableModel> OnClick;

            private UIBuildData _Build;// { get; set; }
            public UIBuildData Build
            {
                get
                {
                    return _Build;
                }
                set {
                    _Build = value;
                    Template.bt_build.Disable(Build.NextLevelConfig == null);
                    Template.lb_build.text = Build.NextLevelConfig == null ? 
                        LanguageManager.Singleton["BUILD_LEVEL_MAX"] : string.Empty;
                    Template.lb_buildName.text = Build.Config.Name;
                    if (value.NextLevelConfig != null)
                    {
                        var needs = DataManagers.PlayerItemManager.SplitFormatItemData(Build.NextLevelConfig.CostItems);
                        var needItems = needs.Select(t => new
                        {
                            Config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(t[0]),
                            Num = t[1]
                        }).ToList();
                        if (needItems.Count > 0)
                        {
                            var sb = new StringBuilder();
                            foreach (var i in needItems)
                            {
                                sb.Append(string.Format("{0} *{1} ", i.Config.Name, i.Num));
                            }
                            this.Template.lb_NeedItems.text = sb.ToString();
                        }
                    }
                    else
                    {
                        this.Template.lb_NeedItems.text = string.Empty;
                    }

                }
            }

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
            this.bt_left.OnMouseClick((s, e) =>
            {
                this.HideWindow();
                var ui = UIManager.Singleton.CreateOrGetWindow<UICastlePanel>();
                ui.ShowWindow();
            });
            this.bt_right.ActiveSelfObject(false);
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();
            var builds = ExcelToJSONConfigManager.Current.GetConfigs<BuildingConfig>();
            var playerBuilds = DataManagers.BuildingManager.Singleton.GetConstructBuildingsList();
            var list = new List<UIBuildData>();
            foreach(var i in builds)
            {
                var build = new UIBuildData() { Config = i };
                var buildData = DataManagers.BuildingManager.Singleton[i.ID];
                int level = 0;
                if(buildData!=null)
                {
                    level = buildData.Level;
                }

             

                build.NextLevelConfig = ExcelToJSONConfigManager.Current.FirstConfig<BuildingConfig>(t =>
                {
                    if (t.Level == level + 1 && t.BuildingId == i.ID) return true;
                    return false;
                });
                list.Add(build);
            }

            list.Sort((l, r) => {
                if (l.Config != null) return -1;
                if (l.Config.ID > r.Config.ID) return 1;
                if (l.Config.ID == r.Config.ID) return 0;
                return -1;
            });

            ItemGridTableManager.Count = list.Count;
            int index = 0;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.Build = list[index];
                i.Model.OnClick = OnClick;
                i.Model.SetDrag(list.Count >= 10);
                index++;
            }
        }

        private void OnClick(ItemGridTableModel item)
        {
            if (item.Build.NextLevelConfig == null) return;
            var result = DataManagers.BuildingManager.Singleton.ConstructBuild(item.Build.Config.ID, 
                item.Build.NextLevelConfig.Level);
            if(result)
            {
                OnUpdateUIData();
            }
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }

    public class UIBuildData
    {
        public BuildingConfig Config { set; get; }

        public BuildingConfig NextLevelConfig { set; get; }
    }
}