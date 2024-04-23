using System.Collections;
using System.Collections.Generic;
using DataStorage;
using UnityEngine;


public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemSO item;
    public bool ownedStatus;
    [SerializeField] private int itemQuantity;
    [SerializeField] private Sprite sprite;

    public bool isOwned
    {
        get => GameData.Get($"Item_{item.itemKey}_Status", false);
        set {
            if(!isOwned)
            GameData.Set($"Item_{item.itemKey}_Status", value);
            }
    }

    protected void Add(int amount) {

        itemQuantity += amount;
    }


    protected void Spend(int amount)
    {
        itemQuantity -= amount;
    }

    protected abstract void Awake();
    
    public int GetItemQuantity()
    {
        return GameData.Get($"Item_{item.itemKey}_Amount", 0);
    }

    public void SaveData()
    {
        GameData.Set($"Item_{item.itemKey}_Amount", itemQuantity);
        GameData.Set($"Item_{item.itemKey}_Status", ownedStatus);
    }
}
