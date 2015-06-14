using UnityEngine;
using System.Collections;
using Assets.Scripts.PersistStructs;

[ExecuteInEditMode]
public class GameMap : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
    void Update()
    {
        if (MapCamera == null) return;
        MapCamera.transform.position = Vector3.Lerp(MapCamera.transform.position, TargetPos, Time.deltaTime * 4);
        MapCamera.orthographicSize = Mathf.Lerp(MapCamera.orthographicSize, targetZone, Time.deltaTime * 4);
    }

    public void LookAt(Vector2 grid, bool nodelay = false)
    {
        var center = (Vector3)grid + new Vector3(0, 0, -20);
        TargetPos = center;
        if (nodelay)
            MapCamera.transform.position = TargetPos;
    }

    public void SetTarget(float zone)
    {
        targetZone = zone;
    }

    public Vector3 TargetPos;
    public float targetZone = 4;

    public Map CurrentMap;

    [SerializeField]
    public Camera MapCamera;

    public void SetZone(float zone,bool noDelay = false)
    {
        targetZone = zone;
        if(noDelay)
        {
            MapCamera.orthographicSize = targetZone;
        }
    }

#if UNITY_EDITOR
    public void SetWH(int w, int h)
    {
        CurrentMap = new Map();
        CurrentMap.Height = w;
        CurrentMap.Width = h;
        var box = this.GetComponent<BoxCollider>();
        box.size = new Vector3(w, h, 0f);
        offset = new Vector3((w / 2f), (h / 2f), 0) - new Vector3(0.5f, 0.5f, 0f);
        box.center = offset;
        
    }
    [HideInInspector]
    public Vector2 CurrentGrid =Vector2.zero;
    [HideInInspector]
    public Vector3 offset = Vector3.zero;

    public void OnDrawGizmos()
    {
        if (!ShowGrid) return;
        if (CurrentMap == null) return;
        for (var x = 0; x < this.CurrentMap.Width; x++)
        {
            for (var z = 0; z < this.CurrentMap.Height; z++)
            {
                var pos = new Vector3(x, z, 0);
                Gizmos.color = Color.white;
                var rect = new Rect(pos.x, pos.z, 1, 1);
                Gizmos.DrawWireCube(pos, new Vector3(0.99f, 0.99f, 0));

            }
        }
    }
    [HideInInspector]
    public bool ShowGrid = false;
#endif
}
