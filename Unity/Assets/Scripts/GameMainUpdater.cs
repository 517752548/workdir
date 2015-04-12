using Assets.Scripts.Appliaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameMainUpdater :MonoBehaviour
    {
        void Awake()
        {

        }

        void Update() {
            if(Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
            GameAppliaction.Singleton.Update();
        }

        void Start()
        {
            GameAppliaction.Singleton.Start();
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

        public int ExploreID = -1;


        private string GM = string.Empty;
        void OnGUI()
        {
            
            int line = 1;
            if(GUI.Button(new Rect(10,Screen.height-(line++ *30), 120,25 ),"GoToBattle"))
            {
                GameAppliaction.Singleton.BeginBattle();
            }
            if (GUI.Button(new Rect(10, Screen.height -( line++ * 30), 120, 25), "GoToExplore"))
            {
                GameAppliaction.Singleton.GoToExplore(ExploreID);
            }
            if (GUI.Button(new Rect(10, Screen.height - (line++ * 30), 120, 25), "GoToCastle"))
            {
                GameAppliaction.Singleton.JoinCastle();
            }
            if (GUI.Button(new Rect(10, Screen.height - (line++ * 30), 120, 25), "SaveData"))
            {
                //will save when exit
                GameAppliaction.Singleton.Exit();
            }
            var rect = new Rect(10, Screen.height - (line++ * 30), 120, 25);

            GM = GUI.TextField(rect, GM);
            rect.Set(rect.xMin+120, rect.yMin, rect.width, rect.height);
            if(GUI.Button(rect,"GMSubmit"))
            {
                //处理GM命令
                Assets.Scripts.DataManagers.GMCommTool.Singleton.ExecuteGMComm(GM);
            }
        }


    }
}
