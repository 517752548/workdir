using UnityEngine;
using System.Collections;

public class ExploreMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {

        if (MapCamera == null) return;
        MapCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(MapCamera.orthographicSize, TargetSize, Time.deltaTime*SizeDump), MinSize, MaxSize);

        MapCamera.transform.position = Vector3.Lerp(MapCamera.transform.position, targetPos, Time.deltaTime * PosDump);
    }

    [SerializeField]
    public Camera MapCamera;

    //Use x,y -> x,z
    public void LookAtPos(Vector2 pos)
    {
        targetPos = new Vector3(pos.x, 10, pos.y); 
    }

    public void LookAtGrid(Vector2 grid)
    {

    }

    public float PosDump = 1;

    public Vector3 targetPos;

    public float SizeDump = 0.1f;
   
    public float TargetSize = 1f;

    [SerializeField]
    public float MaxSize = 2;
    [SerializeField]
    public float MinSize = 0.2f;
}
