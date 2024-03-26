using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class BallPos : MonoBehaviour
{
    public BallPosData ballPosData = new BallPosData();
    private bool isContainBall;

    private void Start()
    {
    }

    private void Instance_OnTubeClickEvent(object sender, System.EventArgs e)
    {
        Clear();
    }

    public void Clear()
    {
        ballPosData.ClearData();
        isContainBall = false;
    }

    public static BallPos Create(Tube tube,Transform prefab, Vector2 position,int tubeIndex,int index)
    {
        Transform BallPosTransform = Instantiate(prefab, position, Quaternion.identity);
        BallPosTransform.SetParent(tube.transform);
        BallPosTransform.gameObject.SetActive(true);
        BallPos ballPos = BallPosTransform.GetComponent<BallPos>();
        return ballPos;
    }

    public Ball GetBallComponent()
    {
        return ballPosData.Ball;
    }

    public bool IsContainBall()
    {
        return GetBallComponent() != null;
    }

    public void AssignBall(Ball ball)
    {
        ballPosData.ball = ball;
        ball.SetIndex(ballPosData.index);
        ballPosData.isOccupied = true;
    }

    public void Reposition(Vector2 pos)
    {
        transform.position = pos;
        ballPosData.position = pos;
    }
}

[System.Serializable]

public class BallPosData {
    public Vector2 position;
    public int tubeIndex;
    public int ballIndex;
    public int index;
    public bool isOccupied;
    public BallPos ballPos;
    public Ball ball;
    public Ball Ball
    {
        get { return ball ?? null; }
        set
        {
            if (isOccupied)
            {
                return;
            }
            else
            {
                ball = value;
            }
        }
    }

    public void SetData (int tubeIndex,int index,Vector2 position)
    {
        this.tubeIndex = tubeIndex;
        this.index = index;
        this.position = position;
    }

    public void SetBallObj(Ball ball)
    {
        this.ball = ball;
        isOccupied = true;
    }

    public void ClearData()
    {
        this.ball = null;
        isOccupied = false;
    }

    
    

}