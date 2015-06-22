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
            UI.UIControllor.Singleton.HideAllUI();
            UI.UIControllor.Singleton.ShowOrHideUIBackground(false);

            Map = GameObject.FindObjectOfType<GameMap>();
            TargetPos = new Vector2(Map.CurrentMap.Height / 2, Map.CurrentMap.Width / 2);
            Map.LookAt(TargetPos, true); 
            Map.SetZone(4,true);
            
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
        }

        public MapConfig Config { private set; get; }

        public override void OnTap(Vector2 pox)
        {
            base.OnTap(pox);
            Debug.Log("V:" + pox);
            var org = new Vector2(Screen.width / 2, Screen.height / 2);
            var distance = Vector2.Distance(org, pox);
            var d = (pox - org).normalized;
            Debug.Log("D:" + d);
            //TargetPos += Vector2.up;

            if(Mathf.Abs(d.x)>0.8)
            {
                if(d.x>0)
                {
                    TargetPos += new Vector2(1, 0);
                }else
                {
                    TargetPos += new Vector2(-1, 0);
                }
            }
            else if(Mathf.Abs(d.y)>0.8f)
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

            OnChange(TargetPos);
            Map.LookAt(TargetPos);
           // Debug.Log("Target:" + TargetPos);
        }

        public void OnChange(Vector2 target)
        {
            //
        }
    }
}
