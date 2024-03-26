using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    private int currenLevel;

    public LevelDataContainer levelDataContainer;
    
    public LevelManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    // level data

    

    public int GetCurrentLevel()
    {
        return currenLevel;
    }
    
}

