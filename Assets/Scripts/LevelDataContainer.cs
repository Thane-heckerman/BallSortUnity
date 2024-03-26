using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="LevelDataContainer")]
public class LevelDataContainer : ScriptableObject
{
    public LevelData levelData;

    public void SetData(LevelData levelData)
    {
        this.levelData = levelData;
    }
}
