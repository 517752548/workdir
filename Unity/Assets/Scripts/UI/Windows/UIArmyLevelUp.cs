using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;
using UnityEngine;
using Assets.Scripts.DataManagers;

namespace Assets.Scripts.UI.Windows
{
    partial class UIArmyLevelUp
    {
        public class LevelUpGridTableModel : TableItemModel<LevelUpGridTableTemplate>
        {
            public LevelUpGridTableModel() { }
            public override void InitModel()
            {
                //todo
                startTable = new UITableManager<UITableItem>();
                startTable.InitFromGrid(this.Template.formStartGrid);
            }

            private UITableManager<UITableItem> startTable;

            private void SetMonster(MonsterConfig monster)
            {
                //formStartGridTableManager.Count = monsterOld.Star;
                //from_des.text = GetInfo(monsterOld);
                var skillName = string.Empty;
                var skill = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(monster.SkillID);
                if (skill != null)
                {
                    skillName = skill.Name;
                }
                Template.lb_attack.text = string.Format(LanguageManager.Singleton["UITavern_Attack"], monster.Damage);
                Template.lb_hp.text = string.Format(LanguageManager.Singleton["UITavern_hp"], monster.Hp);
                Template.lb_skill.text = string.Format(LanguageManager.Singleton["UITavern_skill"], skillName);
                Template.fromName.text = monster.Name;
                DataManagers.PlayerArmyManager.Singleton.SetJob(Template.formjob, monster);
                DataManagers.PlayerArmyManager.Singleton.SetIcon(Template.formicon, monster, TypeOfIcon.LvlUp);
                startTable.Count = monster.Star;

            }

            private MonsterConfig _Monster { get; set; }
            public MonsterConfig Monster
            {
                get { return _Monster; }
                set
                {

                    _Monster = value;
                    SetMonster(value);
                }
            }
        }

        public override void InitModel()
        {
            base.InitModel();
            bt_cancel.OnMouseClick((s, e) =>
            {
                HideWindow();
            });
            bt_ok.OnMouseClick((s, e) =>
            {
                if (DataManagers.PlayerArmyManager.Singleton.LevelUp(Soldier))
                {
                    UIManager.Singleton.UpdateUIData();
                }
                HideWindow();
            });

            bt_close.OnMouseClick((s, e) => {
                HideWindow();
            });
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

            var levelup = ExcelConfig.ExcelToJSONConfigManager.Current.FirstConfig<ExcelConfig.MonsterLvlUpConfig>((t => t.OldMonster == this.Soldier.SoldierID));
            if (levelup == null)
            {
                this.HideWindow();
                return;
            }

            var monsterOld = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(levelup.OldMonster);
            var monsterLate = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(levelup.LateMonster);

            var data = new List<MonsterConfig>() { monsterOld, monsterLate };

            LevelUpGridTableManager.Count = data.Count;
            int index = 0;
            foreach (var i in LevelUpGridTableManager)
            {
                i.Model.Monster = data[index];
                index++;
            }

            var sb = new StringBuilder();
            if (levelup.CostGold > 0)
            {
                var Color = levelup.CostGold <= DataManagers.GamePlayerManager.Singleton.Gold ?
                  LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];
                sb.AppendLine(string.Format(LanguageManager.Singleton["UI_LVLUP_Cost_Gold"],
                    string.Format(Color, levelup.CostGold)));
            }
            var costItems = UtilityTool.SplitKeyValues(levelup.CostItems, levelup.CostItemsNumber);
            foreach (var i in costItems)
            {
                var item = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(i.Key);
                if (item == null) continue;

                var Color = PlayerItemManager.Singleton.GetItemCount(i.Key) >= i.Value ?
                 LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];

                sb.Append(string.Format(LanguageManager.Singleton["UI_LVLUP_Cost_Item"],
                    item.Name,
                    string.Format(Color, i.Value)));
            }

            lb_Message.text = sb.ToString();
        }


        public override void OnHide()
        {
            base.OnHide();
        }

        private DataManagers.PlayerSoldier Soldier;

        public static void Show(DataManagers.PlayerSoldier soldier)
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UIArmyLevelUp>();
            ui.Soldier = soldier;
            ui.ShowAsDialogWindow(false);
        }
    }
}