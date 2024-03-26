using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum TubeState {
    NONE,
    Completed,
}

public class Tube : MonoBehaviour
{
    public Tube tube;
    public TubeState tubeState;
    private Rigidbody2D rb2d;
    public Sprite testingSprite;
    public int index;
    private GameObject tubeGO;
    private int maxBallInTube = 4;
    public TubeData tubeData;
    private List<BallPos> ballPosList = new List<BallPos>();
    [SerializeField] private Transform lastBallPos;
    [SerializeField] private Transform upBallPos;
    private bool canReceiveBall;
    
    public Vector3 spacing;
    public static Tube Create(Transform tubePrefab,Vector3 position, int index)
    {
        GameObject tubeGO = Instantiate(tubePrefab, position, Quaternion.identity).gameObject;
        Tube tube = tubeGO.GetComponent<Tube>();
        tube.tubeData.index = index;
        tube.SetIndex(index);
        tube.SetGameObject(tubeGO);
        return tube;
    }
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lastBallPos.gameObject.SetActive(false);
        tubeData.SetBallPosList(ballPosList);
    }

    public void InitBallPos()
    {
        spacing = testingSprite.bounds.size;
        spacing.x = 0;
        spacing.z = 0;
        for (int i = 0; i< maxBallInTube; i++)
        {
            Vector2 ballPosPosition = lastBallPos.transform.position + i * spacing;
            BallPos ballPos = BallPos.Create(this,lastBallPos, ballPosPosition, this.index, i);
            ballPos.ballPosData.SetData(GetTubeIndex(),i, ballPosPosition);
            ballPosList.Add(ballPos);
        }
        
    }

    // for receiving balls

    public bool IsNotBusy()
    {
        foreach (var ballPos in ballPosList)
        {
            if (ballPos.ballPosData.isOccupied)
            {
                if (ballPos.ballPosData.ball.GetBallState() == BallState.NONE)
                {
                    return true;
                }
            }
            else return true;
        }

        return false;
    }
    //public int GetFirstEmtyBallPos()
    //{
    //    if (!HasNoBall())
    //    {
    //        BallPos ballPos = ballPosList.First(i => !i.IsContainBall());
    //        return ballPos.ballPosData.index;
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}

    public Transform GetUpBallPos()
    {
        return upBallPos;
    }

    public bool CanReceiveBall(int NumberOfBall, BallTypeSO ballType)
    {
        
        int remainingBallPos = 0;
        if (!HasBall()) return true;
        if (GetLastBall().ballData.ballType == ballType)
        {
            remainingBallPos = 4 - GetAllBallInTubeCount();
            if (NumberOfBall <= remainingBallPos) return true;
        }

        return false;
    }

    private void AssignOneBall(Ball ball)
    {
        throw new NotImplementedException();
    }

    // for poping balls


    public void PopBall()
    {
        if (HasBall())
        {
            Ball lastBall = GetLastBall();
            BallMovement ballMovement = lastBall.GetComponent<BallMovement>();
            ballMovement.StartCoroutine(ballMovement.MoveToTarget(upBallPos.position, null));
        }
        else return;
    }

    public void GetLastBallPopedBack()
    {
        Ball lastBall = GetLastBall();
        BallMovement movement = lastBall.GetComponent<BallMovement>();
        movement.StartCoroutine(movement.MoveToTarget(ballPosList[GetAllBallInTubeCount() - 1].ballPosData.position));
    }

    public bool CanPopBall()
    {
        return !IsCompletedTube() && HasBall();
    }

    public List<Ball> GetAllSameColorNeighBorBall()
    {
        List<Ball> allSameColorBallWithLastBall = new List<Ball>();
        Ball lastBall = GetLastBall();
        allSameColorBallWithLastBall.Add(lastBall);
        int lastBallIndex = lastBall.GetIndex();
        for (int i = lastBallIndex - 1; i>=0; i--)
        {
            Ball ball = GetBallByIndex(i);
            if (GetBallByIndex(i).ballData.ballType == lastBall.ballData.ballType)
            {
                allSameColorBallWithLastBall.Add(ball);
            }
        }
        return allSameColorBallWithLastBall;
    }

    public int GetNumberOfSameColor()
    {
        List<Ball> balls = GetAllSameColorNeighBorBall();
        return balls.Count;
    }

    public void ClearPopedBallPos(int index)
    {
        ballPosList[index].ballPosData.ClearData();
    }

    private Ball GetBallByIndex(int index)
    {
        Ball ball = ballPosList[index].GetBallComponent();
        return ball;
    }

    
    public Ball GetLastBall()
    {
        if (HasBall())
        {
            Ball lastBall = ballPosList.Last(i => i.IsContainBall()).GetBallComponent();
            return lastBall;
        }
        else return null;
    }

    
    public List<BallPos> GetBallPosList()
    {
        return ballPosList;
    }

    private bool HasBall()
    {
        return GetAllBallInTubeCount() != 0;
    }

    public int GetAllBallInTubeCount()
    {
        int currentBallCount = 0;
        for (int i = 0; i < ballPosList.Count; i++)
        {
            if (ballPosList[i].ballPosData.ball != null)
            {
                currentBallCount++;
            }
        }
        return currentBallCount;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public GameObject GetGameObject()
    {
        return tubeGO;
    }

    public int GetTubeIndex()
    {
        return index;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void SetGameObject(GameObject gameObject)
    {
        this.tubeGO = gameObject;
    }

    public bool IsCompletedTube()
    {
        if (GetAllBallInTubeCount() != maxBallInTube) return false;
        BallTypeSO ballType = GetLastBall().ballData.ballType;
        var ballList = ballPosList.Select(b => b.ballPosData.ball).Where(b=>b.ballData.ballType == ballType).ToList();
        if (ballList.Count == 4) return true;
        return false;
    }

    public void Reposition(Vector2 pos ) {
        transform.position = pos;
        foreach(var ballPos in ballPosList)
        {
            ballPos.Reposition(new Vector2(pos.x, ballPos.ballPosData.position.y));
            if (ballPos.IsContainBall()) {
                ballPos.ballPosData.ball.transform.position = ballPos.transform.position;
            }
        }
    }
}
