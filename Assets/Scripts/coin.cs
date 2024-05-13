using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using DataStorage;

public class coin : Item, IColectable
{
    public int Amount;

    public bool isBusy;

    private Transform target;

    private void OnEnable()
    {
        Amount = Random.Range(10, 50);
    }

    public void Collect()
    {
        CoinPile.Instance.AddCoin(Amount);
    }
}
