using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(999999)]
public class LevelSelectUI : MonoBehaviour
{
    
    //private void OnEnable()
    //{
    //    Debug.Log("OnEnable level selector UI called");
    //    ToggleLevels(true);
    //}

    public void ToggleLevels(bool enable)
    {
        if (LevelMapManager.Instance == null) return;
        LevelMapManager.Instance.ToggleLevels(enable);
    }

    private void OnDisable()
    {
        ToggleLevels(false);
    }
}
