using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{

    public LevelSO testLevel;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            testLevel.MaxTubeNumber += 1;
        }
    }
}
