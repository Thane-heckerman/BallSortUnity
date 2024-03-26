using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TubeTemplate : MonoBehaviour
{
    [SerializeField] public List<BallPosForEditor> ballPosFEList = new List<BallPosForEditor>();

    public static int maxBallPos = 4;

    public void Init()
    {
        if (ballPosFEList.Count == 0)
        {
            AddItems();
            for (int i = 0; i< maxBallPos; i++)
            {
                ballPosFEList[i] = GetBallPos(i);
            }
        }
    }

    public void AddItems()
    {
        ballPosFEList.Add(new BallPosForEditor(0, new Vector2(0,0),false));
    }

    public void RemoveItem()
    {
        ballPosFEList.RemoveAt(ballPosFEList.Count - 1);
    }
    public BallPosForEditor GetBallPos(int col)
    {
        var ballPos = ballPosFEList[col];
        if(ballPos != null)
        {
            ballPos.position = new Vector2(0, col);
        }
        if(ballPos == null)
        {
            ballPos = new BallPosForEditor(col, new Vector2(0,col),true);
        }
        return ballPos;
    }

}

[System.Serializable]
public class BallPosForEditor
{
    public BallTypeListSO ballTypeList;
    public BallTemplate ball = new BallTemplate();
    public Vector2 position;
    public bool isContainBall;

    public BallPosForEditor(int col, Vector2 position, bool isContainBall)
    {
        this.position = position;
        this.isContainBall = isContainBall;
        if (isContainBall)
        {
            ball.ballType = ballTypeList.ballTypeList[0];
        }
    }

    public BallTemplate GetBallTemplate(int col)
    {
        if (isContainBall)
        {
            return ball;
        }
        else return null;
    }

    public BallTypeSO GetBallType(int col)
    {
        return ball.ballType;
    }
}

[System.Serializable]
public class BallTemplate
{
    public BallTypeSO ballType;
}