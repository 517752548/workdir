using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proto;
using ExcelConfig;

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
		public void ShowChestDialog(int mapID,
			List<Item> reward, 
			int index,
			Action<int,int,List<Item>> callBack)
		{
			
			var ui =UI.Windows.UIBattleResult.Show ();
			ui.ShowResult (mapID, reward, index);
			ui.callAfterCollect = callBack;
		}
		//切换地图UI
		public void ShowMapListUI()
		{
			var maps = DataManagers.PlayerMapManager.Singleton.GetOpenedMaps ();
			if (maps.Count < 2)
				return;
			UI.Windows.UIMapList.Show ();
		   
			//show map list ui
		}
		//驿站UI
		public void ShowRechargeUI(List<Item> shop)
		{
			
		}
    }
}
