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

    private Proto.MapEventType? lastType;
}
