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
        Ray r = Camera.current.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var target = (this.target as GameMap);

        if (Physics.Raycast(r, out hit))
        {
            var index = hit.point;
            if (e.isMouse && e.button == 1)
            {
                switch (e.type)
                {
                    case EventType.MouseDown:
                        //target.OnMouseDown();
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
        W.fontSize = 20;
        var target = (this.target as GameMap);
        Handles.Label( new Vector3(0.5f, 0, 0f),"(0,0)",W);
        Handles.BeginGUI();
        GUI.Label(new Rect(0, 2, 200, 20), "SHOW Grid",W);
        target.ShowGrid = GUI.Toggle(new Rect(3, 22, 200, 20), target.ShowGrid, "");
        if (GUI.Button(new Rect(0, 52, 100, 20), "导出",W))
        {
            MapAutoGenEditor.ExportJsonTabData();
        }
        Handles.EndGUI();
    }
}