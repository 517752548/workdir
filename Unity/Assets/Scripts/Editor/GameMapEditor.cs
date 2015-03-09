using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameMap))]
public class GameMapEditor : Editor
{
    #region enable
    public void OnEnable()
    {
        SceneView.onSceneGUIDelegate = HandleUpdate;
    }

    private void HandleUpdate(SceneView sceneView)
    {
        SceneView.RepaintAll();
        Event e = Event.current;
        Ray r = Camera.current.ScreenPointToRay(
            new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        RaycastHit hit;
        var target = (this.target as GameMap);

        if (Physics.Raycast(r, out hit))
        {
            var index = hit.point;
            target.OnTargetPos(index);
            if (e.isMouse && e.button == 1)
            {
                switch (e.type)
                {
                    case EventType.MouseDown:
                        target.OnMouseDown();
                        break;
                }
            }
        }
    }
    public void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= HandleUpdate;
    }
    #endregion

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public GUIStyle W = new GUIStyle();

    public void OnSceneGUI()
    {
        W.normal.textColor = Color.white;
        var target = (this.target as GameMap);
        Handles.Label(-target.offset - new Vector3(0.5f, 0, 1.5f), target.CurrentGrid.ToString(), W);
        Handles.BeginGUI();
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height- 60, 90, 20), "SAVE"))
        {
            AssetDatabase.DeleteAsset("Assets/StreamingAssets/Maps/default.asset");
            AssetDatabase.CreateAsset(target.CurrentMap, "Assets/StreamingAssets/Maps/default.asset");
        }

        if(target.CurrentSelectMapGrid!=null)
        {
            GUI.Label( new Rect(0,2,200,20),
                string.Format("Select:({0},{1})", target.CurrentSelectMapGrid.PosX,
                target.CurrentSelectMapGrid.PosY));
            GUI.Label(new Rect(0, 22, 200, 20), "ConfigID");
            var t = GUI.TextField(new Rect(0, 42, 80, 20), "" + target.CurrentSelectMapGrid.ConfigID);
            int id = 0;
            if(int.TryParse(t,out id))
            {
                target.CurrentSelectMapGrid.ConfigID = id;
            }
        }
        Handles.EndGUI();
    }
}