using Assets.Scripts.DataManagers;
using Assets.Scripts.GameStates;
using ExcelConfig;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Appliaction
{
    public class GameAppliaction:Tools.XSingleton<GameAppliaction>,ExcelConfig.IConfigLoader
    {
        public void Start()
        {
            //初始化配置
            var excelConfig = new ExcelToJSONConfigManager(this);
            foreach (var i in excelConfig.GetConfigs<ExcelConfig.ItemConfig>())
            {
                Debug.Log(i.Name);
            }
            //初始化道具
            PlayerItemManager.Singleton.Load();
            GamePlayerManager.Singleton.Load();
            Debug.Log(LanguageManager.Singleton["APP_NAME"]);

            JoinCastle();
           
        }

        private ConstValuesConfig _ConstValues;
        public ConstValuesConfig ConstValues
        {
            get
            {
                if (_ConstValues == null)
                    _ConstValues = ExcelToJSONConfigManager.Current.FirstConfig<ConstValuesConfig>((item) => { return true; });
                return _ConstValues;
            }
        }

        public void Exit() 
        {
            PlayerItemManager.Singleton.Presist();
            GamePlayerManager.Singleton.Presist();
        }
        public void Pause() { }
        public void Ruseme() { }

        public void Update() {
            if (Current != null)
                Current.OnTick();
        }

        public void LateUpdate()
        {
        }

        public void FixedUpdate() { }

        public GameState Current { private set; get; }

        public void ChangeState(GameState target) {
            if (Current != null) Current.OnExit();
            Current = target;
            Current.OnEnter();
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        public void BeginBattle()
        {
            var player = new Proto.Army() { 
             Camp = Proto.ArmyCamp.Player
            };
            var monster = new Proto.Army() {
                Camp = Proto.ArmyCamp.Monster
            };
            player.Soldiers.Add(new Proto.Soldier {  ConfigID = 1, Num =10});
            monster.Soldiers.Add(new Proto.Soldier { Num = 10, ConfigID = 2 });
            var state = new GameStates.BattleState(player, monster);
            ChangeState(state);
        }

        /// <summary>
        /// 进入城堡
        /// </summary>
        public void JoinCastle() 
        {
            var state = new CastleState();
            ChangeState(state);
        }

        /// <summary>
        /// 开始探索
        /// </summary>
        public void GoToExplore(int configID) {

            var state = new ExploreState();
            ChangeState(state);
        }

        /// <summary>
        /// 读取stream 目录下文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ReadStreamingFile(string fileName)
        {
            var fileFullName = Tools.Utility.GetStreamingAssetByPath(fileName);
            Debug.Log(fileFullName);
            return Tools.Utility.ReadAStringFile(fileFullName);
        }
        public void ReloadConfig()
        {
            _ConstValues = null;
            new ExcelToJSONConfigManager(this);
        }

        public void SaveFile(string path, string content, bool append = false)
        {
            var presistPath = Tools.Utility.GetPersistentPath(path);
            Tools.Utility.SaveTextFileToPersistentPath(presistPath,content, append);
        }

        public string ReadFile(string path)
        {
            var presistPath = Tools.Utility.GetPersistentPath(path);
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

        internal void NotifyReward(List<Proto.Item> items)
        {
            foreach (var i in items)
            {
                var config = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.ItemConfig>(i.Entry);
                UI.UITipDrawer.Singleton.DrawNotify(string.Format(LanguageManager.Singleton["REWARD_ITEM"],config.Name,i.Diff));
            }
            //throw new NotImplementedException();
        }
    }
}
