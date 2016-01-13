using org.vxwo.csharp.json;
using Proto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MapTypeSpritesEditorWindow : EditorWindow
{
    //MapAutoGenEditor

    [MenuItem("GAME/MAP/SET_MAP_TYPE_SPRITES")]
    public static void ShowMapEditor()
    {
        if (_editor == null)
        {
            _editor = CreateInstance<MapTypeSpritesEditorWindow>();
            _editor.data = new List<SAVE_DATA>();
            var types = Enum.GetValues(typeof(MapEventType));
            var saveTypes = SaveData;
            foreach (var i in types)
            {
                var type = (MapEventType)i;
                SAVE_DATA data =null;
                

                foreach (var d in saveTypes)
                {
                    if (d.Type == type)
                        data = d;
                }

                if (data != null)
                {
                    _editor.data.Add(data);
                }
                else {
                    _editor.data.Add(new SAVE_DATA { Type = type, GUID = string.Empty });
                }
            }
        }
        _editor.Show();
    }

    public static MapTypeSpritesEditorWindow _editor { get; set; }
	public Vector2 socoll = Vector2.zero;
    public void OnGUI()
    {
        if (data == null) return;

		socoll = EditorGUILayout.BeginScrollView (socoll, GUILayout.Height(500));
        EditorGUILayout.BeginVertical();
        int index = 1;
        EditorGUILayout.BeginHorizontal();
        foreach (var i in data)
        {
            UnityEngine.Object obj = i.Obj;
            if (obj==null && !string.IsNullOrEmpty(i.GUID))
            {
                obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(i.GUID), typeof(Sprite));
            }

            obj = EditorGUILayout.ObjectField(i.Type.ToString(), obj, typeof(Sprite));
            if (obj != null)
            {
                i.Obj = obj;
                i.GUID = AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath(obj));
            }

            if (index >0 && index % 2==0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }
            index++;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("SAVE"))
        {
            var path = Path.Combine(Application.dataPath, PATH);
            var json = JsonTool.Serialize(data);
            File.WriteAllText(path,json);
        }
        EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView ();
    }

    public static List<SAVE_DATA> SaveData
    {
        get
        {
            var path = Path.Combine(Application.dataPath, PATH);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(Path.Combine(Application.dataPath, PATH));
                if (string.IsNullOrEmpty(json)) return new List<SAVE_DATA>();
                return JsonTool.Deserialize<List<SAVE_DATA>>(json);
            }
            return  new List<SAVE_DATA>();
        }
    }

    private List<SAVE_DATA> data;

    private const string PATH = "_SAVE_TYPE_SPRITE.setting";

    public class SAVE_DATA
    {
        public MapEventType Type;
        public string GUID;
        [JsonIgnore]
        public UnityEngine.Object Obj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Sprite GetSpriteByType(MapEventType type)
    {
        foreach (var i in SaveData)
        {
            if (i.Type == type)
            {
                if (!string.IsNullOrEmpty(i.GUID))
                {
                    return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(i.GUID), typeof(Sprite)) as Sprite;
                }
            }
        }

        return null;
    }

}
