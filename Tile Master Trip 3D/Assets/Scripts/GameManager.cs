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
    public List<GameObject> listTile = new List<GameObject>();
    public List<GameObject> listPosContainer = new List<GameObject>();
    public Dictionary<string, int> countColor = new Dictionary<string, int>();
    private string tagObj;
    private float totalTimePlay = 10f; // se duoc get tu MapData
    private float currentTime;

    private void Awake()
    {
        currentTime = totalTimePlay;
        tilesManager.Initialized();
    }
    public void AddToContainer(GameObject gameObject)
    {
        if (listTile.Count > 0)
        {
            listTile.Insert(0, gameObject);
            UpdatePosTiles(1, listTile.Count - 1);
        }
        else
        {
            listTile.Insert(0, gameObject);
            Debug.Log(listTile.Count);
        }

        // check 
        tagObj = gameObject.tag;
        if (countColor.ContainsKey(tagObj))
        {
            countColor[tagObj]++;
        }
        else
        {
            countColor.Add(tagObj, 1);
        }

        if (countColor[tagObj] == 3)
        {
            StartCoroutine(RemoveTileMatching(tagObj));
        }
        else if (listTile.Count == 7)
        {
            gameUI.SetStatusMenuLose(true);
        }

        int childs = tileParent.childCount;
        if (childs == 0)
        {
            gameUI.SetStatusMenuWin(true);
        }
    }

    IEnumerator RemoveTileMatching(string tagObj)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        for (int i = 0; i < listTile.Count; i++)
        {
            if (listTile[i].tag == tagObj)
            {
                Destroy(listTile[i].gameObject);
            }
        }
        listTile.RemoveAll(r => r.tag == tagObj);
        countColor[tagObj] = 0;
        UpdatePosTiles(0, listTile.Count - 1);
    }

    private void UpdatePosTiles(int first, int end)
    {
        for (int i = end; i >= first; i--)
        {
            listTile[i].transform.DOMove(listPosContainer[i].transform.position, 0.1f);
        }
    }

    private void Update()
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
