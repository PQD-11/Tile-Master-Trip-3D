using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private TilesManager tilesManager;
    public List<GameObject> containerTile = new List<GameObject>();
    public List<GameObject> containerPositions = new List<GameObject>();
    public Dictionary<string, int> tableCountType = new Dictionary<string, int>();
    private float totalTimePlay = 180f; // se duoc get tu MapData
    private float currentTime;
    private GameObject currentTile;
    private bool isRemoveMatching;
    private bool isUpdatePosTile;

    async void Awake()
    {
        currentTime = totalTimePlay;
        await tilesManager.Initialized();

        Debug.Log("tileManager child count: " + tilesManager.transform.childCount);

        foreach (Transform child in tilesManager.transform)
        {
            Tile tile = child.GetComponent<Tile>();
            tile.OnClick += OnTileClicked;
            tile.OnMoveCompleted += OnTileMoveCompleted;
        }
    }

    private void OnTileMoveCompleted(Tile tile)
    {
        if (!isRemoveMatching)
        {
            AddToContainer(tile.gameObject);
        }
        else
        {
            StartCoroutine(WaitForRemoving(tile));
        }

        currentTile = tile.gameObject;
    }

    private void CheckMatching(GameObject gameObject)
    {
        if (tableCountType[gameObject.tag] == 3)
        {
            isRemoveMatching = true;
            RemoveTileMatching(gameObject.tag);
        }

        if (containerTile.Count == 7)
        {
            gameUI.SetStatusMenuLose(true);
        }

        if (tilesManager.transform.childCount == 0)
        {
            gameUI.SetStatusMenuWin(true);
        }
    }

    IEnumerator WaitForRemoving(Tile tile)
    {
        yield return new WaitUntil(() => isRemoveMatching = true);
        AddToContainer(tile.gameObject);
    }

    private void OnTileClicked(Tile tile)
    {
        if (containerTile.Count > 0)
        {
            UpdatePosTilesBack();
        }
        tile.MoveToContainer(containerPositions[0].transform);
    }

    public void OnButtonBack()
    {
        if (currentTile != null)
        {
            Tile tile = currentTile.GetComponent<Tile>();
            tile.MoveToBack();
            containerTile.Remove(currentTile);
            tilesManager.AddTileBackTileManager(currentTile);

            UpdatePosTiles(0, containerTile.Count - 1);
            tableCountType[currentTile.tag]--;
        }
        
        if (containerTile.Count > 0)
        {
            currentTile = containerTile[0];
        }
        else 
        {
            currentTile = null;
        }
    }

    public void AddToContainer(GameObject gameObject)
    {
        containerTile.Insert(0, gameObject);
        tilesManager.RemoveTileFromTileManager(gameObject);

        if (tableCountType.ContainsKey(gameObject.tag))
        {
            tableCountType[gameObject.tag]++;
        }
        else
        {
            tableCountType.Add(gameObject.tag, 1);
        }

        CheckMatching(gameObject);

    }

    private void RemoveTileMatching(string tag)
    {
        List<GameObject> listDestroy = new List<GameObject>();
        for (int i = 0; i < containerTile.Count; i++)
        {
            if (containerTile[i].tag == tag)
            {
                listDestroy.Add(containerTile[i]);
                tableCountType[tag]--;
            }
        }

        if (!isUpdatePosTile)
        {
            containerTile.RemoveAll(i => i.tag == tag);
            foreach (GameObject obj in listDestroy)
            {
                obj.transform.DOKill();
                Destroy(obj);
            }
        }
        else
        {
            StartCoroutine(WaitForUpdatePosTile(listDestroy));
        }

        UpdatePosTiles(0, containerTile.Count - 1);
        isRemoveMatching = false;
    }

    private IEnumerator WaitForUpdatePosTile(List<GameObject> listDestroy)
    {
        yield return new WaitUntil(() => isUpdatePosTile = true);
        containerTile.RemoveAll(r => r.tag == tag);
        foreach (GameObject obj in listDestroy)
        {
            Destroy(obj);
        }
    }

    private void UpdatePosTiles(int first, int end)
    {
        for (int i = end; i >= first; i--)
        {
            containerTile[i].transform.DOMove(containerPositions[i].transform.position, 0.1f);
        }
    }
    private void UpdatePosTilesBack()
    {
        isUpdatePosTile = true;
        for (int i = containerTile.Count - 1; i >= 0; i--)
        {
            // if (containerTile[i] == null) break;
            containerTile[i].transform.DOMove(containerPositions[i + 1].transform.position, 0.1f);
        }
        isUpdatePosTile = false;
    }

    private void Update()
    {
        //CountDownTimer
        currentTime -= Time.deltaTime;

        if (currentTime < 0)
        {
            currentTime = 0;
            gameUI.SetStatusMenuLose(true);
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        gameUI.SetTimeCountDown(minutes, seconds);

    }
}
