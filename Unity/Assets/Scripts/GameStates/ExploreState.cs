using Assets.Scripts.DataManagers;
using Assets.Scripts.Tools;
using ExcelConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Proto;

namespace Assets.Scripts.GameStates
{
	public class ExploreState : App.GameState
	{

		public const string PLAY_RES = "PlayerMap";

		private HashSet<int> _tempPosIndex = new HashSet<int> ();

		public override void OnEnter ()
		{
			base.OnEnter ();
			UI.UIControllor.Singleton.ShowOrHideMessage (false);
			UI.UIControllor.Singleton.HideAllUI ();
			Map = GameObject.FindObjectOfType<GameMap> ();
			Map.InitForExploreState ();
			this.Map.EachAllPosition<MapPosition> ((t) => {
				var open = DataManagers.PlayerMapManager.Singleton.IsOpen (Config.ID, t.ToIndex ());
				t.SetMask (open);
				var isExplored = IsExploredIndex (t.ToIndex ());
				t.SetExplored (isExplored);
				return false;
			});
			var lastPos = DataManagers.GamePlayerManager.Singleton.CurrentPos;
			if (lastPos == null) { //中心点
				TargetPos = Map.Orgin;
			} else {
				TargetPos = lastPos.Value;
			}
			RecordPos (null,TargetPos, true);

			Map.LookAt (TargetPos, true);
			Map.SetZone (4, true);

			player = GameObject.Instantiate<GameObject> (ResourcesManager.Singleton.LoadResources<GameObject> (PLAY_RES));
			lastPos = targetPosPlayer = player.transform.position = Map.GetPositionOfGrid (TargetPos);
			UI.Windows.UIExplore.Show ();
		}

		private GameObject player;

		private Vector2 TargetPos;

		private GameMap Map;

		public override void OnExit ()
		{
			base.OnExit ();
			//sssUI.UIControllor.Singleton.HideAllUI ();
		}

		public ExploreState (MapConfig map)
		{
			Config = map;
			SubMapConfigs = ExcelToJSONConfigManager.Current.GetConfigs<SubMapConfig> ((t) => {
				return t.MapID == map.ID;
			});
		}

		public MapConfig Config { private set; get; }

		private Vector3 lastPos;

		public override void OnPinch (PinchGesture gesture)
		{
			base.OnPinch (gesture);
			float zone = 4;
			if (gesture.State == GestureRecognitionState.Ended) {
				Map.SetZone (zone);
			} else if (gesture.State == GestureRecognitionState.InProgress) {
				
				var target = zone + 20 * (gesture.Gap / 240);
				target = Mathf.Clamp (target, 4, 20);
				Map.SetZone (target);
			}

		}

		public override void OnTap (Vector2 pox)
		{
			base.OnTap (pox);
			if (CheckWaiting ())
				return;


			var org = new Vector2 (Screen.width / 2, Screen.height / 2);
			var d = (pox - org).normalized;
			lastPos = TargetPos;
			if (Mathf.Abs (d.x) > 0.8) {
				if (d.x > 0) {
					TargetPos += new Vector2 (1, 0);
				} else {
					TargetPos += new Vector2 (-1, 0);
				}
			} else if (Mathf.Abs (d.y) > 0.8f) {
				if (d.y > 0) {
					TargetPos += new Vector2 (0, 1);
				} else {
					TargetPos += new Vector2 (0, -1);
				}
			} else {
				return;
			}

			if (!Map.HaveIndex (TargetPos)) {
				TargetPos = lastPos;
				return;
			}

			delayChange = Time.time + Map.LookAt (TargetPos);
			targetPosPlayer = Map.GetPositionOfGrid (TargetPos);

		}

		private float delayChange = -1f;

		private bool CheckWaiting ()
		{
			if (delayChange > 0) {
				if (delayChange <= Time.time) {
					delayChange = -1f;
					OnChange (lastPos, TargetPos);
					return false;
				} else {
					return true;
				}
			}
			return false;
		}

		private Vector3 targetPosPlayer;

