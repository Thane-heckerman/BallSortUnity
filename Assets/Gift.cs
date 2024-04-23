using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class Gift : MonoBehaviour
{
    private int goldAmount;
    public Vector2 position;
    public static event EventHandler OnGiftClicked;

    [SerializeField] private Transform pileOfCoins;

    private void OnEnable()
    {
        position = transform.position;
        GetComponent<Button>().onClick.AddListener(() => OnClickAnim());
    }


    private void OnClickAnim()
    {
        Debug.Log("gift clicked");
        if (!GiftManager.Instance.canReceiveMoreGift) return;
        transform.DOScale(new Vector2(1.2f, 1.2f), .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            CoinPile.Instance.SpawnCoin();
        });
        OnGiftClicked?.Invoke(this, EventArgs.Empty);
    }
}
