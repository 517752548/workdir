using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Tools;
using System.IO;

public class MapAutoGenEditor:EditorWindow
{

	public class ResourceGridTexture
	{
		public Texture2D Texture { set; get; }

		public int Pro { set; get; }

	}



	[MenuItem ("GAME/MAP/AUTO_GEN_MAP")]
	public static void ShowMapEditor ()
	{
		if (_editor == null)
			_editor = CreateInstance<MapAutoGenEditor> ();
		_editor.Show ();
	}

	//[MenuItem ("GAME/MAP/REFRESH_ALL_SPRITE")]
	public static void ResetAllSprite ()
	{
		float unitOfgrids = (float)gridPlexSize / (float)OneUnitOfPlex;
		var res = AssetDatabase.LoadAssetAtPath ("Assets/Maps/Texture/gezi.png", typeof(Sprite)) as Sprite;
		var grid = GameObject.FindObjectsOfType<MapPosition> ();
		var map = GameObject.FindObjectOfType<GameMap> ();
		map.SetWH ((int)size.x, (int)size.y, unitOfgrids);
		map.MapCamera = GameObject.FindObjectOfType<Camera> ();
		map.ShowGrid = true;

		foreach (var i in grid) {
			MapAutoGenEditor.SetType (i);
			i.transform.localPosition = new Vector3 (
				(float)i.X *(gridPlexSize / (float)OneUnitOfPlex),
				(float)i.Y * (gridPlexSize / (float)OneUnitOfPlex),
				0) ;
		}



		var groundRoot = GameObject.Find (rootname);
		Bg (groundRoot);

		var root = groundRoot.transform.FindChild<Transform> (backgrounName);
		for (var i = 0; i < root.childCount; i++) {
			var ground = root.GetChild (i);
			var gr = ground.FindChild<SpriteRenderer>("groundgrid");
			if (gr == null) {
				var g = new GameObject ("groundgrid", typeof(SpriteRenderer));
				g.transform.parent = ground;
				g.transform.localPosition = Vector3.zero;
				gr = g.GetComponent<SpriteRenderer> ();
			}

			gr.sprite = res;
			gr.sortingOrder = 1;
					//GROUND
		}
	}

	private static void Bg(GameObject root)
	{
		float unitOfgrids = (float)gridPlexSize / (float)OneUnitOfPlex;
		var old = root.transform.FindChild<Transform> (backgrounName);

		if (old != null) {
			GameObject.DestroyImmediate (old.gameObject);
		}
		var ground = new  GameObject (backgrounName);
		ground.transform.parent = root.transform;
		ground.transform.localPosition = Vector3.zero;
		ground.transform.localScale = Vector3.one;

		Vector2 bg = size * ((float)gridPlexSize / (float)OneTexturePlex);

		var of = Vector2.one * ((float)OneTexturePlex / (float)OneUnitOfPlex) / 2f - (Vector2.one * unitOfgrids / 2f);
		var offset = new Vector3 (of.x, of.y, 0);
		var pros = GridSprites.Select (t => t.Pro).ToArray ();
		for (var w = 0; w < bg.x; w++) {
			for (var h = 0; h < bg.y; h++) {
				int index = GRandomer.RandPro (pros);
				var texture = GridSprites [index];
				if (index < GridSprites.Count) {
					var grid = new GameObject (string.Format ("_BG_:{0}-{1}", w, h), typeof(SpriteRenderer));
					grid.transform.parent = ground.transform;
					grid.transform.localScale = Vector3.one;
					grid.transform.localPosition = new Vector3 (
						w * ((float)OneTexturePlex / (float)OneUnitOfPlex),
						h * ((float)OneTexturePlex / (float)OneUnitOfPlex),
						0) + offset;
					var render = grid.GetComponent<SpriteRenderer> ();
					var srpite = AssetDatabase.LoadAssetAtPath (
						AssetDatabase.GetAssetPath (texture.Texture), 
						typeof(Sprite));
					render.sprite = srpite as Sprite;

				}
			}
		}
	}

	[MenuItem ("GAME/MAP/EXPORT_MAP_DATA")]
	public static void ExportJsonTabData ()
	{
		if (EditorUtility.DisplayDialog ("导出", "将生成文件?\n点错了就取消呗！", "OK", "Cancel")) { 
			var str = (EditorUtility.SaveFilePanel ("导出到", Application.dataPath,
				                   EditorApplication.currentScene.Substring (EditorApplication.currentScene.LastIndexOf ("/") + 1)
                .Split ('.') [0].Replace (".", ""),
				                   ".txt"));


			var pos = GameObject.FindObjectsOfType<MapPosition> ();
			var dict = new Dictionary<Proto.MapEventType, List<int>> ();

			foreach (var i in pos) {
				if (i.DataType == Proto.MapEventType.None)
					continue;
				List<int> list;
				if (!dict.TryGetValue (i.DataType, out list)) {
					list = new List<int> ();
					dict.Add (i.DataType, list);
				}

				list.Add (i.ToIndex ());
			}

			StringBuilder sb = new StringBuilder ();
			foreach (var i in dict) {
				sb.Append (i.Key.ToString ()).Append ("\t").Append ((int)i.Key).Append ("\t");
				var temp = new StringBuilder ();
				var isF = true;
				foreach (var t in i.Value) {
					if (!isF) {
						temp.Append ('|');
					}
					isF = false;
					temp.Append (t);
				}

				sb.AppendLine (temp.ToString ());
			}

			File.WriteAllText (str, sb.ToString ());
		}
	}