		public override void OnTick ()
		{
			base.OnTick ();
			if (BState != null) {
				BState.OnTick ();
				if (BState == null)
					return;
				if (BState.NeedEnd) {
					BState.OnExit ();
					BState = null;
				}
			}
          
			CheckWaiting ();
			if (player == null)
				return;
			player.transform.position = Vector3.Lerp (player.transform.position, targetPosPlayer, 20);
		}

		/// <summary>
		/// configs of pos
		/// </summary>
		public SubMapConfig[] SubMapConfigs { private set; get; }

        
        
		public void OnChange (Vector2 oldPos,Vector2 target)
		{

			if (!Map.HaveIndex (target)) {
				GoBack ();
				return;
			}
			//处理回城
			if (Map.IsOrgin (target)) {
				JoinCastle ();
			} else {
				int index = GamePlayerManager.PosXYToIndex ((int)target.x, (int)target.y);
				foreach (var i in SubMapConfigs) {
					var indexs = Tools.UtilityTool.SplitIDS (i.Posistions);
					for (var p = 0; p < indexs.Count; p++) {
						if (index == indexs [p]) {
							#region haveIndex
							Debug.Log ("P:" + (Proto.MapEventType)i.EventType);
							switch ((Proto.MapEventType)i.EventType) {
							case Proto.MapEventType.BattlePos10:
							case Proto.MapEventType.BattlePos11:
							case Proto.MapEventType.BattlePos2:
							case Proto.MapEventType.BattlePos3:
							case Proto.MapEventType.BattlePos4:
							case Proto.MapEventType.BattlePos5:
							case Proto.MapEventType.BattlePos6:
							case Proto.MapEventType.BattlePos7:
							case Proto.MapEventType.BattlePos8:
							case Proto.MapEventType.BattlePos9:
							case Proto.MapEventType.BattlePos:
								//IS have explored
								if (DataManagers.PlayerMapManager.Singleton.IsExplored (Config.ID, index)) {
									var items = PlayerMapManager.Singleton.GetBattleData (Config.ID, index);
									if (items != null && items.Count > 0) {
										var ui = UI.Windows.UIBattleResult.Show ();
										ui.ShowResult (this.Config.ID, items, index);
										ui.callAfterCollect = SaveBattlePos;
									}
									RecordPos(oldPos, target); 
									return;
								}

								var battleGroups = Tools.UtilityTool.ConvertToInt (i.Pars1);
								if (battleGroups > 0) {
									
									StartBattle (battleGroups, index, (winner) => {
										if (winner) {
											
											PlayerMapManager.Singleton.RecordMap (
												Config.ID, 
												index, true,
												string.Empty, false, true);
											RecordPos (oldPos,target);
										} else {
											GoBack ();
										}
									});
									//return;
								}
								break;
							case Proto.MapEventType.BronPos:
								JoinCastle ();
								break;
							case Proto.MapEventType.GoHomePos:
								JoinCastle ();
								break;
							case Proto.MapEventType.GoToNextLvlPos:
								UI.UIControllor.Singleton.ShowMapListUI ();
								break;
							case Proto.MapEventType.PKEnterPos:
								var pkNeedItem = Tools.UtilityTool.SplitIDS (i.Pars1);
								var pkBattleGroup = Tools.UtilityTool.ConvertToInt (i.Pars2);
								//need items
								if (pkNeedItem.Count == 2) {
									if (PlayerItemManager.Singleton.GetItemCount (pkNeedItem [0]) >= pkNeedItem [1]) {
										PlayerItemManager.Singleton.SubItem (pkNeedItem [0], pkNeedItem [1]);
										if (pkBattleGroup > 0) {
											StartBattle (pkBattleGroup, index, 
												(winner) => {
													if (winner) {
														RecordPos (oldPos,target);
													} else {
														GoBack ();
													}
												});
										}
									} else {

										var pkNeedItemName = string.Empty;
										var configPK = ExcelToJSONConfigManager
										.Current.GetConfigByID<ItemConfig> (pkNeedItem [0]);
										if (configPK != null) {
											pkNeedItemName = configPK.Name;
										}
										//no item
										UI.Windows.UIMessageBox.ShowMessage (
											LanguageManager.Singleton ["EXPLORE_NO_KEY_TITLE"],
											String.Format (LanguageManager.Singleton ["EXPLORE_NO_KEY"], 
												pkNeedItemName, pkNeedItem [1]), 
											() => {
												GoBack ();
											},
											() => {

												GoBack ();
											});
										//GoBack();	
									}
								} else {
									if (pkBattleGroup > 0) {
										StartBattle (pkBattleGroup, index, 
											(winner) => {
												if (winner) {
													RecordPos (oldPos, target);
												} else {
													GoBack ();
												}
											});
									}
								}
								break;
							case Proto.MapEventType.RandomChestPos: //宝箱
								if (PlayerMapManager.Singleton.IsExplored (Config.ID, index)) {
									var items = PlayerMapManager.Singleton.GetChestBoxData (Config.ID, index);
									if (items != null && items.Count > 0) {
										UI.UIControllor.Singleton.ShowChestDialog 
										(
											Config.ID, 
											items, 
											index,
											SaveBattlePos);
									}
									RecordPos (oldPos, target);
									break;
								} else {
									var needitems = Tools.UtilityTool.SplitIDS (i.Pars1);
									var rewardItems = Tools.UtilityTool.SplitIDS (i.Pars2);
									var rewardCounts = Tools.UtilityTool.SplitIDS (i.Pars3);
							    
									if (DataManagers.PlayerItemManager.Singleton.GetItemCount (needitems [0]) >= needitems [1]) {
										var needItem = new List<Proto.Item> ();
										var rewardItemDatas = new List<Proto.Item> ();
										needItem.Add (new Proto.Item{ Entry = needitems [0], Num = needitems [1] });
										for (var tIndex = 0; tIndex < rewardItems.Count; tIndex++) {
											rewardItemDatas.Add (
												new Proto.Item { 
													Entry = rewardItems [tIndex],
													Num = rewardCounts [tIndex]
												});
										}

										foreach (var ni in needItem) {
											DataManagers.PlayerItemManager.Singleton.SubItem (ni.Entry, ni.Num);
										}

										UI.UIControllor.Singleton.ShowChestDialog (
											Config.ID, 
											rewardItemDatas, index,
											SaveChestBoxPos);
										PlayerMapManager.Singleton.RecordMap (Config.ID, index, true, string.Empty, false, true);
										RecordPos (oldPos, target);
									} else {
										var cheshItemName = string.Empty;
										var configChesh = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (needitems [0]);
										if (configChesh != null) {
											cheshItemName = configChesh.Name;
										}
										//no item
										UI.Windows.UIMessageBox.ShowMessage (
											LanguageManager.Singleton ["EXPLORE_NO_KEY_TITLE"],
											String.Format (LanguageManager.Singleton ["EXPLORE_NO_KEY"], 
												cheshItemName, needitems [1]), 
											() => {
												GoBack ();
											},
											() => {

												GoBack ();
											});
									}
								}
								break;
							case Proto.MapEventType.RandomEvnetPos:
								//随机
								if (GRandomer.Probability10000 (Config.RandomPro)) {
									var randmonBattleGroupID = Tools.UtilityTool.ConvertToInt (i.Pars2);
									//RechargePos
									if (randmonBattleGroupID > 0) {
										StartBattle (randmonBattleGroupID, index, 
											(winner) => {
												if (winner) {
													RecordPos (oldPos, target);
												} else {
													GoBack ();
												}
											});
									}
								}
								break;
							case Proto.MapEventType.RechargePos:
								if (_tempPosIndex.Contains (index))
									break;

								var rechargeItems = Tools.UtilityTool.SplitIDS (i.Pars1);
								var rechargeNums = Tools.UtilityTool.SplitIDS (i.Pars2);

								var rechageItemDatas = new List<Proto.Item> ();
								for (var tIndex = 0; tIndex < rechargeItems.Count; tIndex++) {
									rechageItemDatas.Add (new Proto.Item {
										Entry = rechargeItems [tIndex],
										Num = rechargeNums [tIndex]
									});
								}

								UI.UIControllor.Singleton.ShowRechargeUI (
									Config.ID,
									index, 
									rechageItemDatas);
								_tempPosIndex.Add (index);
								RecordPos (oldPos, target);
								break;
							case Proto.MapEventType.ScrectShopPos:
								//OPEN  SHOW 
								var shopID = Tools.UtilityTool.ConvertToInt (i.Pars1);
								UI.UIControllor.Singleton.OpenScrectShop (shopID, Config.ID, index);
								RecordPos (oldPos, target);
								break;

							}

							#endregion
							return;
						}
					}
				}


				//received the onchange event
				if (GRandomer.Probability10000 (Config.RandomPro)) {
					//出发随机事件
					/*var battleID = GRandomer.RandomList (Tools.UtilityTool.SplitIDS (Config.));
					StartBattle (battleID, index, (winner) => {
						if (winner) {
							RecordPos (target);
						} else {
							GoBack ();
						}
					});
					return;*/
				}
				//记录当前行走点

				RecordPos (oldPos, target);
			}
		}


