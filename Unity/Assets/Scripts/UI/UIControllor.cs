using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI
{
    public class UIControllor:Tools.XSingleton<UIControllor>
    {
        public void HideAllUI() 
        {
            UIManager.Singleton.Each<UIWindow>((ui) => {
                ui.HideWindow();
                return false;
            });
        }

        public void ShowOrHideUIBackground(bool flag)
        {
            var uiReander = UIManager.Singleton.Render as UIRender;
            if (uiReander == null) return;
            if (uiReander.BackgroundTexutre == null) return;
            uiReander.BackgroundTexutre.gameObject.SetActive ( flag);
        }
    }
}
