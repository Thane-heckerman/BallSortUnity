using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class GiftManager : MonoBehaviour
{
    private int maxGift = 3;
    private int giftClicked = 0;
    public bool canReceiveMoreGift;
    [SerializeField] private TextMeshProUGUI coinText;
    public static GiftManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        canReceiveMoreGift = true;
        Gift.OnGiftClicked += Gift_OnGiftClicked;
    }

    private void Gift_OnGiftClicked(object sender, System.EventArgs e)
    {
        giftClicked++;

        if(giftClicked == maxGift)
        {
            canReceiveMoreGift = false;
            StartCoroutine(Hide());
            giftClicked = 0;
        }
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(1.8f);
        yield return new WaitUntil(()=> !transform.Find("Pile_of_coin").GetComponent<CoinPile>().isBusy);
        transform.DOScale(0f, .3f).SetDelay(.3f).OnComplete(() => gameObject.SetActive(false));
        canReceiveMoreGift = true;
    }

}
