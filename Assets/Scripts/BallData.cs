using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
[System.Serializable]
public class BallData
{

    public BallColor color;
    //public BallColor Color
    //{
    //    get { return color; }
    //    set
    //    {
    //        color = value;
    //    }
    //}
    public BallTypeSO ballType;
    public int index;
    public Transform position;
    public Ball ball;

    public void SetBallType(BallTypeSO ballType)
    {
        this.ballType = ballType;
    }

    public void SetBallColor(BallColor ballColor)
    {
        this.color = ballColor;
    }

    
}
