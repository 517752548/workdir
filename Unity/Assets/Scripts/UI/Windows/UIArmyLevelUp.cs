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
        }
        public class toStartGridTableModel : TableItemModel<toStartGridTableTemplate>
        {
            public toStartGridTableModel() { }
            public override void InitModel()
            {
                //todo
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

            formStartGridTableManager.Count = monsterOld.Star;
            from_des.text = GetInfo(monsterOld);
            fromName.text = monsterOld.Name;
            formicon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("Icon/" + monsterOld.ResName);
            DataManagers.PlayerArmyManager.Singleton.SetJob(formjob, monsterOld);

            toStartGridTableManager.Count = monsterLate.Star; 
            to_des.text = GetInfo(monsterLate);
            toName.text = monsterLate.Name;
            toicon.mainTexture = Tools.ResourcesManager.Singleton.LoadResources<Texture2D>("Icon/" +monsterLate.ResName);
            DataManagers.PlayerArmyManager.Singleton.SetJob(tojob, monsterLate);


            var sb = new StringBuilder();
            if (levelup.CostGold > 0)
            {
                var Color = levelup.CostGold <= DataManagers.GamePlayerManager.Singleton.Gold ?
                  LanguageManager.Singleton["APP_GREEN"] : LanguageManager.Singleton["APP_RED"];
                sb.AppendLine(string.Format(LanguageManager.Singleton["UI_LVLUP_Cost_Gold"],
                    string.Format(Color, levelup.CostGold)));
            }
            var costItems = UtilityTool.SplitKeyValues(levelup.CostItems);
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

        private string GetInfo(MonsterConfig monster)
        {
            //UI_LVLUP_SKILL_NAME
            //UI_LVLUP_HP
            //UI_LVLUP_DAMAGE
            //UI_LVLUP_Speed

            var skill = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(monster.SkillID);
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(LanguageManager.Singleton["UI_LVLUP_SKILL_NAME"], skill.Name));
            sb.AppendLine(string.Format(LanguageManager.Singleton["UI_LVLUP_HP"], monster.Hp));
            sb.AppendLine(string.Format(LanguageManager.Singleton["UI_LVLUP_DAMAGE"], monster.Damage));
            sb.AppendLine(string.Format(LanguageManager.Singleton["UI_LVLUP_Speed"], monster.Speed));
            return sb.ToString();
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