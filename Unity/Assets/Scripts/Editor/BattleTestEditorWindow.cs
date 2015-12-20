using Assets.Scripts.App;
using Assets.Scripts.GameStates;
using ExcelConfig;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BattleTestEditorWindow : EditorWindow
{
    private static BattleTestEditorWindow _editor;

    [MenuItem("GAME/BATTLE/BEGIN_TEST")]
    public static void BeginTest()
    {
        if (_editor == null)
            _editor = CreateInstance<BattleTestEditorWindow>();
        _editor.Show();
    }

    public void OnGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            DrawPlay();
            return;
        }
        if (GameAppliaction.Singleton == null) return;
        if (!(GameAppliaction.Singleton.Current is TestBattleState))
        {
            DrawStart();
        }
        else
        {
            DrawTest();
        }
    }

    private void DrawPlay()
    {
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Start Play"))
        {
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawStart()
    {
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Start Battle Test"))
        {
            GameAppliaction.Singleton.BeginBattleTest();
        }
        EditorGUILayout.EndVertical();
    }

    private BattleGroupConfig[] BattleGroups;
    private int index = -1;
    private string[] battleGroupLabs;
    private int lastIndex = -1;

    private int battleIndex = 0;

    private void DrawTest()
    {
        if (BattleGroups == null)
        {
            BattleGroups = ExcelToJSONConfigManager.Current.GetConfigs<BattleGroupConfig>();
            lastIndex = index = -1;
            battleGroupLabs = BattleGroups.Select(t => t.ID + "_" + t.Name).ToArray();
        }

        if (monsters == null)
        {
            monsters = ExcelToJSONConfigManager.Current.GetConfigs<MonsterConfig>();
            monsterLabs = monsters.Select(t => t.ID + "_" + t.Name).ToArray();
        }
        EditorGUILayout.BeginVertical();
        ShowPlayer();
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.Popup("Battle Groups:", index, battleGroupLabs);
        battleIndex = EditorGUILayout.IntField("BattleIndex:", battleIndex);
        EditorGUILayout.EndHorizontal();

        if (index != lastIndex)
        {
            lastIndex = index;
            battleIndex = 0;
        }
        if (index >= 0)
        {
            ShowGroup(BattleGroups[index]);
        }
        EditorGUILayout.EndVertical();

        var rect = new Rect(this.position.width - 200, this.position.height - 25, 190, 20);
        if (GUI.Button(rect, "Start Battle"))
        {
            if (index >= 0)
            {
                var state = GameAppliaction.Singleton.Current as TestBattleState;
                state.PlayerSoliders = PlaySoilders;
                state.StartBattle(BattleGroups[index].ID, battleIndex);
            }
        }
    }

    private void ShowGroup(BattleGroupConfig group)
    {
        var ids = Assets.Scripts.Tools.UtilityTool.SplitIDS(group.BattleIds);
        foreach (var i in ids)
        {
            var battle = ExcelToJSONConfigManager.Current.GetConfigByID<BattleConfig>(i);
            if (battle == null) continue;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(battle.Name);
            var npc = ExcelToJSONConfigManager.Current.GetConfigByID<MonsterConfig>(battle.NpcID);
            if (npc != null)
            {
                GUILayout.Label(npc.Name);
                var skill = ExcelToJSONConfigManager.Current.GetConfigByID<SkillConfig>(npc.SkillID);
                GUILayout.Label(skill.Name);
                GUILayout.Label(((SkillDamageType)skill.MainEffectType).ToString());
                GUILayout.Label("Number:" + skill.MainEffectNumber);
                GUILayout.Label("Effect:" + skill.Effect);
                GUILayout.Label(skill.Pars1 + "_" + skill.Pars2 + "_" + skill.Pars3);
                
                //GUILayout.Label(skill.Pars2);
                //GUILayout.Label(skill.Pars3);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private int lastSize = 0;
    private int[] PlaySoilders;
    private MonsterConfig[] monsters;
    private string[] monsterLabs;
    private void ShowPlayer()
    {
        var size = EditorGUILayout.IntField("Soliders:", lastSize);
        if (size != lastSize)
        {
            PlaySoilders = new int[size];
            lastSize = size;
        }
        GameAppliaction.BattleDebug = EditorGUILayout.Toggle("ShowBattleLog", GameAppliaction.BattleDebug);

        if (PlaySoilders != null)
        {
            for (int i = 0; i < PlaySoilders.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                PlaySoilders[i] = EditorGUILayout.Popup("Monster:", PlaySoilders[i], monsterLabs);

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
