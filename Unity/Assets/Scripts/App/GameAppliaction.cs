using Assets.Scripts.Components;
using Assets.Scripts.DataManagers;
using Assets.Scripts.GameStates;
using Assets.Scripts.Tools;
using ExcelConfig;
using org.vxwo.csharp.json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//using UnityEngine.SceneManagement;

namespace Assets.Scripts.App
{
    public class GameAppliaction:Tools.XSingleton<GameAppliaction>,ExcelConfig.IConfigLoader
    {

		public const string APP_CONFIG = "AppConfig.xml";

		public static bool BattleDebug = true;

        public void Start(IRuner runer)
        {
            //初始化配置
            new ExcelToJSONConfigManager(this);
            prisit = new List<IPresist>(){
                PlayerItemManager.Singleton,
                GamePlayerManager.Singleton,
                BuildingManager.Singleton,
                PlayerArmyManager.Singleton,
                PlayerMapManager.Singleton,
				AchievementManager.Singleton
            };
            foreach (var i in prisit)
                i.Load();
            //初始化道具
            Debug.Log(LanguageManager.Singleton["APP_NAME"]);
            //进入游戏

            var pos = GamePlayerManager.Singleton.CurrentPos;
            if (pos != null)
            {
                GoToExplore(GamePlayerManager.Singleton.CurrentMap);
            }
            else
            {
                  JoinCastle();
            }
            Runner = runer;

			if (!GamePlayerManager.Singleton.EffectOn) {
				SoundManager.Singleton.SetSourceValue (0);
			}
            Runner.DoRun(Run());

			//UI.UIManager.Singleton.LoadAllResources ();
        }

        public void ResetPlayData()
        {
            foreach (var i in prisit)
            {
                i.Reset();
            }

            DoLogin();
        }


        public IRuner Runner { private set; get; }

        private List<IPresist> prisit;

        public ConstAttConfig ConstValues
        {
            get
            {

                return ExcelToJSONConfigManager.Current.GetConfigByID<ConstAttConfig>(1);
            }
        }

        public void Exit() 
        {
            DoSave();
        }

        private void DoSave()
        {
            foreach (var i in prisit)
                i.Presist();
        }
        public void Pause() 
        {
            DoSave();
        }
        public void Ruseme() { }

        public void Update() {
            if (Current != null)
                Current.OnTick();
        }

        public void LateUpdate()
        {
            if(GamePlayerManager.Singleton.TimeToProduce.TotalSeconds<=0)
            {
                GamePlayerManager.Singleton.CalProduce();
            }
        }

        public void FixedUpdate() { }

        public GameState Current { private set; get; }

        public void ChangeState(GameState target) {
            if (Current != null) 
                Current.OnExit();
		
            Current = target;
			if(Current!=null)
            Current.OnEnter();

			DataManagers.GuideManager.Singleton.	ShowGuild ();
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        public void BeginBattleTest()
        {
			Application.LoadLevel ("Castle");//.LoadScene("Castle", LoadSceneMode.Single);
            var battleState = new TestBattleState();
            ChangeState(battleState);
        }

        public void DoLogin()
        {
            var state = new LoginState();
            ChangeState(state);
        }

        /// <summary>
        /// 进入城堡
        /// </summary>
        public void JoinCastle() 
        {
            //SceneManager.LoadScene("Castle",LoadSceneMode.Single);
            Application.LoadLevel("Castle");

            var state = new CastleState();
            ChangeState(state);
        }

        /// <summary>
        /// 开始探索
        /// </summary>
        public void GoToExplore(int configID)
        {
			ChangeState (null);
            var r = DoGoToExplore(configID);
            works.Enqueue(r);
        }



        private IEnumerator DoGoToExplore(int configID)
        {
            var map = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MapConfig>(configID);
			var run = Application.LoadLevelAsync (map.MapResName);  //SceneManager.LoadSceneAsync(map.MapResName);
            while (!run.isDone)
                yield return null;
            yield return null;
            var state = new ExploreState(map);
            ChangeState(state);
        }

        /// <summary>
        /// 读取stream 目录下文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
		public static string ReadStreamingFile(string fileName)
        {
            var fileFullName = Tools.Utility.GetStreamingAssetByPath(fileName);
            Debug.Log(fileFullName);
            return Tools.Utility.ReadAStringFile(fileFullName);
        }
        public void ReloadConfig()
        {
            new ExcelToJSONConfigManager(this);
        }

        public void SaveFile(string path, string content, bool append = false)
        {
            var presistPath = Tools.Utility.GetPersistentPath(path);
            Tools.Utility.SaveTextFileToPersistentPath(presistPath,content, append);
        }

        public string ReadFile(string path)
        {
            return Tools.Utility.ReadTextFileFromPersistentPath(path);
        }

        public List<T> Deserialize<T>() where T : ExcelConfig.JSONConfigBase
        {
            var type = typeof(T);
            var atts = type.GetCustomAttributes(typeof(ExcelConfig.ConfigFileAttribute), false) as ExcelConfig.ConfigFileAttribute[];
            if(atts.Length>0)
            {
                var json = ReadStreamingFile("/Json/" + atts[0].FileName);
                return JsonTool.Deserialize<List<T>>(json);
            }

            return null;
        }

        public void OnTap(TapGesture gesture)
        {
            if (Current == null) return;
            Current.OnTap(gesture.Position);
        }

		public void OnPinch(PinchGesture gesture)
		{
			if (Current == null)
				return;
			Current.OnPinch (gesture);
		}

        public Queue<IEnumerator> works = new Queue<IEnumerator>();

        public IEnumerator Run()
        {
           
            while(true)
            {
                while (works.Count > 0)
                {
                    var w = works.Dequeue();
					this.Runner.DoRun (w);
                }
                yield return null;
            }
        }

		private IEnumerator DelayRuner(float delay,Action call)
		{
			yield return new WaitForSeconds (delay);
			call ();
		}

		public void DelayCall(Action action,float time)
		{
			var r = DelayRuner (time, action);
			works.Enqueue(r);
		}
    }

    public interface IRuner
    {
        void DoRun(IEnumerator r);
    }
}
