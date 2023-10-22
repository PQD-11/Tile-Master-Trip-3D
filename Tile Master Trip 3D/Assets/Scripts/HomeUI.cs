using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private GameObject menuSetting;
    [SerializeField] private TextMeshProUGUI txt_totalCoin;
    [SerializeField] private TextMeshProUGUI txt_level;

    public void SetTextLevel(int level)
    {
        txt_level.text = level.ToString();
    }

    public void SetTextCoin(int coin)
    {
        txt_totalCoin.text = coin.ToString();
    }

    public void SetStatusMenuSetting(bool status)
    {
        menuSetting.SetActive(status);
    }
}
