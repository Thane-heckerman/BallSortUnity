using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class MovingCoin : MonoBehaviour
{
    //public static event EventHandler<OnCoinMovedToTargetEventArgs> OnCoinMovedToTarget;

    //public class OnCoinMovedToTargetEventArgs
    //{
    //    public int amount;
    //}

    public bool isBusy;

    private Transform coinMovingTarget;

    public void SetTarget(Transform target)
    {
        this.coinMovingTarget = target;
    }

    //public IEnumerator MoveToTarget(Transform target, float delay)
    //{
    //    AppearOnPosition(target.position);

    //    SetTarget(target);

    //    isBusy = true;

    //    gameObject.SetActive(true);

    //    transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

    //    transform.DOMove(target.position, 0.5f).SetDelay(delay + 0.5f)
    //        .SetEase(Ease.OutBack);

    //    transform.DOScale(0f, 0.3f).SetDelay(delay + 1.5f).OnComplete(() => GetComponent<coin>().Collect());

    //    isBusy = false;

    //    yield return new WaitForEndOfFrame();
    //}

    public void AppearOnPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x + UnityEngine.Random.Range(-150f, 50f), position.y - 50f, 0f);
    }

}
