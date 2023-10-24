using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameUI gameUI;
    [SerializeField] private TilesManager tilesManager;
    public List<MapDataSO> mapDatas = new List<MapDataSO>();
    public List<GameObject> containerTile = new List<GameObject>();
    public List<GameObject> containerPositions = new List<GameObject>();
    public Dictionary<string, int> tableCountType = new Dictionary<string, int>();
    private float currentTime;
    private int currentLevel;
    private int cointInGame;
    private MapDataSO currentMapData;
    private GameObject currentTile;
    private bool isRemoveMatching;
    private bool isUpdatePosTile;
    private bool isPaused;
    public event Action<int> OnTileMatching;

    async void Awake()
    {
        Initialized();

        await tilesManager.Initialized(mapDatas[currentLevel - 1].tileDatas);

        Debug.Log("tileManager child count: " + tilesManager.transform.childCount);

        foreach (Transform child in tilesManager.transform)
        {
            Tile tile = child.GetComponent<Tile>();
            tile.OnClick += OnTileClicked;
            tile.OnMoveCompleted += OnTileMoveCompleted;
        }
    }

    private void Initialized()
    {
        AudioManager.Instance.PlayMusic("Theme");

        cointInGame = 0;
        currentLevel = SaveSystem.LoadLevel();

        currentMapData = mapDatas[currentLevel - 1];
        currentTime = currentMapData.timePlay;

        gameUI.SetTextLevel(currentLevel);
        gameUI.SetCoinInGame(cointInGame);
    }

    private void OnTileClicked(Tile tile)
    {
        if (containerTile.Count > 0)
        {
            UpdatePosTilesBack();
        }
        tile.MoveToContainer(containerPositions[0].transform);
    }

    private void OnTileMoveCompleted(Tile tile)
    {
        Debug.Log("Move Completed");
        if (!isRemoveMatching)
        {
            AddToContainer(tile.gameObject);
        }
        else
        {
            StartCoroutine(WaitForRemoving());
            AddToContainer(tile.gameObject);
        }

        currentTile = tile.gameObject;
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

    private void CheckMatching(GameObject gameObject)
    {
        if (tableCountType[gameObject.tag] == 3)
        {
            isRemoveMatching = true;
            RemoveTileMatching(gameObject.tag);
            OnTileMatching?.Invoke(cointInGame);
            AudioManager.Instance.PlaySFX("TileMatching");
            // gameUI.SetCoinInGame(cointInGame);
        }

        if (containerTile.Count == 7)
        {
            gameUI.SetStatusMenuLose(true);
        }
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
        cointInGame += 10;

        if (tilesManager.CheckWin())
        {
            int totalCoin = SaveSystem.LoadCoin() + cointInGame;
            SaveSystem.SaveCoin(totalCoin);
            gameUI.SetCoinInMenuWin(cointInGame);
            currentLevel = (currentLevel % mapDatas.Count) + 1;
            SaveSystem.SaveLevel(currentLevel);
            gameUI.SetStatusMenuWin(true);
        }
    }

    IEnumerator WaitForRemoving()
    {
        yield return new WaitUntil(() => isRemoveMatching = true);
    }

    private IEnumerator WaitForUpdatePosTile(List<GameObject> listDestroy)
    {
        yield return new WaitUntil(() => isUpdatePosTile = true);
        containerTile.RemoveAll(r => r.tag == tag);
        foreach (GameObject obj in listDestroy)
        {
            obj.transform.DOKill();
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
        if (!isPaused)
        {
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

    public void OnButtonBackTile()
    {
        if (isPaused) { return; }

        AudioManager.Instance.PlaySFX("PushButton");

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

    public void OnButtonHint()
    {
        var maxKeyValue = tableCountType.Aggregate((x, y) => x.Value > y.Value ? x : y);
        string keyOfMaxValue = maxKeyValue.Key;
        int maxValue = maxKeyValue.Value;

        tilesManager.FindHint(keyOfMaxValue, 3 - maxValue);
    }

    public void OnPauseButtonClicked()
    {
        AudioManager.Instance.PlaySFX("PushButton");

        isPaused = !isPaused;
        gameUI.SetStatusMenuPause(isPaused);
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        tilesManager.SetEnableTilesInStore(isPaused);
    }

    public void OnButtonBackHomeClicked()
    {
        AudioManager.Instance.PlaySFX("PushButton");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ReLoadScene()
    {
        AudioManager.Instance.PlaySFX("PushButton");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
