using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    public List<Tile> listTile = new List<Tile>();

    // private List<TileData> tileData;
    private int typeTile;
    private int chanceOfTile;

    private void Start()
    {
        for (int i = 0; i < typeTile; i++)
        {
            for (int j = 0; j < chanceOfTile; j++)
            {
                Vector3 pos = new Vector3(UnityEngine.Random.Range(-3f, 3f), 1, UnityEngine.Random.Range(-3.5f, 6.5f));
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                tile.transform.SetParent(transform);
            }
        }
    }

    public void Initialized()
    {
        typeTile = 4;
        chanceOfTile = 6;
    }
}
