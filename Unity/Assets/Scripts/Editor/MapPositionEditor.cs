using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapPosition))]
public class MapPositionEditor :  Editor
{
    public override void OnInspectorGUI()
    {
        var pos = target as MapPosition;
        if (lastType == null)
        {
            lastType = pos.DataType;
        }

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField(string.Format("POS:({0},{1})", pos.X, pos.Y));

        pos.DataType = (Proto.MapEventType) EditorGUILayout.EnumPopup("EventType",pos.DataType);

        if (pos.DataType != lastType)
        { 
            //OnChanage

            lastType = pos.DataType;
            MapAutoGenEditor.SetType(pos);
        }

        EditorGUILayout.EndVertical();
    }


    public void OnEnable()
    {
        if (!IsAuto) return;
        var target = this.target as MapPosition;
        target.DataType = current;
        MapAutoGenEditor.SetType(target);
    }

    private static Proto.MapEventType current = Proto.MapEventType.BattlePos;

    private static bool IsAuto = false;

    public void OnSceneGUI()
    {
        Handles.BeginGUI();

        EditorGUILayout.BeginVertical();
		var lst = current;
		var pos = target as MapPosition;
        current=  (MapEventType)EditorGUILayout.EnumPopup("默认编辑类型",current, GUILayout.Width(300));
		if (lst != current) 
		{
			pos.DataType = current;
			MapAutoGenEditor.SetType(pos);
		}
        IsAuto = EditorGUILayout.Toggle("自动改变:", IsAuto, GUILayout.Width(300));
        EditorGUILayout.EndVertical();
		EditorApplication.MarkSceneDirty ();
        Handles.EndGUI();
    }
    private Proto.MapEventType? lastType;
}
