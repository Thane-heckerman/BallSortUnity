using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class BuyBtn : MonoBehaviour
{
    private Button button;
    [SerializeField] TextMeshProUGUI tmp;
    public static event EventHandler OnBuyShopItem ;

    private void OnEnable()
    {
        ShopItem.OnPreviewValueChanged += ShopItem_OnPreviewValueChanged;
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            OnBuyShopItem?.Invoke(this, EventArgs.Empty);
        });
    }

    private void OnDisable() {
        ShopItem.OnPreviewValueChanged -= ShopItem_OnPreviewValueChanged;
    }


    private void ShopItem_OnPreviewValueChanged(object sender, ShopItem.OnPreviewValueChangedEventArgs e)
    {
        tmp.text = e.cost.ToString() ;
    }
}
