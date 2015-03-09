using UnityEngine;
using System.Collections;
using Assets.Scripts.PersistStructs;

[ExecuteInEditMode]
public class GameMap : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        if (this.GetComponent<BoxCollider>() == null)
            this.gameObject.AddComponent<BoxCollider>();
    }
	
	// Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (CurrentMap == null)
        {
            this.CurrentMap = ScriptableObject.CreateInstance<Map>();
            SetWH(32, 32);
        }
#endif
    }

    public void SetWH(int w, int h)
    {
        CurrentMap.Height = w;
        CurrentMap.Width = h;
        var box = this.GetComponent<BoxCollider>();
        box.size = new Vector3(w, 0f, h);
        offset = new Vector3((w / 2f), 0, (h / 2f)) - new Vector3(0.5f,0,0.5f);
    }

    [HideInInspector]
    public Vector3 offset = Vector3.zero;
    public Map CurrentMap;
    public void OnDrawGizmos()
    {
        if (CurrentMap == null) return;
        for (var x = 0; x < this.CurrentMap.Width; x++)
        {
            for (var z = 0; z < this.CurrentMap.Height; z++)
            {
                var pos = new Vector3(x, 0, z) - offset;
                Gizmos.color = Color.white;
                var rect = new Rect(pos.x, pos.z, 1, 1);
                if (rect.Contains(new Vector2(CurrentPos.x, CurrentPos.z)))
                {
                    //pos += new Vector3(0, 0.1f, 0);
                    Gizmos.color = Color.green;
                    CurrentGrid = new Vector2(x, z);
                    Gizmos.DrawCube(pos, new Vector3(0.99f, 0, 0.99f));
                }
                else
                {
                    Gizmos.DrawWireCube(pos, new Vector3(0.99f, 0, 0.99f));
                    if(CurrentSelectMapGrid!=null)
                    {
                        if (x == CurrentSelectMapGrid.PosX && z == CurrentSelectMapGrid.PosY)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(pos, new Vector3(0.99f, 0, 0.99f));
                        }
                    }
                   
                }
            }
        }
    }

    [HideInInspector]
    public Vector2 CurrentGrid =Vector2.zero; 
    private  Vector3 CurrentPos;

    public void OnTargetPos(Vector3 index)
    {
        CurrentPos = index;
    }

    public MapGrid CurrentSelectMapGrid;
    public void OnMouseDown()
    {
        foreach(var i in CurrentMap.Grids)
        {
            if (i.PosX == (int)CurrentGrid.x && i.PosY == (int)CurrentGrid.y)
            {
                CurrentSelectMapGrid = i;
            }
        }

        CurrentSelectMapGrid = new MapGrid
        {
            PosX = (int)CurrentGrid.x,
            PosY = (int)CurrentGrid.y,
            ConfigID = -1
        };
        CurrentMap.Grids.Add(CurrentSelectMapGrid);
    }
}
