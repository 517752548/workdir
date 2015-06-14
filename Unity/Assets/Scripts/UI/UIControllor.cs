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
            UIManager.Singleton.Render.ShowOrHideBack(flag);
        }
    }
}
