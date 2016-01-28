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


		private bool _hide = false;
		public bool HidenMessage {
			set {
				_hide = value;	
				var uirender = UIManager.Singleton.Render;
				uirender.ShowMessage (string.Empty, -1);
			}
			get{ return _hide; } 
		}

        internal void ShowMessage(string msg, float delayTime=-1f)
        {
			if (_hide)
				return;
            var uirender = UIManager.Singleton.Render;
            uirender.ShowMessage(msg, delayTime);
        }

        public void ShowOrHideMessage(bool show)
        {
            var uirender = UIManager.Singleton.Render;
            uirender.ShowOrHideMessage( show);
        }

		//奸商商店
		public void OpenScrectShop(int shopID, int mapID, int index)
		{
			
		}
		//开宝箱的UI
		public void ShowChestDialog(List<Item> need, List<Item> reward, int index)
		{


		}
		//切换地图UI
		public void ShowMapListUI()
		{
			
		}
		//驿站UI
		public void ShowRechargeUI(List<Item> shop)
		{
			
		}
    }
}
