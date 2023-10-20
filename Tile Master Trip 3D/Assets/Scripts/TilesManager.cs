using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    public List<GameObject> TilesStore = new List<GameObject>();
    private List<TileData> tileDatas;

    private void Start()
    {
        for (int i = 0; i < tileDatas.Count; i++)
        {
            for (int j = 0; j < tileDatas[i].quantity; j++)
            {
                Vector3 pos = new Vector3(UnityEngine.Random.Range(-3f, 3f), 1, UnityEngine.Random.Range(-3.5f, 6.5f));
                GameObject tileObject = Instantiate(tilePrefab, pos, Quaternion.identity);
                tileObject.transform.SetParent(transform);
                tileObject.tag = tileDatas[i].nameTag;
                TilesStore.Add(tileObject);
                Tile tile = tileObject.GetComponent<Tile>();
                tile.Initialized(tileDatas[i].sprite);
            }
        }
    }

    public async Task Initialized(List<TileData> tileDatas)
    {
        this.tileDatas = tileDatas;
        await Task.Delay(100);
    }

    public void RemoveTileFromTileManager(GameObject gameObject)
    {
        TilesStore.Remove(gameObject);
    }

    public void AddTileBackTileManager(GameObject gameObject)
    {
        TilesStore.Add(gameObject);
    }

    public bool CheckWin()
    {
        return TilesStore.Count == 0;
    }
}