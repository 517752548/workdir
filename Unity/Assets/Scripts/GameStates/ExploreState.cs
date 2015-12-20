using Assets.Scripts.DataManagers;
using Assets.Scripts.Tools;
using ExcelConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class ExploreState : App.GameState
    {

        public const string PLAY_RES = "PlayerMap";

        public override void OnEnter()
        {
            base.OnEnter();
            UI.UIControllor.Singleton.ShowOrHideMessage(false);
            UI.UIControllor.Singleton.HideAllUI();
            Map = GameObject.FindObjectOfType<GameMap>();
            Map.InitForExploreState();

            var lastPos = DataManagers.GamePlayerManager.Singleton.CurrentPos;
            if (lastPos == null) //中心点
                TargetPos = Map.Orgin;
            else
                TargetPos = lastPos.Value;

            Map.LookAt(TargetPos, true);
            Map.SetZone(4, true);

            player = GameObject.Instantiate<GameObject>(ResourcesManager.Singleton.LoadResources<GameObject>(PLAY_RES));
            lastPos = targetPosPlayer = player.transform.position = Map.GetPositionOfGrid(TargetPos);
            UI.Windows.UIExplore.Show();
        }

        private GameObject player;

        private Vector2 TargetPos;

        private GameMap Map;

        public override void OnExit()
        {
            base.OnExit();
        }

        public ExploreState(MapConfig map)
        {
            Config = map;
            SubMapConfigs = ExcelToJSONConfigManager.Current.GetConfigs<SubMapConfig>((t) => { return t.MapID == map.ID; });
        }

        public MapConfig Config { private set; get; }

        private Vector3 lastPos;

        public override void OnTap(Vector2 pox)
        {
            base.OnTap(pox);
            if (CheckWaiting()) return;
            Debug.Log("V:" + pox);
            var org = new Vector2(Screen.width / 2, Screen.height / 2);
            var distance = Vector2.Distance(org, pox);
            var d = (pox - org).normalized;
            Debug.Log("D:" + d);
            //TargetPos += Vector2.up;
            lastPos = TargetPos;
            if (Mathf.Abs(d.x) > 0.8)
            {
                if (d.x > 0)
                {
                    TargetPos += new Vector2(1, 0);
                }
                else
                {
                    TargetPos += new Vector2(-1, 0);
                }
            }
            else if (Mathf.Abs(d.y) > 0.8f)
            {
                if (d.y > 0)
                {
                    TargetPos += new Vector2(0, 1);
                }
                else
                {
                    TargetPos += new Vector2(0, -1);
                }
            }
            else
            {
                return;
            }

            delayChange = Time.time + Map.LookAt(TargetPos);
            targetPosPlayer = Map.GetPositionOfGrid(TargetPos);
            Debug.Log("Target:" + TargetPos);
        }

        private float delayChange = -1f;
        private bool CheckWaiting()
        {
            if (delayChange > 0)
            {
                if (delayChange < Time.time)
                {
                    delayChange = -1f;
                    OnChange(TargetPos);
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private Vector3 targetPosPlayer;

        public override void OnTick()
        {
            base.OnTick();
            if (BState != null)
            {
                BState.OnTick();
                if (BState == null) return;
                if(BState.NeedEnd)
                {
                    BState.OnExit();
                    BState = null;
                }
            }
          
            CheckWaiting();
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosPlayer, 20);
        }

        /// <summary>
        /// configs of pos
        /// </summary>
        public SubMapConfig[] SubMapConfigs { private set; get; }

        
        
        public void OnChange(Vector2 target)
        {

            if (!Map.HaveIndex(target)) {
                GoBack();
                return;
            }
            //处理回城
            if (Map.IsOrgin(target))
            {
                JoinCastle();
            }
            else
            {
                int index = GamePlayerManager.PosXYToIndex((int)target.x, (int)target.y);
                foreach (var i in SubMapConfigs)
                {
                    var indexs = Tools.UtilityTool.SplitIDS(i.Posistions);
                    for (var p = 0; p < indexs.Count; p++)
                    {
                        if (index == indexs[p])
                        {
                            #region haveIndex
                            switch ((Proto.MapEventType)i.EventType)
                            {
                                case Proto.MapEventType.BattlePos:
                                    var battleGroups = Tools.UtilityTool.SplitIDS(i.Pars1);
                                    if (battleGroups.Count > p)
                                    {
                                        StartBattle(battleGroups[p], index,(winner) =>
                                        {
                                            if (winner)
                                            {
                                                RecordPos(target);
                                            }
                                            else
                                            {
                                                GoBack();
                                            }
                                        });
                                    }
                                    break;
                                case Proto.MapEventType.BronPos:
                                    break;
                            }

                            #endregion
                            return;
                        }
                    }
                }
                //received the onchange event
                if (GRandomer.Probability10000(Config.RandomPro))
                {
                    //出发随机事件
                    var battleID = GRandomer.RandomList(Tools.UtilityTool.SplitIDS(Config.RandomBattle));
                    StartBattle(battleID,index, (winner) =>
                    {
                        if (winner)
                        {
                            RecordPos(target);
                        }
                        else {
                            GoBack();
                        }
                    });
                    return;
                }
                //记录当前行走点

                RecordPos(target);
            }
        }


        private void GoBack()
        {
            TargetPos = lastPos ;
            player.transform.position = targetPosPlayer = Map.GetPositionOfGrid(lastPos);
            Map.LookAt(TargetPos);
        }

        private void RecordPos(Vector2? target)
        {
            DataManagers.GamePlayerManager.Singleton.GoPos(target);
        }

        public void JoinCastle(bool useItem = false)
        {
            if (useItem)
            {
                var itemID = App.GameAppliaction.Singleton.ConstValues.JoinCastleItemID; 
                var config = ExcelToJSONConfigManager.Current.GetConfigByID<ItemConfig>(itemID);
                if (DataManagers.PlayerItemManager.Singleton.GetItemCount(itemID) >= 1)
                {
                    DataManagers.PlayerItemManager.Singleton.SubItem(itemID, 1);
                    UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["JOIN_CASTLE_ITEM_COST"], config.Name));
                }
                else
                {
                    UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["JOIN_CASTLE_ITEM_NOT_ENOUGH"],config.Name));
                    return;
                }
            }
            RecordPos(null);//回城的时候
            App.GameAppliaction.Singleton.JoinCastle();
        }

        public void StartBattle(int battlegroup, int index, Action<bool> callBack)
        {
            var group = ExcelToJSONConfigManager.Current.GetConfigByID<BattleGroupConfig>(battlegroup);
            var battleIndex = DataManagers.PlayerMapManager.Singleton.GetBattleIndex(this.Config.ID, index);

#region OK
             Action ok = () => {
                var battleUI = UI.Windows.UIBattle.Show();
                var soldiers = DataManagers.PlayerArmyManager.Singleton.GetTeam();
                 BState = new Combat.Battle.States.BattleState(
                 battlegroup,
                 battleUI,
                 battleIndex,
                 soldiers,
                 (result) =>
                 {
                     callBack(result.Winner == Proto.ArmyCamp.Player);
                     //战斗失败处理
                     //Hide 
                     battleUI.HideWindow();
                 });
                BState.Start();
            };
#endregion
             UI.Windows.UIMessageBox.ShowMessage(group.Name, group.battleDiscribtion, ok, () => { callBack(false); });
            //showUI 
            //On 
           
        }

        public Combat.Battle.States.BattleState BState { private set; get; }
    }
}