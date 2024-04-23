using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStorage;

[CreateAssetMenu(menuName ="ItemSO/Item")]

public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public string itemKey;

    public void SetItemKey(string id) {
        itemKey = string.Format($"{itemType}_{itemName}_{id}");
    }

}

public enum ItemType
{
    ShopItem,
    RewardItem,
}
