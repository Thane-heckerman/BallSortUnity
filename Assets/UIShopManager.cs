using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class UIShopManager : MonoBehaviour
{
    public static UIShopManager Instance { get; private set; }
    public static event EventHandler OnShopUIEnable;
    private CoinAmount coinAmount = new CoinAmount();
    public int coinsOwned
    {
        get { return coinAmount.totalCoinOwned; }
        set { coinAmount.totalCoinOwned = value; }
    }
    [SerializeField] private Transform ballPreviewTransform;
    [SerializeField] private Transform shopContentTransform;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        Instance = this;
    }

    public void Spend(int amount)
    {
        coinAmount.Spend(amount);
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = coinsOwned.ToString();
    }

    private void OnEnable()
    {
        coinText.text = coinAmount.totalCoinOwned.ToString();
        OnShopUIEnable?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        ShopUIPreview.Instance.Clear();
    }
}
