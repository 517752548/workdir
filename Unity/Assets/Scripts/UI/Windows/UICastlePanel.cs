using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    partial class UICastlePanel
    {
        public class BagGridTableModel : TableItemModel<BagGridTableTemplate>
        {

            public BagGridTableModel() { }
            public override void InitModel()
            {
                //todo
            }
            private DataManagers.PlayerGameItem _GameItem;
            public DataManagers.PlayerGameItem GameItem
            {
                get
                {
                    return _GameItem;
                }
                set
                {
                    _GameItem = value;
                    Template.lb_name.text = value.Config.Name;
                    Template.lv_num.text = string.Format(string.Format(LanguageManager.Singleton["APP_NUM_FORMAT"], value.Num));
                }
            }

            public void SetDrag(bool canDrag)
            {
				var d = this.Item.Root.GetComponent<UIDragScrollView>();
				if (d == null)
					return;
                d.enabled = canDrag;
            }
        }


        public override void InitModel()
		{
			base.InitModel ();
			//Write Code here
			bt_market.OnMouseClick ((s, e) => {
				//this.HideWindow();
				var ui = UIManager.Singleton.CreateOrGetWindow<UIShop> ();
				ui.ShowWindow ();
				UIControllor.Singleton.ClearMaskEvent();

				if(shopCompleted !=null)
				{
					shopCompleted();
					shopCompleted =null;
				}
				if(shopFinger !=null)
				{
					GameObject.Destroy(shopFinger);
					shopFinger =null;
				}

			});

			bt_bar.OnMouseClick ((s, e) => {
				var ui = UIManager.Singleton.CreateOrGetWindow<UITavern> ();
				ui.ShowWindow ();

				UIControllor.Singleton.ClearMaskEvent();
				if(traveFinger!=null)
				{
					GameObject.Destroy(traveFinger);
					traveFinger =null;
				}

				if(traveCompleted!=null)
				{
					traveCompleted();
					traveCompleted=null;
				}
			});

			bt_produce.OnMouseClick ((s, e) => {

				if (produceFinger != null){
					GameObject.Destroy (produceFinger);
					produceFinger =null;
				}

				if (DataManagers.GamePlayerManager.Singleton.OpenProduceConfigs ().Count == 0) {
					//NO_BUILD
					//UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_BUILD"], 3f);
					UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["NO_BUILD"]);
					return;
				}
				var ui = UIManager.Singleton.CreateOrGetWindow<UIProducePanel> ();
				ui.ShowWindow ();

				UI.UIControllor.Singleton.ClearMaskEvent();
				if(this.produceCompleted!=null)
				{
					this.produceCompleted();
					this.produceCompleted=null;
				}


			});

			this.PlayerIcon.OnMouseClick ((s, e) => {
				var ui = UIManager.Singleton.CreateOrGetWindow<UISetting> ();
				ui.ShowWindow ();
			});

			bt_make.OnMouseClick ((s, e) => {
				var makeConfigs = ExcelConfig.ExcelToJSONConfigManager
                 .Current.GetConfigs<ExcelConfig.MakeConfig> ()
                 .Where (
					                              (t) => {
						switch ((Proto.MakeItemUnlockType)t.UnlockType) {
						case Proto.MakeItemUnlockType.NONE:
							return true;
						case Proto.MakeItemUnlockType.NeedScroll:
							int item = Tools.UtilityTool.ConvertToInt (t.UnlockPars1);
							return DataManagers.PlayerItemManager.Singleton.GetItemCount (item) > 0;
						}
						return false;

					}).ToList ();
				if (makeConfigs.Count == 0) {
					//UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_SCROLL"], 3f);
					UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["NO_SCROLL"]);
					return;
				}
				var ui = UIManager.Singleton.CreateOrGetWindow<UIMake> ();
				ui.ShowWindow ();
			});

			bt_Coin.OnMouseClick ((s, e) => {
				if (IAPExample.Current == null)
					return;
				if (!IAPExample.Current.isActiveAndEnabled)
					return;
				var ui = UIManager.Singleton.CreateOrGetWindow<UIPayment> ();
				ui.ShowWindow ();
			});

			bt_battle.OnMouseClick ((s, e) => {
				if (DataManagers.PlayerArmyManager.Singleton.GetAllSoldier ().Count == 0) {
					//UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_HERO"], 3f);
					UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["NO_HERO"]);
					return;
				}

				var ui = UIManager.Singleton.CreateOrGetWindow<UIGoToExplore> ();
				ui.ShowWindow ();

				UIControllor.Singleton.ClearMaskEvent();

				if(exploreFinger!=null)
				{
					GameObject.Destroy(exploreFinger);
					exploreFinger =null;
				}

				if(exploreCompleted!=null)
				{
					exploreCompleted();
					exploreCompleted =null;
				}
			});

			bt_contruct.OnMouseClick ((s, e) => {
				UIStructureBuilding.Show ();
				UIControllor.Singleton.ClearMaskEvent();
				if(buildCompleted!=null)
				{
					buildCompleted();
					buildCompleted =null;
				}
				if(buildFinger!=null)
				{
					GameObject.Destroy(buildFinger);
					buildFinger =null;
				}

			});

			bt_train.OnMouseClick ((s, e) => {
				if (DataManagers.PlayerArmyManager.Singleton.GetAllSoldier ().Count == 0) {
					//UIControllor.Singleton.ShowMessage(LanguageManager.Singleton["NO_HERO"]);
					UITipDrawer.Singleton.DrawNotify (LanguageManager.Singleton ["NO_HERO"]);
					return;
				}
				UIArmyHouseSelect.Show ();
				//UIArmyHouse.Show();
			});

			fingerEvent.OnLeft = () => {
				ShowOrHideBag (true);
			};
			fingerEvent.OnRight = () => {
				ShowOrHideBag (false);
			};
			bt_Package.OnMouseClick ((s, e) => {
				ShowOrHideBag (true);
			});

			bt_hide.OnMouseClick ((s, e) => {
				ShowOrHideBag (false);
			});

			bt_gold.OnMouseClick ((s, e) => {
				int addNum = 0;
				var cd = DataManagers.GamePlayerManager.Singleton.CallProduceGold (out addNum);
				cdTime = Time.time + cd;
				bt_gold.Disable (true);
				iTween.scaleFrom (lb_gold.gameObject, 0.3f, 0, Vector3.one * 1.3f, iTween.EasingType.spring);
				if (needgold > 0)
				{
					
					needgold -= addNum;
					if(needgold<=0){
						needgold = 0;
						UIControllor.Singleton.ClearMaskEvent();
						if(goldfinger!=null)
						{
							GameObject.Destroy(goldfinger);
							goldfinger = null;
						}
						if(goldCompleted!=null)
							goldCompleted();
						
					}
				}
			});


			char1.OnMousePress ((s, e) => {
				char1.spriteName = e.IsDwon ? "zjmui_jy012" : "zjmui_jy011";
				char1.MakePixelPerfect ();
			});

			char2.OnMousePress ((s, e) => {
				char2.spriteName = e.IsDwon ? "zjmui_jy022" : "zjmui_jy021";
				char2.MakePixelPerfect ();
			});


			char3.OnMousePress ((s, e) => {
				char3.spriteName = e.IsDwon ? "zjmui_jy032" : "zjmui_jy031";
				char3.MakePixelPerfect ();
			});

			char4.OnMousePress ((s, e) => {
				char4.spriteName = e.IsDwon ? "zjmui_jy042" : "zjmui_jy041";
				char4.MakePixelPerfect ();
			});

		}
        private float cdTime = 0f;
        public override void OnShow()
        {
            base.OnShow();
			lb_title.text = LanguageManager.Singleton ["APP_NAME"];
            OnUpdateUIData();
            ShowOrHideBag(false);
			//ShowGoldProcess (20,null);
			//ShowOpenShop(null);
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (cdTime > 0) {
                if (cdTime <= Time.time) {
                    bt_gold.Disable(false);
                    cdTime = -1f;
                }
            }
        }

        private void ShowOrHideBag(bool show)
        {
            var pos = s_bagRoot.GetComponent<TweenPosition>();
            pos.Play(show);
        }
        public override void OnUpdateUIData()
        {
            base.OnUpdateUIData();

            var gold = DataManagers.GamePlayerManager.Singleton.Gold>100000?
                string.Format("{0:0.0}", ((float)DataManagers.GamePlayerManager.Singleton.Gold / 10000f)) + "W" : 
                string.Format("{0:N0}", DataManagers.GamePlayerManager.Singleton.Gold);

            this.lb_gold.text = (string.Format(LanguageManager.Singleton["APP_GOLD_Label"], gold));

            bt_Coin.Text(string.Format(LanguageManager.Singleton["APP_NUM_FORMAT"], DataManagers.GamePlayerManager.Singleton.Coin));
            ShowBag();
          
        }

        private void ShowBag()
        {
			var allItem = DataManagers.PlayerItemManager
				.Singleton.GetAllItems()
				.Where(t=>t.Config.CanSale ==1)
				.ToList();
            this.BagGridTableManager.Count = allItem.Count;
            int index = 0;
            foreach (var i in BagGridTableManager)
            {
                i.Model.GameItem = allItem[index];
                i.Model.SetDrag(allItem.Count >= 11);
                index++;
            }
        }

		#region gold 
		public void ShowGoldProcess(int gold,Action completed)
		{
			if (goldfinger != null) {
				GameObject.Destroy (goldfinger);
			}
			var finger = DataManagers.GuideManager.Singleton.GetFinger ();
			goldfinger = GameObject.Instantiate<GameObject> (finger);
			goldfinger.transform.SetParent (bt_gold.transform);
			goldfinger.transform.localScale = Vector3.one;
			goldfinger.transform.localPosition = new Vector3(40,-40,0);

			needgold = gold;
			goldCompleted = completed;
			UIControllor.Singleton.SetMaskEventObject (bt_gold.gameObject);
		}

		private GameObject goldfinger;
		private int needgold = 0;
		private Action goldCompleted;

		#endregion

		#region OpenShop
		public void ShowOpenShop(Action completed)
		{
			if (shopFinger != null) {
				GameObject.Destroy (shopFinger);
			}
			var finger = DataManagers.GuideManager.Singleton.GetFinger ();
			shopFinger = GameObject.Instantiate<GameObject> (finger);
			shopFinger.transform.SetParent (bt_market.transform);
			shopFinger.transform.localScale = Vector3.one;
			shopFinger.transform.localPosition = new Vector3(-60,-60,0);
			shopFinger.transform.localRotation = Quaternion.Euler (0, 180, 0);
			shopCompleted = completed;
			UIControllor.Singleton.SetMaskEventObject (bt_market.gameObject);
		}

		private GameObject shopFinger;
		private Action shopCompleted;
		#endregion

		public void ShowOpenBuild(Action completed)
		{
			if (buildFinger != null) {
				GameObject.Destroy (buildFinger);
			}
			var finger = DataManagers.GuideManager.Singleton.GetFinger ();
			buildFinger = GameObject.Instantiate<GameObject> (finger);
			buildFinger.transform.SetParent (bt_contruct.transform);
			buildFinger.transform.localScale = Vector3.one;
			buildFinger.transform.localPosition = new Vector3(-40,-40,0);
			buildFinger.transform.localRotation = Quaternion.Euler (0, 180, 0);

			buildCompleted = completed;
			UIControllor.Singleton.SetMaskEventObject (bt_contruct.gameObject);
		}
		private GameObject buildFinger;
		private Action buildCompleted;

		public void ShowOpenProduce(Action completed)
		{
			if (produceFinger != null)
				GameObject.Destroy (produceFinger);
			var finger = DataManagers.GuideManager.Singleton.GetFinger ();
			produceFinger = GameObject.Instantiate<GameObject> (finger);
			produceFinger.transform.SetParent (bt_produce.transform);
			produceFinger.transform.localScale = Vector3.one;
			produceFinger.transform.localPosition = new Vector3(-40,-40,0);
			produceFinger.transform.localRotation = Quaternion.Euler (0, 180, 0);
			produceCompleted = completed;
			UIControllor.Singleton.SetMaskEventObject (bt_produce.gameObject);
		}
		private GameObject produceFinger;
		private Action produceCompleted;


		public void OpenTrave(Action completed)
		{
			
			if (traveFinger != null)
				GameObject.Destroy (traveFinger);
			var finger = DataManagers.GuideManager.Singleton.GetFinger ();
			traveFinger = GameObject.Instantiate<GameObject> (finger);
			traveFinger.transform.SetParent (bt_bar.transform);
			traveFinger.transform.localScale = Vector3.one;
			traveFinger.transform.localPosition = new Vector3(40,-40,0);
			//traveFinger.transform.localRotation = Quaternion.Euler (0, 180, 0);
			traveCompleted = completed;
			UIControllor.Singleton.SetMaskEventObject (bt_bar.gameObject);
		}
		private GameObject traveFinger;
		private Action traveCompleted;


		public void GoToExplore(Action completed)
		{
			
			if (exploreFinger != null)
				GameObject.Destroy (exploreFinger);
			var finger = DataManagers.GuideManager.Singleton.GetFinger ();
			exploreFinger = GameObject.Instantiate<GameObject> (finger);
			exploreFinger.transform.SetParent (bt_battle.transform);
			exploreFinger.transform.localScale = Vector3.one;
			exploreFinger.transform.localPosition = new Vector3(40,-40,0);
			//traveFinger.transform.localRotation = Quaternion.Euler (0, 180, 0);
			exploreCompleted = completed;
			UIControllor.Singleton.SetMaskEventObject (bt_battle.gameObject);
		}
		private GameObject exploreFinger;
		private Action exploreCompleted;
    }
}