		private void GoBack ()
		{
			TargetPos = lastPos;
			player.transform.position = targetPosPlayer = Map.GetPositionOfGrid (lastPos);
			Map.LookAt (TargetPos);
		}

		private void RecordPos (Vector2? last, Vector2? target, bool isEnter = false)
		{
			DataManagers.GamePlayerManager.Singleton.GoPos (target);
			if (target == null)
				return;


			
			if (!isEnter && !DataManagers.GamePlayerManager.Singleton.CostFood (1)) {
				DataManagers.PlayerItemManager.Singleton.EmptyPackage ();
				var deads = DataManagers.PlayerArmyManager.Singleton.DeadAllSoldiersInTeam ();
				StringBuilder sb = new StringBuilder ();
				foreach (var d in deads) {
					var config = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig> (d);
					if (config == null)
						continue;
					sb.Append (string.Format (LanguageManager.Singleton ["NOFOOD_DEAD"], config.Name));
				}

				JoinCastle ();
				var str = LanguageManager.Singleton ["EXPLORE_NO_FOOD"] + sb.ToString ();
				UI.UIControllor.Singleton.ShowMessage (str);
				UI.UITipDrawer.Singleton.DrawNotify (str);
				return;
			}

			int index = GamePlayerManager.PosXYToIndex ((int)target.Value.x, (int)target.Value.y);
			DataManagers.PlayerMapManager.Singleton.TryToAddExploreValue (Config.ID, index);

			DataManagers.PlayerMapManager.Singleton.OpenClosedIndex (Config.ID,
				index, this.Map);
			
			this.Map.EachAllPosition<MapPosition> ((t) => {
				var open = DataManagers.PlayerMapManager.Singleton.IsOpen (Config.ID, t.ToIndex ());
				var isExplored = IsExploredIndex (t.ToIndex ());
				t.SetExplored (isExplored);
				t.SetMask (open);
				return false;
			});

			UI.UIManager.Singleton.UpdateUIData ();
		}

