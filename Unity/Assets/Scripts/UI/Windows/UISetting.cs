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
                UIAchievement.Show();
            });

			Bt_Music.OnMouseClick ((s, e) => {
				DataManagers.GamePlayerManager.Singleton.MusicState(DataManagers.GamePlayerManager.Singleton.IsMusicOn ? 0:1);
			});

			Bt_MusicEffect.OnMouseClick ((s, e) => {
				DataManagers.GamePlayerManager.Singleton.EffectMusicState(DataManagers.GamePlayerManager.Singleton.EffectOn ? 0:1);

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
        }
        public override void OnHide()
        {
            base.OnHide();
        }
    }
}