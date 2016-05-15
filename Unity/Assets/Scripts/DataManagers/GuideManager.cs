using System;
using UnityEngine;
using Assets.Scripts.App;

namespace Assets.Scripts.DataManagers
{

	public enum GuideStep
	{
		Welcome,
		ProduceGold,
		ProduceGold2,

		BuildHouse,
		BuildHouse_BuyWood,
		BuildHouse_BuildOpen,
		BuildHouse_BuildHouse,

		BuildFort,
		BuildFort_Produce_Gold,
		BuildFort_Shop_Open,
		BuildFort_Buy_Item,
		BuildFort_BuildOpen, //--
		BuildFort_Build,


		ProduceWheat,
		ProduceWheat_Add,

		ProduceFood,
		ProduceFood_Produce_Gold,
		ProduceFood_BuyWood,
		ProduceFood_Build,
		ProduceFood_Add,

		EmployHero,
		EmployHero_Employ,

		GoToExplore,
		BeginExplore,

		Completed = 10000
	}

	public class GuideManager:Tools.XSingleton<GuideManager>
	{
		
		public GuideManager ()
		{
			
		}

		public GuideStep CurrentStep
		{
			get{ 
				int step = GamePlayerManager.Singleton [PlayDataKeys.GuideStep];
				if (step < 0) {
					return GuideStep.Welcome;
				}
				return (GuideStep)step;
			}

			set
			{
				GamePlayerManager.Singleton [PlayDataKeys.GuideStep] = (int)value;
			}
		}

		public bool IsCompleted(GuideStep step)
		{
			if((int)this.CurrentStep >= (int)step) return true;
			return false;
		}

		public const string FingerProcess = "UIGuideWindow";


		public GameObject GetFinger()
		{
			return Resources.Load<GameObject> (FingerProcess);
		}

