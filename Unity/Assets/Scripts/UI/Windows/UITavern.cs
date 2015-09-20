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
                this.Item.Root.OnMouseClick((s, e) => {
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
                startTable.Count = monster.Star;
            }

            
            public Action<ItemGridTableModel> OnClick;
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
                    return DataManagers.PlayerArmyManager.Singleton.CanEmployHero(hero);
                });
            ItemGridTableManager.Count = heros.Length;
            int index = 0;
            foreach (var i in ItemGridTableManager)
            {
                i.Model.SetData(heros[index]);
                i.Model.SetDrag(heros.Length >= 4);
                i.Model.OnClick = OnItemClick;
                index++;
            }
        }

        private void OnItemClick(ItemGridTableModel obj)
        {
            var currentType = (EmployCostCurrent)obj.Hero.recruit_current_type;

            var cuurentName = currentType == EmployCostCurrent.Coin?
                LanguageManager.Singleton["APP_COIN"] : LanguageManager.Singleton["APP_GOLD"];

            UIMessageBox.ShowMessage(LanguageManager.Singleton["BUY_HERO_BT_OK"],
                string.Format(LanguageManager.Singleton["BUY_HERO_Message"], cuurentName, obj.Hero.recruit_price),
                () => {
                    if (DataManagers.PlayerArmyManager.Singleton.BuyHero(obj.Hero))
                    {
                        UIManager.Singleton.UpdateUIData();
                    }
                },
                null);
        }



        public override void OnHide()
        {
            base.OnHide();
        }
    }
}