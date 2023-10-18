using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject menuWin;
    [SerializeField] private GameObject menuLose;
    [SerializeField] private TextMeshProUGUI scoreInGame;
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private TextMeshProUGUI level;

    public void SetTimeCountDown(int minute, int second)
    {
        // scoreInGame.text = score.ToString();
        countDown.text = $"{minute}:{second}";
    }
    public void SetTextLevel(int level)
    {
        this.level.text = level.ToString();
    }


    public void SetStatusMenuWin(bool status)
    {
        menuWin.SetActive(status);
    }

    public void SetStatusMenuLose(bool status)
    {
        menuLose.SetActive(status);
    }

}
