using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Maps/Data")]

[Serializable]
public class MapDataSO : ScriptableObject
{
    public int level;
    public float timePlay;
    public List<TileData> tileDatas = new List<TileData>();
}

[Serializable]
public struct TileData
{
    public string nameTag;
    public Sprite sprite;
    public int quantity;
}