		private bool IsExploredIndex (int index)
		{
			if (_tempPosIndex.Contains (index))
				return true;
			return DataManagers.PlayerMapManager.Singleton.IsExplored (Config.ID, index);
		}
			

		public void JoinCastle (bool useItem = false)
		{
			if (useItem) {
				var itemID = App.GameAppliaction.Singleton.ConstValues.JoinCastleItemID; 
				var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig> (itemID);
				if (DataManagers.PlayerItemManager.Singleton.GetItemCount (itemID) >= 1) {
					DataManagers.PlayerItemManager.Singleton.SubItem (itemID, 1);
					UI.UITipDrawer.Singleton.DrawNotify (string.Format (LanguageManager.Singleton ["JOIN_CASTLE_ITEM_COST"], config.Name));
				} else {
					UI.UITipDrawer.Singleton.DrawNotify (string.Format (LanguageManager.Singleton ["JOIN_CASTLE_ITEM_NOT_ENOUGH"], config.Name));
					return;
				}
			}
			RecordPos (null,null);//回城的时候
			App.GameAppliaction.Singleton.JoinCastle ();
			PlayerItemManager.Singleton.JoinCastle ();
		}

		private void SaveBattlePos (int mapId, int indexPos, List<Item> list)
		{
			PlayerMapManager.Singleton.SaveBattleIndex (mapId, indexPos, list);
		}

