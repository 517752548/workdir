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

    public void Start()
    {
        Map = GameObject.FindObjectOfType<GameMap>();
    }
    
    public GameMap Map;
}
