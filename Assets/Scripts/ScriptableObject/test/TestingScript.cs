using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    [SerializeField] private OnPressPEvent OnPressPEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPressPEvent.Raise(new EventArgumentTest
            {
                value = 9
            });
        }
    }
}