		private void SaveChestBoxPos (int mapID, int indexPos, List<Item> list)
		{
			PlayerMapManager.Singleton.SaveChestBoxIndex (mapID, indexPos, list);
		}

		private  void MonsterDead(int monsterID)
		{
			MapUnlockType type = (MapUnlockType)Config.OpenCondtion;
			if (type == MapUnlockType.KillMonster) {
				var killed = Tools.UtilityTool.ConvertToInt (Config.OpenParams);
				if (killed == monsterID) {
					CompletedMap ();
				}
			}
			//monster dead;
		}

		private void CompletedMap()
		{
			MapUnlockModeType unlockMode = (MapUnlockModeType)Config.UnlockMode;
			switch (unlockMode) {
			case MapUnlockModeType.UnlockMap:
				var unlockMapID = Tools.UtilityTool.ConvertToInt (Config.UnlockParams);
				PlayerMapManager.Singleton.OpenMap (unlockMapID);
				break;
			}

			PlayerMapManager.Singleton.CompletedMap (this.Config.ID);
		}

		public void StartBattle (int battlegroup, int index, Action<bool> callBack)
		{
			//var group = ExcelToJSONConfigManager.Current.GetConfigByID<BattleGroupConfig> (battlegroup);
			var battleIndex = 0;
			//var battleIndex = DataManagers.PlayerMapManager.Singleton.GetBattleIndex(this.Config.ID, index);

#region OK
			Action ok = () => {
				var battleUI = UI.Windows.UIBattle.Show ();
				battleUI.OnMonsterDeadCallBack = MonsterDead;
				var soldiers = DataManagers.PlayerArmyManager.Singleton.GetTeam ();
				BState = new Combat.Battle.States.BattleState (
					battlegroup,
					battleUI,
					battleIndex,
					soldiers,
					(result) => {
						SoundManager.Singleton.PlaySound ("battle_complete");
						callBack (result.Winner == Proto.ArmyCamp.Player);
						if (result.Winner == Proto.ArmyCamp.Player) {

							if (result.DropGold > 0) {						
								GamePlayerManager.Singleton.AddGold (result.DropGold);	
								UI.UITipDrawer.Singleton.DrawNotify (
									string.Format (LanguageManager.Singleton ["Battle_End_Reward_Gold"], result.DropGold));
							}

							//战斗
							if (result.DropList.Count > 0) {
								var ui = UI.Windows.UIBattleResult.Show ();
								ui.ShowResult (this.Config.ID, result.DropList, index);
								ui.callAfterCollect = SaveBattlePos;
							}
						}
						if (result.Dead) {
							PlayerItemManager.Singleton.EmptyPackage ();
							var deads = DataManagers.PlayerArmyManager.Singleton.DeadAllSoldiersInTeam ();
							StringBuilder sb = new StringBuilder ();
							foreach (var d in deads) {
								var config = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig> (d);
								if (config == null)
									continue;
								sb.Append (string.Format (LanguageManager.Singleton ["BATTLE_DEAD"], config.Name));
							}
							JoinCastle ();
							RecordPos (null, null);
							var str = LanguageManager.Singleton ["EXPLORE_BATTLE_F"] + sb.ToString ();
							UI.UIControllor.Singleton.ShowMessage (str);
							UI.UITipDrawer.Singleton.DrawNotify (str);
						}
						//战斗失败处理
						//Hide 
						battleUI.HideWindow ();
					});
				BState.Start ();
			};
#endregion
			ok ();

			// UI.Windows.UIMessageBox.ShowMessage(group.Name, group.battleDiscribtion, ok, () => { callBack(false); });
			//showUI 
			//On 
           
		}

		public Combat.Battle.States.BattleState BState { private set; get; }
	}
}