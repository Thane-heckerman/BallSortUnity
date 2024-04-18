using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BallType")]
public class BallTypeSO : ScriptableObject
{
    public BallColor color;
    
    public string nameString;
    public Sprite sprite;
}
