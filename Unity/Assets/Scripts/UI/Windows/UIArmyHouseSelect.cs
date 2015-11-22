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
    partial class UIArmyHouseSelect
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel() { }

            public override void InitModel()
            {
                //todo

                this.Template.Bt_selected.OnMouseClick((s, e) =>
                {
                    if (OnClick == null) return;
                    OnClick(this);
                });

                startTable = new UITableManager<UITableItem>();
                startTable.InitFromGrid(this.Template.StartGrid);
                
            }

            private UITableManager<UITableItem> startTable;

            public DataManagers.PlayerSoldier _soldier;

            private void SetHero(PlayerSoldier hero)
            {
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
                DataManagers.PlayerArmyManager.Singleton.SetIcon(Template.icon, monster);
              
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

            public Action<ItemGridTableModel> OnClick;

            public MonsterConfig Monster { get; set; }

            public void SetDrag(bool canDrag)
            {
                var d = this.Item.Root.GetComponent<UIDragScrollView>();
                d.enabled = canDrag;
            }
        }

        public override void InitModel()
        {
            base.InitModel();
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
            var allHero = DataManagers.PlayerArmyManager.Singleton.GetAllSoldier();
            ItemGridTableManager.Count = allHero.Count;
            int index = 0;
            foreach (var i in ItemGridTableManager)
            {
                i.Model.PlayerSoldier = allHero[index];
                i.Model.OnClick = OnClickItem;
                i.Model.SetDrag(allHero.Count >= 5);
                index++;
            }
        }

        private void OnClickItem(ItemGridTableModel obj)
        {

            var levelup = ExcelToJSONConfigManager.Current.FirstConfig<MonsterLvlUpConfig>(t => {
                return t.OldMonster == obj.PlayerSoldier.SoldierID;
            });

            if (levelup == null)
            {
                UITipDrawer.Singleton.DrawNotify(LanguageManager.Singleton["MAX_LEVEL_SOLDIER"]);
                return;
            }
            UIArmyLevelUp.Show(obj.PlayerSoldier);
        }


        public override void OnHide()
        {
            base.OnHide();
        }
    }
}