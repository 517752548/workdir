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
        public override void OnEnter()
        {
            base.OnEnter();
            UI.UIControllor.Singleton.ShowOrHideMessage(false);
            UI.UIControllor.Singleton.HideAllUI();
            Map = GameObject.FindObjectOfType<GameMap>();
            var lastPos = DataManagers.GamePlayerManager.Singleton.CurrentPos;
            if (lastPos == null) //中心点
                TargetPos = new Vector2(Map.CurrentMap.Height / 2, Map.CurrentMap.Width / 2);
            else
                TargetPos = lastPos.Value;

            Map.LookAt(TargetPos, true);
            Map.SetZone(4, true);

            UI.Windows.UIExplore.Show();
        }

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

        public override void OnTick()
        {
            base.OnTick();
            if (BState != null)
            {
                BState.OnTick();
            }
            CheckWaiting();
        }

        /// <summary>
        /// configs of pos
        /// </summary>
        public SubMapConfig[] SubMapConfigs { private set; get; }
        
        public void OnChange(Vector2 target)
        {
            //处理回城
            if (Map.IsOrgin(target))
            {
                DataManagers.GamePlayerManager.Singleton.GoPos(null);//回城的时候
                App.GameAppliaction.Singleton.JoinCastle();
            }
            else
            {

                

                //received the onchange event
                if (GRandomer.Probability10000(Config.RandomPro))
                {
                    //出发随机事件
                    var battleID = GRandomer.RandomList(Tools.UtilityTool.SplitIDS(Config.RandomBattle));
                    StartBattle(battleID, (winner) =>
                    {
                        if (winner)
                            DataManagers.GamePlayerManager.Singleton.GoPos(target);

                    });
                    return;
                }
                //记录当前行走点

            }
        }

        public void StartBattle(int battlegroup, Action<bool> callBack)
        {
            //showUI 
            var battleUI = UI.Windows.UIBattle.Show();

            BState = new Combat.Battle.States.BattleState(
             battlegroup,
             battleUI,
             (result) =>
             {
                 BState = null;
                 callBack(result.Winner == Proto.ArmyCamp.Player);
                 //战斗失败处理
                 //Hide 
                 battleUI.HideWindow();
             });
            BState.Start();
        }

        public Combat.Battle.States.BattleState BState { private set; get; }
    }
}