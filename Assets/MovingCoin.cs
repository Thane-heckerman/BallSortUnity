using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingCoin : MonoBehaviour
{
    public bool isBusy;

    public static MovingCoin Instance;

    [SerializeField] private Transform coinMovingTarget;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Gift.OnGiftClicked += Gift_OnGiftClicked;
    }

    private void OnDisable()
    {
        Gift.OnGiftClicked -= Gift_OnGiftClicked;
    }

    private void Gift_OnGiftClicked(object sender, System.EventArgs e)
    {
        AppearOnPosition();
        movingToTarget();
    }

    public void AppearOnPosition()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform coinChild = transform.GetChild(i);
            coinChild.DOScale(1, .2f);
            coinChild.transform.DOMove(new Vector3(transform.position.x + Random.Range(-150,150), transform.position.y + Random.Range(-150, 150)),.1f);

        }
    }

    private void movingToTarget()
    {
        Reset();
        float delay = 0;
        for (int i = 0; i< transform.childCount; i++)
        {

            Transform coinChild = transform.GetChild(i);

            coin coin = coinChild.GetComponent<coin>();

            coin.IsMovingToTarget(coinMovingTarget,delay);

            delay += 0.2f;
        }

    }

    private void FixedUpdate()
    {
        isBusy = !IsAllCoinMovedToTarget();
    }

    public bool IsAllCoinMovedToTarget()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform coinChild = transform.GetChild(i);
            if (coinChild.GetComponent<coin>().isBusy)
            {
                return false;
            }
        }
        return true;
    }

    public IEnumerator CoinMovingAnimation()
    {
        yield return new WaitForEndOfFrame();
        movingToTarget();
        
    }

    private void Reset()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform coinChild = transform.GetChild(i);
            coinChild.transform.position = this.transform.position;
        }
    }
}
