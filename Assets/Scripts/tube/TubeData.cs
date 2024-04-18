using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class TubeData 
{
    public int index;
    public List<BallPos> ballPostList;
    public int maxBallPos = 4;
    public Tube tube;
    public bool isCompleted;

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void SetBallPosList(List<BallPos> ballPosList)
    {
        this.ballPostList = ballPosList;
    }


}


