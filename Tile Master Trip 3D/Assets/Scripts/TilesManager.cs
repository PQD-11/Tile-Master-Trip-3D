using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    public List<GameObject> tiles = new List<GameObject>();

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
                tiles.Add(tile);
            }
        }
    }

    public async Task Initialized()
    {
        typeTile = 4;
        chanceOfTile = 6;
        await Task.Delay(100);
    }

    public void RemoveTileFromTileManager(GameObject gameObject)
    {
        tiles.Remove(gameObject);
    }

    public void AddTileBackTileManager(GameObject gameObject)
    {
        tiles.Add(gameObject);
    }

    public bool CheckWin()
    {
        return tiles.Count == 0;
    }
}