using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSettings
{
    // lưu thông tin setting của prefab ball
    // validate setting của ball bằng cách ref đến list active ball và điều chỉnh theo thông số cài đặt

    public Sprite ballSprite;
    public float diameterMargin { get { return radiusMargin * 2; } }
    public float radiusMargin = .5f;
    public Vector3 ballDiameter;
    public BallData[] ballDataArray;
    public BallRuntimeSetList ballList;
    private int defautBallDataCount = -1;

    public BallSettings() {
        
    }
    public void Validate()
    {
        CheckValidateBallDataArray();
    }

    public void CheckValidateBallDataArray()
    {
        //loop through array and create
        //create constructor and call this constructor in game manager
            
        //if(ballList.Items.Count < defautBallDataCount) return;
        //for (int i = 0; i <= ballList.Items.Count; i++)
        //{
            
        //}
    }

    public Vector3 GetBallSize()
    {
        ballDiameter = ballSprite.bounds.size;
        return ballDiameter;
        //clamp theo size của tube
    }
}
