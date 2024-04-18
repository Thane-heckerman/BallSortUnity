using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this class is for loadLevel in GameManager
public static class LoadingManager
{
    public static LevelData LoadLevel(int level,LevelData levelData)
    {
        var levelDataContainer = Resources.Load("Levels/Level_" + level) as LevelDataContainer;

        if (levelDataContainer) {
            levelData = levelDataContainer.levelData;
            return levelData;
        }
        return null;                              
    }

}
