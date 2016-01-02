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
        public class formStartGridTableModel : TableItemModel<formStartGridTableTemplate>
        {
            public formStartGridTableModel() { }
            public override void InitModel()
            {
                //todo
            }

            public void SetStar(bool flag)
            {
                this.Template.HSprite.ActiveSelfObject(flag);
                   //this.Template
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
                    HideWindow();
                }
            });

            bt_close.OnMouseClick((s, e) =>
            {
                HideWindow();
            });
            //Write Code here
        }

        public override void OnShow()
        {
            base.OnShow();
            OnUpdateUIData();
        }

        private void SetMonster(MonsterConfig oldMonster, MonsterConfig newMonster)
        {
            //formStartGridTableManager.Count = monsterOld.Star;
            //from_des.text = GetInfo(monsterOld);
            var skillName = string.Empty;
            var skill = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(oldMonster.SkillID);
            if (skill != null)
            {
                skillName = skill.Name;
            }
            lb_attack.text = string.Format(LanguageManager.Singleton["UITavern_Attack"], oldMonster.Damage);
            lb_hp.text = string.Format(LanguageManager.Singleton["UITavern_hp"], oldMonster.Hp);
            lb_skill.text = string.Format(LanguageManager.Singleton["UITavern_skill"], skillName);
            fromName.text = oldMonster.Name;


            DataManagers.PlayerArmyManager.Singleton.SetJob(formjob, oldMonster);
            DataManagers.PlayerArmyManager.Singleton.SetIcon(T_Icon, oldMonster, TypeOfIcon.BattleMax, false);
            this.formStartGridTableManager.Count =4;
            int index = 0;
            foreach(var i in formStartGridTableManager)
            {
                i.Model.SetStar(index < oldMonster.Star);
                    index++;
            }

            var skillNewName = string.Empty;
            var skillNew = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(newMonster.SkillID);


            if (skillNew != null)
            {
                skillNewName = skillNew.Name;
            }
            lb_attack_next.text = string.Format(LanguageManager.Singleton["UITavern_Attack"], newMonster.Damage);
            lb_hp_next.text = string.Format(LanguageManager.Singleton["UITavern_hp"], newMonster.Hp);
            lb_skill_next.text = string.Format(LanguageManager.Singleton["UITavern_skill"], skillName);
            fromName.text = oldMonster.Name;


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

            SetMonster(monsterOld, monsterLate);
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
