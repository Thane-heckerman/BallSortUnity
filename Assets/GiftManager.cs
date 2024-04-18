using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GiftManager : MonoBehaviour
{
    private int maxGift = 3;
    private int giftClicked;
    private bool canReceiveMoreGift;

    private void OnEnable()
    {
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
        yield return new WaitUntil(()=> !transform.Find("Pile_of_coin").GetComponent<MovingCoin>().isBusy);
        transform.DOScale(0f, .3f).SetDelay(.3f).OnComplete(() => gameObject.SetActive(false));
    }

}
