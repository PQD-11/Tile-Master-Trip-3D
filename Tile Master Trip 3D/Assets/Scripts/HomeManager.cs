using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private HomeUI homeUI;

    private int totalCoin;
    private int level;

    private void Awake()
    {
        totalCoin = SaveSystem.LoadCoin();
        level = SaveSystem.LoadLevel();
        homeUI.SetTextLevel(level);
        homeUI.SetTextCoin(totalCoin);
    }

    public void OnButtonSettingClicked()
    {
        homeUI.SetStatusMenuSetting(true);
    }

    public void OnButtonCloseMenuSettingUI()
    {
        homeUI.SetStatusMenuSetting(false);
    }

    public void OnButtonPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
