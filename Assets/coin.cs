using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class coin : MonoBehaviour
{
    public bool isBusy;

    private Transform target;

    private void Update()
    {
        CheckBusy();
    }

    public bool IsMovingToTarget(Transform target, float delay)
    {
        SetTarget(target);

        isBusy = true;

        gameObject.SetActive(true);

        transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

        transform.DOMove(target.position, 0.5f).SetDelay(delay + 0.5f)
            .SetEase(Ease.OutBack);

        transform.DOScale(0f, 0.3f).SetDelay(delay + 1.5f);

        return isBusy;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private bool CheckBusy()
    {
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {

            isBusy = false;
        }
        else isBusy = true;
        return isBusy;
    }

    
}
