using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

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
    private ShopContentSpawner shopContentSpawner;
    private List<ShopItem> shopItems = new List<ShopItem>();
    [SerializeField] private TextMeshProUGUI notEnoughCoinText;
    private float showTextTimer = 2f;
    private void Awake()
    {
        shopContentSpawner = GetComponent<ShopContentSpawner>();
        Instance = this;
    }

    public void Spend(int amount)
    {
        coinAmount.Spend(amount);
        
    }

    private void UpdateCoinText()
    {
        coinText.text = coinAmount.totalCoinOwned.ToString();
    }

    private void OnEnable()
    {
        HideText();
        if (shopItems.Count == 0)
        {
            shopItems = shopContentSpawner.SpawnContent();
        }
        UpdateCoinText();
        OnShopUIEnable?.Invoke(this, EventArgs.Empty);
        //BuyBtn.OnBuyShopItem += BuyBtn_OnBuyShopItem;
    }


    public void BuyItem()
    {
        var item = GetShopItem(ShopUIPreview.Instance.PreviewBallTypeListIndex);
        if (coinsOwned > item.costAmount)
        {
            GetShopItem(ShopUIPreview.Instance.PreviewBallTypeListIndex).
                  GetComponent<IColectable>().Collect();
            coinAmount.Spend(item.costAmount);
        }
        else
        {
            ShowText($"not enough gold for item {GetShopItem(ShopUIPreview.Instance.PreviewBallTypeListIndex).Index}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            ResetAllBoughtItem();
        }
    }

    private void OnDisable()
    {
        //ClearContent();
        //shopItems.Clear();
        ShopUIPreview.Instance.Clear();
        //BuyBtn.OnBuyShopItem -= BuyBtn_OnBuyShopItem;
    }

    private void BuyBtn_OnBuyShopItem(object sender, System.EventArgs e)
    {
        GetShopItem(ShopUIPreview.Instance.PreviewBallTypeListIndex).
                  GetComponent<IColectable>().Collect();
        UpdateCoinText();
    }

    private ShopItem GetShopItem(int index)
    {
        ShopItem item = shopItems.Where(i => i.Index == index).
                  FirstOrDefault();
        return item;
    }

    public bool CheckItemOwnedStatus(int index)
    {
        return GetShopItem(index).isOwned;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void HideText()
    {
        notEnoughCoinText.gameObject.SetActive(false);
    }

    public void ShowText(string text)
    {
        notEnoughCoinText.gameObject.SetActive(true);
        Counter.Instance.CompleteCountDownCallBack = new Action(HideText);
        notEnoughCoinText.text = text;
        StartCoroutine(Counter.Instance.CountDown(showTextTimer, () => showTextTimer <= 0f));
    }

    private void ResetAllBoughtItem()
    {
        foreach(var item in shopItems)
        {
            item.isOwned = false;
            Debug.Log("reset" + item.Index);
        }
    }
}
