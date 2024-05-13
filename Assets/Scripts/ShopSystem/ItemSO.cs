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

    public virtual void SetItemKey()
    {
        itemKey = string.Format($"{itemType}_{itemKey}");
    }

}

public enum ItemType
{
    ShopItem,
    RewardItem,
}
