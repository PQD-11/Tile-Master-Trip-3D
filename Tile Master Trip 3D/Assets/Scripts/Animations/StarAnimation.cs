using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class StarAnimation : MonoBehaviour
{
    [SerializeField] private GameObject prefabsStar;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameUI gameUI;
    [SerializeField] Transform transformInit;
    [SerializeField] Transform target;

    private void Awake()
    {
        gameManager.OnTileMatching += StartAnimation;
    }

    public void StartAnimation(int cointInGame)
    {
        GameObject star = Instantiate(prefabsStar, transformInit.position, Quaternion.identity);
        star.transform.SetParent(transformInit);
        star.transform.DOMove(target.position, 0.4f).SetEase(Ease.Linear);
        StartCoroutine(WaitMoveStar(star, cointInGame));
    }

    IEnumerator WaitMoveStar(GameObject star, int cointInGame)
    {
        yield return new WaitForSecondsRealtime(0.4f);
        Destroy(star);
        target.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f);
        gameUI.SetCoinInGame(cointInGame);
    }
}
