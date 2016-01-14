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

		private  LinkedList<float> frames = new LinkedList<float> ();


        void Update()
        {
			frames.AddLast (1 / Time.deltaTime);
			if (frames.Count > 200) {
				frames.RemoveFirst ();

			}

			if (lastTime + 1 > Time.time) {
				pfs++;
			} else {
				fps = pfs;
				pfs = 0;
				lastTime = Time.time;
			}
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

		private float pfs = 0;
		private float fps = 0;
		private float lastTime = 0;


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


			rect =new Rect(2, Screen.height - (line++ * 20), 120, 20);
			GUI.Label( rect,string.Format("fps:{0:0} {1:0.00}",fps,1/Time.deltaTime));
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

		public void DrawFrame()
		{
			

		}
    }
}
