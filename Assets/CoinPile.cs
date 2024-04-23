using System.Collections;
using System.Collections.Generic;
using DataStorage;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class CoinPile : MonoBehaviour
{

    public static CoinPile Instance { get; private set; }

    public bool isBusy;

    public int CoinNumber;

    public CoinAmount coinAmount = new CoinAmount();

    private CoinSpawner coinSpawner;
    
    private List<coin> coins;

    private bool coinSpawned;

    [SerializeField] private Transform target;

    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        Instance = this;
        coinSpawner = GetComponent<CoinSpawner>();
    }

    private void movingToTarget(Transform target)
    {
        float delay = 0;

        for (int i = 0; i < coins.Count; i++)
        {
            var coinChild = coins[i];

            coinChild.GetComponent<MovingCoin>().MoveToTarget(target,.2f);

            delay += 0.2f;
        }

    }

    private void OnEnable()
    {
        //UpdateCoinText();
        coinSpawned = false;
        //Gift.OnGiftClicked += Gift_OnGiftClicked;
    }

    private void OnDisable()
    {
        //Gift.OnGiftClicked -= Gift_OnGiftClicked;

    }

    private void Gift_OnGiftClicked(object sender, System.EventArgs e)
    {
        SpawnCoin();
        //if (coinSpawned)
        //{
        //    AppearCoinPile();
        //    //movingToTarget(target);
        //}
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
        coinSpawner.numberToSpawn = Random.Range(5, 7);
        coins = coinSpawner.SpawnABunch();
        foreach(var coin in coins)
        {
            coin.transform.SetParent(this.transform);
        }
        AppearCoinPile();
        coinSpawned = true;
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

    public void Spend(int amount)
    {
        coinAmount.Spend(amount);
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