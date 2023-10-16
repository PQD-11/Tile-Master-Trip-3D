using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public Transform target;
    public float duration = 0.01f;
    private Rigidbody rgb;
    private BoxCollider boxCollider;
    private Vector3 upScale, downScale;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        rgb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }


    private void OnMouseDown()
    {
        rgb.useGravity = false;
        boxCollider.enabled = false;

        upScale = transform.localScale * 1.2f;
        transform.DOScale(upScale, -1);
    }

    private void OnMouseUp()
    {
        downScale = transform.localScale * 0.7f;
        transform.DOScale(downScale, 0.5f);

        gameManager.AddToContainer(gameObject); // co the dung async

        Vector3 startPos = transform.position;
        Vector3 endPos = target.position;
        Vector3 midPos = new Vector3((startPos.x + endPos.x) / 2, startPos.y + 2, (startPos.z + endPos.z) / 2);

        transform.DOPath(new Vector3[] { startPos, midPos, endPos }, 0.2f, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() => Debug.Log("Hoàn thành di chuyển"));
    }
}
