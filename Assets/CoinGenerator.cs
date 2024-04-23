using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStorage;
using System;
public class CoinGenerator : MonoBehaviour //has not been added yet
{

    public string OWNED_COIN_AMOUNT = "OWNED_COIN_AMOUNT";
    public int ownedCoinAmount
    {
        get => GameData.Get(OWNED_COIN_AMOUNT, 0);
        set => GameData.Set(OWNED_COIN_AMOUNT, value);
    }

    public void Add(int amount)
    {
        ownedCoinAmount += amount;
    }

    public int GetOwnedCoinAmount() {
        return ownedCoinAmount;
    }

    public void Spend(int amount)
    {
        ownedCoinAmount -= amount;
    }


}
