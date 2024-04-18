using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ShopItem/Item")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public Sprite ownedSprite;
    public int costAmount;
    public bool isOwned;

    public void SetStatus(bool Owned) {
        isOwned = Owned;
    }



}
