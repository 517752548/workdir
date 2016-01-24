using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proto;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// UI的控制，处理一些游戏中UI功能操作
    /// </summary>
    public class UIControllor:Tools.XSingleton<UIControllor>
    {
        public void HideAllUI() 
        {
            UIManager.Singleton.Each<UIWindow>((ui) => {
                ui.HideWindow();
                return false;
            });
        }


        internal void ShowMessage(string msg, float delayTime=-1f)
        {
            var uirender = UIManager.Singleton.Render;
            uirender.ShowMessage(msg, delayTime);
        }

        public void ShowOrHideMessage(bool show)
        {
            var uirender = UIManager.Singleton.Render;
            uirender.ShowOrHideMessage( show);
        }


		public void OpenScrectShop(int shopID, int mapID, int index)
		{
			
		}

		public void ShowChestDialog(List<Item> need, List<Item> reward, int index)
		{
			
		}
    }
}
