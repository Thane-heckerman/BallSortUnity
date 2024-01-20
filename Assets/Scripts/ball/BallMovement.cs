using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BallMovement : MonoBehaviour
{
    private MOVEMENT_STATE currentState;
    //public GameObject targetPos;
    protected float moveSpeed = 0.1f;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float progress;
    [SerializeField] private Vector3 currentPos;
    [SerializeField] private int currentTargetIndex; // Thêm biến này để theo dõi mục tiêu hiện tại
    [SerializeField] private List<GameObject> targetList; // Thêm biến này để lưu danh sách mục tiêu

    private void Start()
    {
        currentPos = transform.position;
        progress = 0;
        currentState = MOVEMENT_STATE.NOT_MOVING;
        targetPos = Vector3.zero;
        currentTargetIndex = 0;
    }

    private void Update()
    {
        if (currentState == MOVEMENT_STATE.NOT_MOVING) return;

        MovingBall();
    }

    public void StartMoveBall(List<GameObject> targets)
    {

        targetList = targets;

        if (transform.position == targetPos)
        {
            // Debug.Log("vì 2 vị trí giống nhau nên chạy vào đây");
            if (currentTargetIndex >= targetList.Count)
            {
                // Debug.Log("targetlistcount là " + targetList.Count);
                // Debug.Log("chạy xuống targets clear");
                targets.Clear();
                currentState = MOVEMENT_STATE.NOT_MOVING;
                currentTargetIndex = 0;
                // Debug.Log("hoàn thành 1 vòng di chuyển ball");
                return;
            }
        }
        currentState = MOVEMENT_STATE.MOVING;

        // Debug.Log("vị trí hiện tại là " + transform.position);

        targetPos = targetList[currentTargetIndex].transform.position; // Cập nhật targetPos với mục tiêu hiện tại

        // Debug.Log("target pos thứ" + currentTargetIndex + "là " + targetPos);

        // Debug.Log("số @hần tử trong targetList là" + targetList.Count);
        // Debug.Log("index đang là" + currentTargetIndex);
    }

    private void MovingBall()
    {
        // Kiểm tra currentState trước khi gọi MoveBall
        if (currentState == MOVEMENT_STATE.MOVING)
        {
            MoveBall(targetPos);
        }
    }

    public void MoveBall(Vector3 target)
    {
        Debug.Log("target list count trước khi move ball" + targetList.Count);

        progress += Time.deltaTime * moveSpeed;// aka step

        // gameObject.transform.position = Vector3.Lerp(transform.position, target, progress);

        gameObject.transform.position = Vector3.MoveTowards(transform.position, target, progress);





        // Kiểm tra nếu đã di chuyển đến mục tiêu
        if (Vector3.Distance(transform.position, target) <= 0.1f)
        {
            transform.position = target;

            currentPos = transform.position;

            // Debug.Log("set currentPos" + currentPos);

            // Cập nhật mục tiêu tiếp theo

            // Debug.Log("targetListcount " + targetList.Count);

            if (currentTargetIndex < targetList.Count)
            {
                currentTargetIndex++;

                // targetPos = targetList[currentTargetIndex].transform.position;

                StartMoveBall(targetList);

                // Debug.Log("index tiếp theo để gọi là " + currentTargetIndex);

            }
            else
            {
                currentState = MOVEMENT_STATE.NOT_MOVING;

            }
        }

    }
}// 
public enum MOVEMENT_STATE
{
    MOVING,
    NOT_MOVING
}
