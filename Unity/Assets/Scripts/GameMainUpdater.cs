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
        void Awake() { }

        void Update() {
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

        void OnAppliactionPause(bool isPause)
        {
            if (isPause)
                GameAppliaction.Singleton.Pause();
            else
                GameAppliaction.Singleton.Ruseme();
        }

        void OnAppliactionQuit()
        {
            GameAppliaction.Singleton.Exit();
        }

        public int ExploreID = -1;
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
        }
    }
}
