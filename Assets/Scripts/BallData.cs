using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class BallData
{

    public BallColor color;
    public BallColor Color
    {
        get { return color; }
        set
        {
            color = value;
        }
    }
    public BallTypeSO ballType;
    public int index;
    public Transform position;
    public Ball ball;

    public BallData()
    {

    }

    public void SetBallType(BallTypeSO ballType)
    {
        this.ballType = ballType;
    }

}
