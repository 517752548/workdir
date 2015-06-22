﻿using Assets.Scripts.DataManagers;
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

namespace Assets.Scripts.App
{
    public class GameAppliaction:Tools.XSingleton<GameAppliaction>,ExcelConfig.IConfigLoader
    {
        public void Start(IRuner runer)
        {
            //初始化配置
            var excelConfig = new ExcelToJSONConfigManager(this);
            prisit = new List<IPresist>(){
                PlayerItemManager.Singleton,
                GamePlayerManager.Singleton,
                BuildingManager.Singleton,
                PlayerSoldierManager.Singleton,
                PlayerArmyManager.Singleton
            };
            foreach (var i in prisit)
                i.Load();
            //初始化道具
            Debug.Log(LanguageManager.Singleton["APP_NAME"]);
            //进入游戏
            JoinCastle();

            Runner = runer;
            Runner.DoRun(Run());
        }

        public IRuner Runner { private set; get; }

        private List<IPresist> prisit;

        private ConstValues _ConstValues;
        public ConstValues ConstValues
        {
            get
            {
                if (_ConstValues == null)
                    _ConstValues = new ConstValues();
                   // _ConstValues = ExcelToJSONConfigManager.Current.FirstConfig<ConstValuesConfig>((item) => { return true; });
                return _ConstValues;
            }
        }

        public void Exit() 
        {
            foreach (var i in prisit)
                i.Presist();
        }
        public void Pause() { }
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
            Application.LoadLevel("Castle");
            var state = new CastleState();
            ChangeState(state);
        }

        /// <summary>
        /// 开始探索
        /// </summary>
        public void GoToExplore(int configID)
        {
            var r = DoGoToExplore(configID);
            works.Enqueue(r);
        }

        private IEnumerator DoGoToExplore(int configID)
        {
            var map = ExcelConfig.ExcelToJSONConfigManager.Current.GetConfigByID<ExcelConfig.MapConfig>(configID);
            var run = Application.LoadLevelAsync(map.MapName);
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

        public Queue<IEnumerator> works = new Queue<IEnumerator>();

        public IEnumerator Run()
        {
           
            while(true)
            {
                while (works.Count > 0)
                {
                    var w = works.Dequeue();
                    while (w.MoveNext())
                        yield return null;
                }
                yield return null;
            }
        }
    }

    public interface IRuner
    {
        void DoRun(IEnumerator r);
    }
}