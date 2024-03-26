using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class ScriptableLevelManager
{
    
#if UNITY_EDITOR
    public static void CreateFileLevel(int level, LevelData _levelData)
    {
        var path = "Assets/Resources/Levels/";

        if (Resources.Load("Levels/Level_" + level))
        {
            SaveLevel(path, level, _levelData);
        }
        else
        {
            string fileName = "Level_" + level;

            var newLevelData = ScriptableObjectUtility.CreateAsset<LevelDataContainer>(path, fileName);
            newLevelData.SetData(_levelData.DeepCopy(level));
            EditorUtility.SetDirty(newLevelData);
            AssetDatabase.SaveAssets();
        }
    }
    public static void SaveLevel(string path, int level, LevelData _levelData)
    {
        var levelScriptable = Resources.Load("Levels/Level_" + level) as LevelDataContainer;
        if (levelScriptable != null)
        {
            levelScriptable.SetData(_levelData.DeepCopy(level));
            EditorUtility.SetDirty(levelScriptable);
        }

        AssetDatabase.SaveAssets();
    }
#endif

    public static LevelData LoadLevel(int level)
    {
        var levelScriptable = Resources.Load("Level " + level) as LevelDataContainer;
        LevelData levelData;
        if (levelScriptable)
        {
            levelData = levelScriptable.levelData.DeepCopy(level);
        }
        else
        {
            var levelScriptables = Resources.Load("Levels/LevelListScriptable") as LevelListScriptable; //<= this is a list of levels scriptableobject
            var ld = levelScriptables.LevelDataContainers[level - 1].levelData;
            if (ld != null)
            {
                levelData = ld.DeepCopy(level);
            }
            else return default;
        }

        return levelData;
    }


}
