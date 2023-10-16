using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.Sqlite;

public class GameManager : MonoBehaviour
{
    public List<GameObject> listTile = new List<GameObject>();
    public List<GameObject> listPosContainer = new List<GameObject>();
    public Dictionary<string, int> countColor = new Dictionary<string, int>();
    private string tagObj;

    public void AddToContainer(GameObject gameObject)
    {
        if (listTile.Count == 7)
        {
            // handle overgame
            return;
        }

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
            StartCoroutine(RemoveTileMatching());
        }
    }

    IEnumerator RemoveTileMatching()
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
}
