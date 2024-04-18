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
        if (pileOfCoins.GetComponent<MovingCoin>().isBusy) return;
        transform.DOScale(new Vector2(1.2f, 1.2f), .5f);
        pileOfCoins.gameObject.SetActive(true);
        gameObject.SetActive(false);
        OnGiftClicked?.Invoke(this, EventArgs.Empty);
    }
}
