using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using ExcelConfig;
using Proto;

namespace Assets.Scripts.UI.Windows
{
    partial class UITavernMessageBox
    {

        public override void InitModel()
        {
            base.InitModel();
            c_close.OnMouseClick((s, e) => {
                HideWindow();
                if (Cancel != null) Cancel();
            
            });

            bt_cancel.OnMouseClick((s, e) => { HideWindow(); if (Cancel != null) Cancel(); });
            bt_ok.OnMouseClick((s, e) => {
                HideWindow();
                if (OK != null) OK();
            });
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        private Action OK;
        private Action Cancel;

        public static void Show(HeroConfig hero, Action ok, Action cancel)
        {
            var ui = UIManager.Singleton.CreateOrGetWindow<UITavernMessageBox>();
            ui.ShowAsDialogWindow(true);
            ui.SetHero(hero);
            ui.OK = ok;
            ui.Cancel = cancel;
        }

        private void SetHero(HeroConfig hero)
        {
            var currentType = (EmployCostCurrent)hero.recruit_current_type;

            var cuurentName = currentType == EmployCostCurrent.Coin ?
                LanguageManager.Singleton["APP_COIN"] : LanguageManager.Singleton["APP_GOLD"];
            var monster = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MonsterConfig>(hero.recruit_id);
            lb_Title.text = string.Format(LanguageManager.Singleton["UITavern_Title_MessageBox"], monster.Name);
            lb_Message.text = hero.recruit_des;// monster.Description;
            lb_cost.text = string.Format(LanguageManager.Singleton["BUY_HERO_Message"], cuurentName, hero.recruit_price);
            var category = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterCategoryConfig>(monster.Type);
            lb_job.text = string.Format(LanguageManager.Singleton["UITavern_MessageBox_Job"], category.Name);
            //throw new NotImplementedException();
        }
    }
}