using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemSO/ShopItem")]
public class ShopItemSO : ItemSO
{
    public int costAmount = 175;
    private Sprite img;
    private void Awake()
    {
        base.SetItemKey();
    }
}
