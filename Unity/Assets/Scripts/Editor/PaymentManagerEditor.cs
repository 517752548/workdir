using Assets.Scripts.DataManagers;
using Assets.Scripts.PersistStructs;
using Assets.Scripts.Tools;
using org.vxwo.csharp.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PaymentManagerEditor : EditorWindow
{
	[MenuItem("GAME/CONFIGS_INIT")]
	public static void Init_Config()
	{
		//var list = new List<Assets.Scripts.AppConfig.KeyValue> ();
	}


    [MenuItem("GAME/PAYMENT/SHOW")]
    public static void ShowMapEditor()
    {
        if (_editor == null)
            _editor = CreateInstance<PaymentManagerEditor>();
        _editor.ShowInit();
        _editor.Show();
       
    }


    private string path;

    private void ShowInit()
    {
        path = GamePlayerManager.PAYMENT_PATH;
        List<PaymentData> datas;
        var json = Utility.ReadAStringFile(Utility.GetStreamingAssetByPath(path));
        if (!string.IsNullOrEmpty(json))
        {
            datas = JsonTool.Deserialize<List<PaymentData>>(json);
        }

        else
        {
            datas = new List<PaymentData>();
        }

        Data = datas;
    }


    public void OnGUI()
    {
        if (Data == null) return;
        EditorGUILayout.BeginVertical();


       
        var list = Data.ToArray();
        foreach (var i in list)
        {
            EditorGUILayout.BeginVertical();
            i.BundleID = EditorGUILayout.TextField("标识", i.BundleID);
            EditorGUILayout.BeginHorizontal();
            i.Reward = EditorGUILayout.IntField("获得", i.Reward);
            i.SpriteName = EditorGUILayout.TextField( "图标",i.SpriteName);
            EditorGUILayout.EndHorizontal();
            i.Des = EditorGUILayout.TextField("描述", i.Des);
            if (GUILayout.Button("del",GUILayout.Width(80)))
            {
                Data.Remove(i);
            }
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("ADD"))
        {
            Data.Add(new PaymentData());
        }

        if (GUILayout.Button("Save"))
        {
            var targetPath = Utility.GetStreamingAssetByPath(path);
            var json = JsonTool.Serialize(Data);
            Utility.WriteStringFile(targetPath, json);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private List<PaymentData> Data;

    public static PaymentManagerEditor _editor { get; set; }
}

    //PaymentData
