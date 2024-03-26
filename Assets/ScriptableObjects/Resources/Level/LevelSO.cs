using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Levels/level")]
public class LevelSO : ScriptableObject
{
    public int MaxTubeNumber;
    public int NumberOfColor;
    //public List<sprite> levelSprites;
    public List<TubeData> tubeList;
    //LimitType
    //GoalType
}
