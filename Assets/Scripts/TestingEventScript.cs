using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingEventScript : MonoBehaviour
{
    public void TestingEventFunction(EventArgumentTest data)
    {
        Debug.Log("Data argument test" + data.value);
    }
}
