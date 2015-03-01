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
            var package = new Proto.ItemPackage();
            for (var i = 0; i < 1000; i++)
            {
                package.Items.Add(new Proto.Item() { Entry = 1, Num = i });
            }
            byte[] bytes = Tools.ProtoTool.Serialize(package);
            Debug.Log("Length=" + bytes.Length);
            var newp = Tools.ProtoTool.Deserialize<Proto.ItemPackage>(bytes); 
            var user = new Proto.Session() {  UserName ="张三是个大蹦达美", Time = 100000};
            Debug.Log(Tools.ProtoTool.Deserialize<Proto.Session>(Tools.ProtoTool.Serialize(user)).UserName);
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
