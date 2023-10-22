using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] public Image image;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Vector3 upScale, downScale, baseScale;
    private Vector3 startPos;
    private Quaternion startRotation;
    public event Action<Tile> OnClick;
    public event Action<Tile> OnMoveCompleted;

    private void Awake()
    {
        baseScale = transform.localScale;

        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Initialized(Sprite sprite)
    {
        image.sprite = sprite;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            rb.velocity = -rb.velocity * 2;
        }
    }
    private void OnMouseDown()
    {
        rb.useGravity = false;
        boxCollider.enabled = false;

        upScale = transform.localScale * 1.2f;
        transform.DOScale(upScale, -1);
    }

    private void OnMouseUp()
    {
        OnClick?.Invoke(this);

        startPos = transform.position;
        startRotation = transform.rotation;
    }

    public void MoveToContainer(Transform destination)
    {
        downScale = transform.localScale * 0.7f;
        transform.DOScale(downScale, 0.5f);

        Vector3 endPos = destination.position;
        Vector3 midPos = new Vector3((startPos.x + endPos.x) / 2, startPos.y + 2, (startPos.z + endPos.z) / 2);

        transform.DOPath(new Vector3[] { startPos, midPos, endPos }, 0.2f, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.rotation = Quaternion.identity;
                OnMoveCompleted?.Invoke(this);
            });
    }

    public void MoveToBack()
    {
        transform.DOScale(baseScale, 0.5f);
        transform.DOMove(startPos, 0.1f)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            transform.rotation = startRotation;
            rb.useGravity = true;
            boxCollider.enabled = true;
        });
    }
}
