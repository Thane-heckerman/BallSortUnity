using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    private BallColor color;
    public BallColor Color
    {
        get  { return color; }
        set { color = value; }
    }
}
