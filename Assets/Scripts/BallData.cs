using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
[System.Serializable]
public class BallData
{
    public static event EventHandler<OnBallColorValueChangeEventArgs> OnBallColorValueChange;
    public class OnBallColorValueChangeEventArgs
    {
        public BallColor color;
    }
    private BallColor color;
    public BallColor Color
    {
        get { return color; }
        set
        {
            color = value;
            OnBallColorValueChange?.Invoke(this, new OnBallColorValueChangeEventArgs() { color = value, });
        }
    }
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
