using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proto;
using ExcelConfig;
using Assets.Scripts.GameStates;
using Assets.Scripts.UI.Windows;
using UnityEngine;

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

		private LinkedList<string> message = new LinkedList<string> ();

        internal void ShowMessage(string msg)
        {
			if (!(App.GameAppliaction.Singleton.Current is CastleState))
				return;
			if (string.IsNullOrEmpty (msg))
				return;
			message.AddLast (msg);
			if (message.Count > 40)
				message.RemoveFirst ();
			var showMessage = GetMessage ();
            var uirender = UIManager.Singleton.Render;
			uirender.ShowMessage(showMessage);
        }

		private string GetMessage()
		{
			var sb = new StringBuilder ();
			foreach (var i in message) {
				sb.AppendLine (i);
			}
			return sb.ToString ();
		}

		private int _lock = 0;

        public void ShowOrHideMessage(bool show)
		{

			if (show) {
				_lock++;
			} else {
			
				_lock--;
			}

			var uirender = UIManager.Singleton.Render;
			uirender.ShowOrHideMessage (show);
			if (show && _lock > 0) {
				var showMessage = GetMessage ();
				uirender.ShowMessage (showMessage);			
			} else {
				//message.Clear ();

				uirender.ShowMessage (string.Empty);
			}

			uirender.ShowOrHideMessage(_lock>0);
		}

		public void ShowInfo(string message,float delay = 4f){
		
			UIManager.Singleton.Render.ShowInfo (message, delay);
		}

		//奸商商店
		public void OpenScrectShop(int shopID, int mapID, int index)
		{
			//screct shop
			 UI.Windows.UIScrectShop.ShowMapScrectShop(mapID,index,shopID);
		
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
			if (maps.Count < 2) {
				ShowInfo (LanguageManager.Singleton ["GONextMapNoMap"]);
				return;
			}
			UI.Windows.UIMapList.Show ();
		   
			//show map list ui
		}
		//驿站UI
		public void ShowRechargeUI(int mapID, int index,int itemID, int gold)
		{
			var ui = UIChargeShop.Show ();
			ui.ShowFood (itemID, gold);
			return;
		}

		public string MaskEventObjectName = string.Empty;


		public void SetMaskEventObject(GameObject obj)
		{
			MaskEventObjectName = obj.name;
			//Debug.LogError ("Mask:" + obj.name);

		}
		public void ClearMaskEvent()
		{
			MaskEventObjectName = string.Empty;
			//Debug.LogError ("Mask clear!!!");
		}
    }
}
