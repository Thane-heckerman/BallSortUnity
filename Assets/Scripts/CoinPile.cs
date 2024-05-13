using System.Collections.Generic;
using System.Collections;
using DataStorage;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System;

public class CoinPile : MonoBehaviour
{

    public static CoinPile Instance { get; private set; }
    public static event EventHandler<OnCoinAmountChangedEventArgs> OnCoinAmountChanged;
    public class OnCoinAmountChangedEventArgs
    {
        public int amount;
    }
    public bool isBusy;

    public int CoinNumber;

    public CoinAmount coinAmount = new CoinAmount();

    private CoinSpawner coinSpawner;
    
    public List<coin> coins;

    [SerializeField] private Transform target;

    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        Instance = this;
        coinSpawner = GetComponent<CoinSpawner>();
    }

    private void OnEnable()
    {
        UpdateCoinText();
        //Gift.OnGiftClicked += Gift_OnGiftClicked;
    }

    private void OnDisable()
    {
        //Gift.OnGiftClicked -= Gift_OnGiftClicked;
        foreach(var coin in coins)
        {
            Destroy(coin.gameObject);
        }
        coins.Clear();
    }

    public bool IsAllCoinMovedToTarget()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            var coinChild = coins[i];

            if (coinChild.isBusy)
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnCoin()
    {
       
        coinSpawner.numberToSpawn = UnityEngine.Random.Range(5, 7);
        coins = coinSpawner.SpawnABunch();
        foreach(var coin in coins)
        {
            coin.transform.SetParent(this.transform);
        }
        AppearCoinPile();
        StartCoroutine(MoveCoins(0.2f));
    }

    private void AppearCoinPile()
    {
        foreach( var coin in coins)
        {
            coin.GetComponent<MovingCoin>().AppearOnPosition(this.transform.position);
            
        }
    }

    public void AddCoin(int amount)
    {
        //Debug.Log("add coins" + amount);
        coinAmount.Add(amount);
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = coinAmount.GetOwnedCoinAmount().ToString();
    }

    public int GetOwnedCoinAmount()
    {
        return coinAmount.GetOwnedCoinAmount();
    }

    public IEnumerator MoveCoins(float delay)
    {
        for (int i = 0; i < coins.Count; i++)
        {
            coin coin = coins[i];
            coin.isBusy = true;
            int amount = coins[i].Amount;
            coins[i].transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            coins[i].transform.DOMove(target.transform.position, 0.8f)
                        .SetDelay(delay + 0.5f).SetEase(Ease.InBack).OnComplete(()=> AddCoin(amount));
            
            coins[i].transform.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                        .SetEase(Ease.Flash);

            coins[i].transform.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);
            delay += 0.1f;
            target.transform.DOScale(1.1f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f)
                .OnComplete(() => coin.isBusy = false);
            
            yield return null;
        }
    }
    
}

public class CoinAmount
{
    private string totalCoinAmountKey = "TOTAL_COIN_OWNED";

    public int totalCoinOwned
    {
        get => GameData.Get(totalCoinAmountKey, 0);
        set => GameData.Set(totalCoinAmountKey, value);
    }

    public void Add(int amount)
    {
        totalCoinOwned += amount;
    }

    public int GetOwnedCoinAmount()
    {
        return totalCoinOwned;
    }

    public void Spend(int amount) {
        totalCoinOwned -= amount;
    }

}