using Assets.Scripts.DataManagers;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Tools;


public class MapPosition :MonoBehaviour 
{
    [SerializeField]
    public int X;
    [SerializeField]
    public int Y;
    [SerializeField]
    public MapEventType DataType = MapEventType.None;

    public void OnDrawGizmosSelected()
    {
       // Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(this.transform.position, Vector3.one);
    }

    public int ToIndex()
    {
        return GamePlayerManager.PosXYToIndex(X, Y);
    }

	public void SetMask(bool isOpen)
	{
		var mask = this.transform.Find ("mask");
		if (isOpen) {
			if(mask!=null)
			Destroy (mask.gameObject);
		} else {
			if (mask == null) {
				var maskObj = new GameObject ("mask");
				maskObj.transform.parent = this.transform;
				maskObj.transform.localPosition = Vector3.zero;
				var sp = maskObj.AddComponent<SpriteRenderer> ();
				sp.sprite = ResourcesManager.Singleton.LoadResources<Sprite> ("mask");
				sp.sortingOrder = 3;
			}
		}

	}
}
