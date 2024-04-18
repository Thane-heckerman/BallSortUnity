using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStorage;

public class TestGameData : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            GameData.Save();
            Debug.Log("level"+LevelManager.Instance.LevelID);
        }
    }
}
