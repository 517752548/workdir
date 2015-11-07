using Assets.Scripts.DataManagers;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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
}