		public void ShowGuild()
		{
			switch (this.CurrentStep) {
			case GuideStep.Welcome:
				//GUILD_WELCOME
				UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_WELCOME"], 5);
				UI.UIControllor.Singleton.MaskEventObjectName = "GUILD_WELCOME";
				App.GameAppliaction.Singleton.DelayCall(
					()=>
					{	
						UI.UIControllor.Singleton.ClearMaskEvent();
						this.CurrentStep = GuideStep.ProduceGold;
						ShowGuild();
					},1.5f);
				break;
			case GuideStep.ProduceGold:
				{
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					//GUILD_GOLD
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_GOLD"], 105);
					castle.ShowGoldProcess (1, 
						() => {
							this.CurrentStep = GuideStep.ProduceGold2;
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ShowGuild ();
						});
				}
				break;
			case GuideStep.ProduceGold2:
				{
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					//GUILD_GOLD
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_GOLD2"], 105);
					castle.ShowGoldProcess (2, 
						() => {
							this.CurrentStep = GuideStep.BuildHouse;
							GamePlayerManager.Singleton.AddGold(22);
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ShowGuild ();
						});
				}
				break;
			case GuideStep.BuildHouse:
				{
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildHouse"], 105);
					castle.ShowOpenShop (() => {
						CurrentStep = GuideStep.BuildHouse_BuyWood;
						ShowGuild();
					});
					//ShowBuyItem (7, 10, ShowType.Gold, null);
				}
				break;
			case GuideStep.BuildHouse_BuyWood:
				{
					var shop = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIShop> ();
					if (shop == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildHouse_BuyWood"], 105);
					shop.ShowBuyItem (7, 1, Assets.Scripts.UI.Windows.UIShop.ShowType.Gold,
						() => {
							this.CurrentStep = GuideStep.BuildHouse_BuildOpen;
							shop.HideWindow();
							ShowGuild();
					});
				}
				break;
			case GuideStep.BuildHouse_BuildOpen:
				{
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildHouse_BuildOpen"], 105);
					castle.ShowOpenBuild (() => {
						CurrentStep = GuideStep.BuildHouse_BuildHouse;
						ShowGuild();
					});
				}
				break;

			case GuideStep.BuildHouse_BuildHouse:
				{
					//ShowStructuerBuild
					//GUILD_BuildHouse_BuildHouse
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIStructureBuilding>();
					if (ui == null)
						return;
					
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildHouse_BuildHouse"], 105);
					ui.ShowStructuerBuild (8, 
						() => {
							CurrentStep = GuideStep.BuildFort; 
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ui.HideWindow();
							ShowGuild();
					});
				}
				break;
			case GuideStep.BuildFort:
				//GUILD_BuildFort
				UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildFort"], 105);
				UI.UIControllor.Singleton.MaskEventObjectName = "GUILD_BuildFort";
				App.GameAppliaction.Singleton.DelayCall(
					()=>
					{	
						UI.UIControllor.Singleton.ClearMaskEvent();
						this.CurrentStep = GuideStep.BuildFort_Produce_Gold;
						ShowGuild();
					},1f);
				break;
			case GuideStep.BuildFort_Produce_Gold:
				{
					//GUILD_BuildFort_Produce_Gold
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildFort_Produce_Gold"], 105);
					castle.ShowGoldProcess (2, 
						() => {
							GamePlayerManager.Singleton.AddGold(20);
							this.CurrentStep = GuideStep.BuildFort_Shop_Open;
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ShowGuild ();
						});
				}
				break;
			case GuideStep.BuildFort_Shop_Open:
				{
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildFort_Shop_Open"], 105);
					castle.ShowOpenShop (() => {
						CurrentStep = GuideStep.BuildFort_Buy_Item;
						ShowGuild();
					});
					//ShowBuyItem (7, 10, ShowType.Gold, null);
				}
				break;

			case GuideStep.BuildFort_Buy_Item:
				{
					//GUILD_BuildFort_Buy_item
					var shop = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIShop> ();
					if (shop == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildFort_Buy_item"], 105);
					shop.ShowBuyItem (168, 1, Assets.Scripts.UI.Windows.UIShop.ShowType.Gold,
						() => {
							this.CurrentStep = GuideStep.BuildFort_BuildOpen;
							shop.HideWindow();
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ShowGuild();
						});
				}
				break;
			case  GuideStep.BuildFort_BuildOpen:
				{
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildFort_OpenBuild"], 105);
					castle.ShowOpenBuild (() => {
						CurrentStep = GuideStep.BuildFort_Build;
						ShowGuild();
					});
				}
				break;
			case GuideStep.BuildFort_Build:
				{
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIStructureBuilding>();
					if (ui == null)
						return;

					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_BuildFort_Build"], 105);
					ui.ShowStructuerBuild (2, 
						() => {
							CurrentStep = GuideStep.ProduceWheat; 
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ui.HideWindow();
							ShowGuild();
						});
				}
				break;
			case GuideStep.ProduceWheat:
				{
					//GUILD_Produce_Wheat
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_Produce_Wheat"], 105);
					castle.ShowOpenProduce (() => {
						CurrentStep = GuideStep.ProduceWheat_Add;
						UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
						ShowGuild();
					});
				}
				break;
			case GuideStep.ProduceWheat_Add:
				{
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIProducePanel> ();
					if (ui == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_Produce_Wheat_Add"], 105);
					ui.GuideShowAddPeople(1,5,()=>{
						CurrentStep = GuideStep.ProduceFood;
						UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
						ui.HideWindow();
						ShowGuild();
					});

				}
				break;
			case GuideStep.ProduceFood:
				{
					UI.UIControllor.Singleton.MaskEventObjectName="GUILD_Produce_Food";
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GUILD_Produce_Food"], 4);
					GameAppliaction.Singleton.DelayCall (() => {
						this.CurrentStep = GuideStep.ProduceFood_Produce_Gold;
						ShowGuild();
					}, 3f);
				}
				break;
			case GuideStep.ProduceFood_Produce_Gold:
				{
					//40
					var castle = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (castle == null)
						return;
					//GUILD_GOLD
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["ProduceFood_Produce_Gold"], 105);
					castle.ShowGoldProcess (2, 
						() => {
							GamePlayerManager.Singleton.AddGold(60);
							this.CurrentStep = GuideStep.ProduceFood_BuyWood;
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							UI.Windows.UIShop.Show();
							ShowGuild ();
						});
				}
				break;
			case GuideStep.ProduceFood_BuyWood:
				{
					var shop = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIShop> ();
					if (shop == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["ProduceFood_BuyWood"], 105);
					shop.ShowBuyItem (7, 4, Assets.Scripts.UI.Windows.UIShop.ShowType.Gold,
						() => {
							this.CurrentStep = GuideStep.ProduceFood_Build;
							shop.HideWindow();
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							UI.Windows.UIStructureBuilding.Show();
							ShowGuild();
						});
				}
				break;
			case GuideStep.ProduceFood_Build:
				{
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIStructureBuilding>();
					if (ui == null)
						return;

					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["ProduceFood_Build"], 105);
					ui.ShowStructuerBuild (1, 
						() => {
							CurrentStep = GuideStep.ProduceFood_Add; 
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ui.HideWindow();
							UI.Windows.UIProducePanel.Show();
							ShowGuild();
						}); 
				}
				break;
			case GuideStep.ProduceFood_Add:
				{
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIProducePanel> ();
					if (ui == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["ProduceFood_Add"], 105);
					ui.GuideShowAddPeople(2,3,()=>{
						CurrentStep = GuideStep.EmployHero;
						UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
						ui.HideWindow();
						ShowGuild();
					});
				}
				break;
			case GuideStep.EmployHero:
				{
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel> ();
					if (ui == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["EmployHero"], 100);
					ui.OpenTrave (() => 
						{
						this.CurrentStep = GuideStep.EmployHero_Employ;
						UI.UIControllor.Singleton.ShowInfo("",0.1f);
						ShowGuild();
					});
				}
				break;
			case GuideStep.EmployHero_Employ:
				{
					//employ
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UITavern>();
					if (ui == null)
						return;
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["EmployHero_Employ"], 100);
					ui.EmployHero (245, () => {
						CurrentStep = GuideStep.GoToExplore;
						PlayerItemManager.Singleton.AddItem(158,30);
						DataManagers.GamePlayerManager.Singleton.AddFood(20);
						DataManagers.PlayerArmyManager.Singleton.SetTeam(new System.Collections.Generic.List<int>{245});
						ui.HideWindow();
						ShowGuild();
					});
					//EmployHero
				}
				break;
			case GuideStep.GoToExplore:
				{
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UICastlePanel>();
					if (ui == null)
						return;

					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["GoToExplore"], 105);
					ui.GoToExplore ( 
						() => {
							CurrentStep = GuideStep.BeginExplore; 
							UI.UIControllor.Singleton.ShowInfo ("", 0.1f);
							ShowGuild();
						}); 
				}
				break;

			case GuideStep.BeginExplore:
				{
					var ui = UI.UIManager.Singleton.GetUIWindow<UI.Windows.UIGoToExplore> ();
					if (ui == null)
						return;
					
					UI.UIControllor.Singleton.ShowInfo (LanguageManager.Singleton ["BeginExplore"], 1);

					ui.ShowGuide (() => {
						CurrentStep = GuideStep.Completed;
						ShowGuild();
						//UI.UIControllor.Singleton.ShowInfo("",0.1f);
					});
				}
				break;
			}
		}
	}
		
}

