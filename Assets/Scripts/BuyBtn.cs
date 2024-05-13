using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DataStorage;
public class BuyBtn : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;
    public bool ownedStatus;
    public int currentShopItemIndex;
    private void OnEnable()
    {
        ShopItem.OnPreviewValueChanged += ShopItem_OnPreviewValueChanged;
        GetComponent<Button>()
        .onClick.AddListener(() =>
        {
            //OnBuyShopItem?.Invoke(this, EventArgs.Empty);
            // gọi đế Uishopmanager hàm buy Item không thông qua event nữa
            Debug.Log("onclick buy btn");
            
            Debug.Log("buy!!!");
            UIShopManager.Instance.BuyItem();
            
        });
    }

    private void OnDisable() {
        ShopItem.OnPreviewValueChanged -= ShopItem_OnPreviewValueChanged;
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void ShopItem_OnPreviewValueChanged(object sender, ShopItem.OnPreviewValueChangedEventArgs e)
    {
        if (e.ownedStatus)
        {
            Debug.Log("item#" + e.value + "owned status :" + e.ownedStatus);
            tmp.text = "OWNED";
            ownedStatus = e.ownedStatus;
        }
        else
        {
            tmp.text = e.cost.ToString();
        }
    }
}
