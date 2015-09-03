using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Assets.Scripts.DataManagers;
using ExcelConfig;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    partial class UIGoToExplore
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }
            public override void InitModel()
            {
                //todo
                startTable = new UITableManager<UITableItem>();
                startTable.InitFromGrid(this.Template.StartGrid);
                this.Item.Root.OnMouseClick((s, e) =>
                {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }
            public Action<ItemGridTableModel> OnClick;
            public DataManagers.PlayerSoldier _soldier;

            private void SetHero(PlayerSoldier hero)
            {
                Template.dead.ActiveSelfObject(!hero.IsAlive);
                var monster = ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MonsterConfig>(hero.SoldierID);
                Monster = monster;
                if (monster == null) return;
                var skillName = string.Empty;
                var skill = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(monster.SkillID);
                if (skill != null)
                {
                    skillName = skill.Name;
                }
                Template.lb_attack.text = string.Format(LanguageManager.Singleton["UITavern_Attack"], monster.Damage);
                Template.lb_hp.text = string.Format(LanguageManager.Singleton["UITavern_hp"], monster.Hp);
                Template.lb_skill.text = string.Format(LanguageManager.Singleton["UITavern_skill"], skillName);
                Template.lb_name.text = monster.Name;
                startTable.Count = monster.Star;
                DataManagers.PlayerArmyManager.Singleton.SetJob(Template.s_job, monster);
                Template.icon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("Icon/" + monster.ResName);
            }

            public DataManagers.PlayerSoldier PlayerSoldier
            {
                get { return _soldier; }
                set
                {
                    _soldier = value;
                    SetHero(_soldier);
                }
            }

            private UITableManager<UITableItem> startTable;

            public MonsterConfig Monster { get; set; }

            private bool selected = false;

            public bool IsSelected
            {
                set
                {
                    Template.Checked.ActiveSelfObject(value);
                    selected = value;
                }
                get { return selected; }
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) =>
            {
                HideWindow();
            });

            bt_go.OnMouseClick((s, e) =>
            {
                var team = new List<int>();
                foreach (var i in ItemGridTableManager)
                {
                    if (i.Model.IsSelected)
                    {
                        team.Add(i.Model.PlayerSoldier.SoldierID);
                    }
                }
                if (team.Count == 0)
                {
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_GOEXPLORE_NEED_ARMY"]);
                    return;
                }

                DataManagers.PlayerArmyManager.Singleton.SetTeam(team);

                App.GameAppliaction.Singleton.GoToExplore(DataManagers.GamePlayerManager.Singleton.CurrentMap);
            });

            bt_add.OnMouseClick((s, e) =>
            {
                if (DataManagers.GamePlayerManager.Singleton.FoodCount == DataManagers.GamePlayerManager.Singleton.PackageSize)
                {
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_GOEXPLORE_NO_PLACE"]);
                    return;
                }
                var foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;
                if (DataManagers.PlayerItemManager.Singleton.GetItemCount(foodEntry) < 1)
                {
                    var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(foodEntry);
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_GOEXPLORE_NO_FOOD"],config.Name);
                    return;
                }
                if (DataManagers.GamePlayerManager.Singleton.AddFood(1))
                {
                    ShowFood();
                }
            });

            bt_cal.OnMouseClick((s, e) =>
            {
                if (DataManagers.GamePlayerManager.Singleton.SubFood(1))
                {
                    ShowFood();
                }
            });
            //Write Code here
        }

        private void ShowFood()
        {
            lb_packageSize.text = string.Format(LanguageManager.Singleton["UI_GOEXPLORE_PACKAGE"],
                DataManagers.GamePlayerManager.Singleton.FoodCount, DataManagers.GamePlayerManager.Singleton.PackageSize);
            this.lb_foodvalue.text = string.Format("{0}", DataManagers.GamePlayerManager.Singleton.FoodCount);
        }

        private void ShowArmyCount()
        {
            var selectCount = 0;
            foreach (var i in ItemGridTableManager)
            {
                if (i.Model.IsSelected)
                    selectCount++;
            }
            lb_herolb.text = string.Format(LanguageManager.Singleton["UI_GOEXPLORE_TEAM_SIZE"],
                selectCount, DataManagers.GamePlayerManager.Singleton.TeamSize);
        }

        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }

        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();            
            var foodEntry = App.GameAppliaction.Singleton.ConstValues.FoodItemID;
            var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(foodEntry);
            lb_food_name.text = config.Name;
          
            var allHero = DataManagers.PlayerArmyManager.Singleton.GetAllSoldier();
            ItemGridTableManager.Count = allHero.Count;
            int index = 0;
            foreach (var i in ItemGridTableManager)
            {
                i.Model.PlayerSoldier = allHero[index];
                i.Model.OnClick = OnClickItem;
                i.Model.IsSelected = DataManagers.PlayerArmyManager.Singleton.IsTeam(allHero[index].SoldierID);
                index++;
            }

            ShowFood();
            ShowArmyCount();
        }

        private void OnClickItem(ItemGridTableModel obj)
        {
            if (!obj.PlayerSoldier.IsAlive)
            {
                var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(App.GameAppliaction.Singleton.ConstValues.ReliveNeedItem);

                UIMessageBox.ShowMessage(LanguageManager.Singleton["UI_GOEXPLORE_Relive_OK"],
                    string.Format(LanguageManager.Singleton["UI_GOEXPLORE_Relive_Message"], config.Name, obj.Monster.Name),
                    () => {
                        if (DataManagers.PlayerArmyManager.Singleton.Relive(obj.PlayerSoldier.SoldierID))
                        {
                            UIManager.Singleton.UpdateUIData();
                        }
                    },
                    null);

                return;
               //复活
            }

            if (obj.IsSelected)
            {
                obj.IsSelected = false;
            }
            else
            {

                var selectCount = 0;
                foreach (var i in ItemGridTableManager)
                {
                    if (i.Model.IsSelected)
                        selectCount++;
                }
                if (selectCount >= DataManagers.GamePlayerManager.Singleton.TeamSize)
                {
                    //已经上限了
                    UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["UI_GOEXPLORE_NO_TEAM_PLACE"]);
                    return;
                }

                obj.IsSelected = true;
            }
            ShowArmyCount();

        }

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}