using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;

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
            }

            private UITableManager<UITableItem> startTable;

            public MonsterConfig Monster { private set; get; }

            internal void SetData(HeroConfig heroConfig)
            {
                var monster = ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MonsterConfig>(heroConfig.recruit_id);
                Monster = monster;
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
            var heros = ExcelToJSONConfigManager.Current.GetConfigs<HeroConfig>((hero) => {
                return true;
            });
            ItemGridTableManager.Count = heros.Length;
            int index = 0;
            foreach(var i in ItemGridTableManager)
            {
                i.Model.SetData(heros[index]);
                index++;
            }
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}