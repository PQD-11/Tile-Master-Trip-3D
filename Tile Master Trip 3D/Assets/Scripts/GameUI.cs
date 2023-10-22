using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject menuWin;
    [SerializeField] private GameObject menuLose;
    [SerializeField] private GameObject menuPause;
    [SerializeField] private TextMeshProUGUI coinInGame;
    [SerializeField] private TextMeshProUGUI coinInMenuWin;
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private TextMeshProUGUI level;

    public void SetTimeCountDown(int minute, int second)
    {
        countDown.text = $"{minute}:{second}";
    }
    public void SetTextLevel(int level)
    {
        this.level.text = "Level." + level.ToString();
    }

    public void SetCoinInGame(int coin)
    {
        coinInGame.text = coin.ToString();
    }

    public void SetCoinInMenuWin(int coin)
    {
        coinInMenuWin.text = coin.ToString();
    }

    public void SetStatusMenuWin(bool status)
    {
        menuWin.SetActive(status);
    }

    public void SetStatusMenuLose(bool status)
    {
        menuLose.SetActive(status);
    }

    public void SetStatusMenuPause(bool status)
    {
        menuPause.SetActive(status);
    }

}
