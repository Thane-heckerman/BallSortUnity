using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallMovement : MonoBehaviour
{

    public BallState ballState;
    //public GameObject targetPos;
    protected float moveSpeed = 0.1f;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float progress;
    [SerializeField] private Vector3 currentPos;
    [SerializeField] private int currentTargetIndex; 
    private List<GameObject> targetList;
    private void Start()
    {
    }

    private void Update()
    {
        UpdateBallState();
    }

    private void UpdateBallState()
    {
        if(Vector2.Distance(transform.position, targetPos) < 0.001f)
        {
            ballState = BallState.NONE;
        }
    }

    public IEnumerator MoveToTarget(Vector3 pos1, Vector3? pos2 = null)
    {
        ballState = BallState.MOVING;
        Sequence sequence = DOTween.Sequence();
        Tween moveToUpPos = transform.DOMove(pos1,.2f).OnComplete(()=> transform.position = pos1);
        
        sequence.Append(moveToUpPos);
        yield return new WaitForSeconds(.2f);
        if (pos2 != null)
        {
            Tween moveToBallPos = transform.DOMove((Vector3)pos2, .2f).SetEase(Ease.InOutBounce).Play().
                OnComplete(()=> transform.position = (Vector3)pos2);
        }
        yield return new WaitForEndOfFrame();
    }

    //public IEnumerator MoveToTargetParam(params Vector3[] poses)
    //{
        
    //}

    public bool IsMovedToTarget(Vector3 pos)
    {
        if (transform.position == pos)
        {
            ballState = BallState.NONE;
            return true;
        }
        return false;
    }

   
    public void StartMoveBall(List<GameObject> targets)
    {

        targetList = targets;

        if (transform.position == targetPos)
        {
            if (currentTargetIndex >= targetList.Count)
            {
                targets.Clear();
                currentTargetIndex = 0;
                return;
            }
        }

        targetPos = targetList[currentTargetIndex].transform.position; // Cập nhật targetPos với mục tiêu hiện tại

    }

    
}

