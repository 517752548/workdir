using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Tools;

public class MapAutoGenEditor:EditorWindow
{

    [MenuItem("GAME/MAP/AUTO_GEN_MAP")]
    public static void ShowMapEditor() 
    {
        if (_editor == null)
          _editor=  CreateInstance<MapAutoGenEditor>();
        _editor.Show();
    }

    private static MapAutoGenEditor _editor;

    public List<ResourceGridTexture> GridSprites;
    private bool needAdd = false;

    private Vector2 size = new Vector2(4, 4);
    private Vector2 gridPlexSize = new Vector2(64, 64);

    /// <summary>
    /// 一个单位相当于多少像素
    /// </summary>
    private const float oneofPlex = 64f;

    private const string rootname = "MAP_ROOT"; 
    public void OnGUI()
    {
        
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        size = EditorGUILayout.Vector2Field("地图大小", size);
        gridPlexSize = EditorGUILayout.Vector2Field("格子大小", gridPlexSize);

        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("创建"))
        {

            if (GridSprites != null || GridSprites.Count > 0)
            {
                var root = GameObject.Find(rootname);
                if (root != null)
                    GameObject.DestroyImmediate(root);
                root = new GameObject(rootname,typeof(BoxCollider), typeof(GameMap));
                root.transform.position = Vector3.zero;
                root.transform.localScale = Vector3.one;

                var boxCollider = root.GetComponent<BoxCollider>();
                boxCollider.size = new Vector3(size.x, size.y, 0.1f);
                boxCollider.center = new Vector3(size.x / 2, size.y, 0);

                var map = root.GetComponent<GameMap>();
                map.SetWH((int)size.x,(int)size.y);
                

                var pros = GridSprites.Select(t=>t.Pro).ToArray();
                for(var w =0;w <size.x;w++)
                {
                    for (var h = 0; h < size.y; h++)
                    {
                        int index = GRandomer.RandPro(pros);
                        var texture = GridSprites[index];
                        if (index < GridSprites.Count)
                        {
                            var grid = new GameObject(string.Format("grid:{0}-{1}", w, h), typeof(SpriteRenderer));
                            grid.transform.parent = root.transform;
                            grid.transform.localScale = Vector3.one;
                            grid.transform.localPosition = new Vector3(w, h, 0);
                            var render = grid.GetComponent<SpriteRenderer>();
                            var srpite = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(texture.Texture), typeof(Sprite));
                            render.sprite = srpite as Sprite;
                            var pos =grid.AddComponent<MapPosition>();
                            pos.X = w;
                            pos.Y = h;

                            
                            //  render.sprite. = texture.Texture;
                        }
                    }
                }
            }
        }
        
        if (Selection.gameObjects != null)
        {
            if(GridSprites ==null)
            {
                needAdd = true;
                
            }

            if(needAdd)
            {
                GridSprites = new List<ResourceGridTexture>();
            }
            int count = 0;

            EditorGUILayout.LabelField("GridResours:");
            foreach (var i in Selection.objects)
            {
                if (i is Texture2D)
                {
                    var t = i as Texture2D;
                   
                    count++;
                    if(needAdd )
                    {
                        GridSprites.Add(new ResourceGridTexture {  Pro = 1, Texture = t});
                        Debug.Log((i).GetType());
                    }
                }
            }

            needAdd = GridSprites.Count != count;

            foreach(var i in GridSprites)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(string.Format("{2} WH({0},{1})", i.Texture.width, i.Texture.height, i.Texture.name));
                i.Pro=  EditorGUILayout.IntField("出现比例:", i.Pro);
                EditorGUILayout.EndHorizontal();
            }

        }
        EditorGUILayout.EndVertical();
        
        
    }

    public class ResourceGridTexture
    {
        public Texture2D Texture { set; get; }
        public int Pro { set; get; }

    }
}


