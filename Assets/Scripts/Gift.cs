using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class Gift : MonoBehaviour
{
    public static event EventHandler OnGiftClicked;
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClickAnim());
    }


    private void OnClickAnim()
    {
        Debug.Log("gift clicked");
        if (!GiftManager.Instance.canReceiveMoreGift) return;
        transform.DOScale(new Vector2(1.2f, 1.2f), .5f).OnComplete(() =>
        {
            transform.localScale = Vector3.zero;
            CoinPile.Instance.SpawnCoin();
        });
        OnGiftClicked?.Invoke(this, EventArgs.Empty);
    }
}
