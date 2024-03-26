using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BallType")]
public class BallTypeSO : ScriptableObject
{
    public BallColor color
    {
        get { return color; }
        set
        {
            color = value;
        }
    }
    public string nameString;
    public Sprite sprite;
}
