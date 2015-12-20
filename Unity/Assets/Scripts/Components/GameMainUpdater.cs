using Assets.Scripts.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameMainUpdater : MonoBehaviour,IRuner
    {
        void Awake()
        {
            GM = PlayerPrefs.GetString("GM");
            GM = GM == null ? "" : GM;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
            GameAppliaction.Singleton.Update();
        }

        void Start()
        {
            GameAppliaction.Singleton.Start(this);
        }

        void LateUpdate()
        {
            GameAppliaction.Singleton.LateUpdate();
        }

        void FixedUpdate()
        {
            GameAppliaction.Singleton.FixedUpdate();
        }

        void OnApplicationPause(bool isPause)
        {
            if (isPause)
                GameAppliaction.Singleton.Pause();
            else
                GameAppliaction.Singleton.Ruseme();
        }

        void OnApplicationQuit()
        {
            GameAppliaction.Singleton.Exit();
        }

        public int ExploreID = 1;

        private string GM = string.Empty;

        void OnGUI()
        {
            int line = 1;
            var rect = new Rect(2, Screen.height - (line++ * 20), 120, 20);
            GM = GUI.TextField(rect, GM);
            rect.Set(rect.xMin + 120, rect.yMin, rect.width, rect.height);
            if (GUI.Button(rect, "GMSubmit"))
            {
                //处理GM命令
                Assets.Scripts.DataManagers.GMCommTool.Singleton.ExecuteGMComm(GM);
                PlayerPrefs.SetString("GM", GM);
            }

            GameAppliaction.BattleDebug = GUI.Toggle(new Rect(2, Screen.height - (line++ * 20), 120, 20),
                GameAppliaction.BattleDebug, "Show Battle Log");
        }

        void OnGameTap(TapGesture tap)
        {
            if (UICamera.hoveredObject) return;
            GameAppliaction.Singleton.OnTap(tap);
        }

        public void DoRun(System.Collections.IEnumerator r)
        {
            this.StartCoroutine(r);
        }
    }
}