	private static MapAutoGenEditor _editor;

	public  static List<ResourceGridTexture> GridSprites;
	private bool needAdd = false;

	private static Vector2 size = new Vector2 (20, 20);
	private static float gridPlexSize = 640f/6f;
	private static float OneUnitOfPlex = 100;
	private static float OneTexturePlex = 640;

	private const string rootname = "MAP_ROOT";
	private const string backgrounName = "GROUND";
	private const string gridName = "GRIDS";

	public void OnGUI ()
	{
        
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.BeginVertical ();

		size = EditorGUILayout.Vector2Field ("地图大小", size);
		gridPlexSize = EditorGUILayout.FloatField ("格子大小", gridPlexSize);
		OneUnitOfPlex = EditorGUILayout.FloatField ("一个单位代表像素", OneUnitOfPlex);
		OneTexturePlex = EditorGUILayout.FloatField ("图片像素大小", OneTexturePlex);
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();
		if (GUILayout.Button ("刷新")) 
		{
			var map = GameObject.FindObjectOfType<GameMap> ();
			size = new Vector2 (map.CurrentMap.Width, map.CurrentMap.Height);
			ResetAllSprite ();
		}


		if (GUILayout.Button ("创建")) {

			if(EditorUtility.DisplayDialog("WARNING!","CREATE WILL LOST CURRENT?","OK","CANCEL")) {
			if (GridSprites != null || GridSprites.Count > 0) {
				var root = GameObject.Find (rootname);
				if (root != null)
					GameObject.DestroyImmediate (root);
				root = new GameObject (rootname, typeof(BoxCollider), typeof(GameMap));
				root.transform.position = Vector3.zero;
				root.transform.localScale = Vector3.one;


				//var boxSize = size * ((float)OneUnitOfPlex/(float)gridPlexSize) ;

				float unitOfgrids = (float)gridPlexSize / (float)OneUnitOfPlex;// / (float)gridPlexSize;


				var map = root.GetComponent<GameMap> ();
				map.SetWH ((int)size.x, (int)size.y, unitOfgrids);
				map.MapCamera = GameObject.FindObjectOfType<Camera> ();
				map.ShowGrid = true;
				
			    Bg (root);

				var grids = new GameObject (gridName);
				grids.transform.parent = root.transform;
				grids.transform.localPosition = Vector3.zero;
				grids.transform.localScale = Vector3.one;

				var maxX = size.x;
				var maxY = size.y;
				for (var x = 0; x < maxX; x++) {
					for (var y = 0; y < maxY; y++) {
						var grid = new GameObject (string.Format ("GRID:{0}-{1}", x, y), typeof(MapPosition));
						var pos = grid.GetComponent<MapPosition> ();
						grid.transform.parent = grids.transform;
						grid.transform.localScale = Vector3.one;

						var pox = x * (gridPlexSize / (float)OneUnitOfPlex);
						var poy = y * (gridPlexSize / (float)OneUnitOfPlex);
						grid.transform.localPosition = new Vector3 (pox, poy, 0);

						pos.X = x;
						pos.Y = y;

						SetType (pos);
					}
				}
                     
				ResetAllSprite ();
				}
			}
		}
        
		if (Selection.gameObjects != null) {
			if (GridSprites == null) {
				needAdd = true;
                
			}

			if (needAdd) {
				GridSprites = new List<ResourceGridTexture> ();
			}
			int count = 0;

			EditorGUILayout.LabelField ("GridResours:");
			foreach (var i in Selection.objects) {
				if (i is Texture2D) {
					var t = i as Texture2D;
                   
					count++;
					if (needAdd) {
						GridSprites.Add (new ResourceGridTexture { Pro = 1, Texture = t });
						Debug.Log ((i).GetType ());
					}
				}
			}

			needAdd = GridSprites.Count != count;

			foreach (var i in GridSprites) {
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField (string.Format ("{2} WH({0},{1})", i.Texture.width, i.Texture.height, i.Texture.name));
				i.Pro = EditorGUILayout.IntField ("出现比例:", i.Pro);
				EditorGUILayout.EndHorizontal ();
			}

		}
		EditorGUILayout.EndVertical ();
        
        
	}

	public static void SetType (MapPosition pos)
	{
		EditorApplication.MarkSceneDirty ();

		Sprite sp = MapTypeSpritesEditorWindow.GetSpriteByType (pos.DataType);
		var sprite = pos.gameObject.GetComponent<SpriteRenderer> ();
		if (sprite == null) {
			sprite = pos.gameObject.AddComponent<SpriteRenderer> ();
		}
		sprite.sprite = sp;
		sprite.sortingOrder = 3;
	    
		var explore = pos.transform.FindChild<SpriteRenderer> ("explored");
		if (explore == null) {
		
			var obj = new GameObject ("explored", typeof(SpriteRenderer));
			obj.transform.parent = pos.transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			var render = obj.GetComponent<SpriteRenderer> ();
			render.sprite = 
				MapTypeSpritesEditorWindow.GetExploreSpriteByType (pos.DataType);
			obj.SetActive (false);
			render.sortingOrder = 2;
		} else {
			explore.sprite =
				MapTypeSpritesEditorWindow.GetExploreSpriteByType (pos.DataType);
			explore.ActiveSelfObject (false);
		}
        
	}

   
}


