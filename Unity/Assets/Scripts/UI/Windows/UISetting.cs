using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using Proto;

namespace Assets.Scripts.UI.Windows
{
    partial class UISetting
    {

        public override void InitModel()
        {
            base.InitModel();
            bt_close.OnMouseClick((s, e) => {
                this.HideWindow();
            });

            Bt_Achievement.OnMouseClick((s, e) => {
				HideWindow();
                UIAchievement.Show();
            });

			Bt_Music.OnMouseClick ((s, e) => {
				DataManagers.GamePlayerManager.Singleton.MusicState(
					DataManagers.GamePlayerManager.Singleton.IsMusicOn ? 1:0);
			});

			Bt_MusicEffect.OnMouseClick ((s, e) => {
				DataManagers.GamePlayerManager.Singleton.EffectMusicState(
					DataManagers.GamePlayerManager.Singleton.EffectOn ? 1:0);

			});
			Bt_Reset.OnMouseClick (
				(s, e) => {
					UIMessageBox.ShowMessage(
						LanguageManager.Singleton["RESET_TITLE"], 
						LanguageManager.Singleton["RESET_CONTENT"],
						()=>{App.GameAppliaction.Singleton.ResetPlayData(); this.HideWindow();},
						()=>{ });
			});

			Bt_GameSkill.OnMouseClick ((s, e) => {
				HideWindow();
				UIPlaySKill.Show();
			});
            //Write Code here
        }
        public override void OnShow()
        {
            base.OnShow();
            lb_version.text = string.Format(LanguageManager.Singleton["UI_Setting_version"],
                (int)GameVersion.Master, (int)GameVersion.Major, (int)GameVersion.Develop);

			Bt_Music.value = DataManagers.GamePlayerManager.Singleton.IsMusicOn;
			Bt_MusicEffect.value = DataManagers.GamePlayerManager.Singleton.EffectOn;
			UI.UIControllor.Singleton.ShowOrHideMessage( false);
			Bt_GameSkill.ActiveSelfObject (false);
        }
        public override void OnHide()
        {
            base.OnHide();
			UI.UIControllor.Singleton.ShowOrHideMessage( true);
        }


    }
}