using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private static string levelKey = "LevelKey";
    private static string coinKey = "CoinKey";

    public static void SaveLevel(int level)
    {
        PlayerPrefs.SetInt(levelKey, level);
    }

    public static int LoadLevel()
    {
        if (!PlayerPrefs.HasKey(levelKey))
        {
            PlayerPrefs.SetInt(levelKey, 1);
        }
        Debug.Log("level: " + PlayerPrefs.GetInt(levelKey));
        return PlayerPrefs.GetInt(levelKey);
    }

    public static void SaveCoin(int coin)
    {
        PlayerPrefs.SetInt(coinKey, coin);
    }

    public static int LoadCoin()
    {
        if (!PlayerPrefs.HasKey(coinKey))
        {
            PlayerPrefs.SetInt(coinKey, 0);
        }
        return PlayerPrefs.GetInt(coinKey);
    }

    

    public static void ResetSaveSystem()
    {
        PlayerPrefs.DeleteAll();
    }
}
