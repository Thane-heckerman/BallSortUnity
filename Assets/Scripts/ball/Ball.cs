using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditorInternal;
using UnityEngine;

public enum BallColor
{
    NONE,
    RED,
    GREEN,
    YELLOW,
    PURPLE,
    TEAL,
    ORANGE,
    BLUE
}

public enum BallState {
    NONE,
    MOVING
}

public class Ball : MonoBehaviour
{
    private BallState ballState;
    public Ball bottomNeighbor;
    public BallData ballData = new BallData();
    private BallPos ballPos;
    private IColorableComponent colorableComponent;
    private bool canGoOut;

    public static Ball Create(Transform ballPos)
    {
        Transform ballPrefab = Resources.Load<Transform>("BallPrefab");
        GameObject ballGO = Instantiate(ballPrefab, ballPos.position, Quaternion.identity).gameObject;
        Ball ball = ballGO.GetComponent<Ball>();
        return ball;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    
    public int GetIndex()
    {
        return ballData.index;
    }

    public BallState GetBallState()
    {
        return ballState;
    }

    public void SetIndex(int index)
    {
        ballData.index = index;
    }

    public bool IsMoving()
    {
        return ballState == BallState.MOVING;
    }
}
