using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;
using Proto;
using Assets.Scripts.DataManagers;

namespace Assets.Scripts.UI.Windows
{
    partial class UITavern
    {
        public class ItemGridTableModel : TableItemModel<ItemGridTableTemplate>
        {
            public ItemGridTableModel(){}
            public override void InitModel()
            {
                //todo
                startTable = new UITableManager<UITableItem>();
                startTable.InitFromGrid(this.Template.StartGrid);
                this.Template.Bt_Emp.OnMouseClick((s, e) => {
                    if (OnClick == null) return;
                    OnClick(this);
                });
            }

            private UITableManager<UITableItem> startTable;

            public MonsterConfig Monster { private set; get; }
            public HeroConfig Hero { private set; get; }
            public void SetDrag(bool canDrag)
            {
                var d = this.Item.Root.GetComponent<UIDragScrollView>();
                d.enabled = canDrag;
            }
            internal void SetData(HeroConfig heroConfig)
            {
                Hero = heroConfig;
                var monster = ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MonsterConfig>(heroConfig.recruit_id);
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
                DataManagers.PlayerArmyManager.Singleton.SetJob(Template.s_job, monster);
                DataManagers.PlayerArmyManager.Singleton.SetIcon(Template.icon, monster);
                startTable.Count = monster.Star;
            }

            public void SetCanEmp(bool can)
            {
                Template.s_lock.ActiveSelfObject(!can);
                Template.Bt_Emp.ActiveSelfObject(can);
                CanEmp = can;
            }
            
            public Action<ItemGridTableModel> OnClick;
            public bool CanEmp { private set; get; }
        }

        public override void InitModel()
        {
            base.InitModel();
            ItemGridTableManager.Cached = false;
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
            //Ã»ÓÐÕÐÄ¼µÄÓ¢ÐÛ
            var heros = ExcelToJSONConfigManager.Current.GetConfigs<HeroConfig>( (hero) =>
                {
                    if( DataManagers.PlayerArmyManager.Singleton.HaveEmployHero(hero)) return false;
                    return true;
                    //return DataManagers.PlayerArmyManager.Singleton.CanEmployHero(hero);
                });

            var noEmployHero = heros.Select(t => new
            {
                Hero = t,
                CanEmploy = DataManagers.PlayerArmyManager.Singleton.CanEmployHero(t)
            })
                .ToList();
            noEmployHero.Sort((l, r) => {
                if (l.CanEmploy == r.CanEmploy) return 0;
                if (l.CanEmploy) return -1;
                else return 1;
            });

            ItemGridTableManager.Count = noEmployHero.Count;
            int index = 0;
            foreach (var i in ItemGridTableManager)
            {
                i.Model.SetData(noEmployHero[index].Hero);
                i.Model.SetCanEmp((noEmployHero[index].CanEmploy));
                i.Model.SetDrag(heros.Length >= 4);
                i.Model.OnClick = OnItemClick;
                index++;
            }
        }

        private void OnItemClick(ItemGridTableModel obj)
        {
           

            UITavernMessageBox.Show(obj.Hero, () => {
                if (DataManagers.PlayerArmyManager.Singleton.BuyHero(obj.Hero))
                {
                    UIManager.Singleton.UpdateUIData();
                }
            }, null);

        }



        public override void OnHide()
        {
            base.OnHide();
        }
    }
}