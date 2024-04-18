using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefManager
{
    public static string GetLevelKey(int number)
    {
        return string.Format("Level.{0:000}.StarsCount", number);
    }

    public static int LoadLevelStarsCount(int level)
    {
        return PlayerPrefs.GetInt(GetLevelKey(level), 0);
    }

    public static void SaveLevelStarsCount(int level, int starsCount)
    {
        PlayerPrefs.SetInt(GetLevelKey(level), starsCount);
    }

    public static void ClearLevelProgress(int level)
    {
        PlayerPrefs.DeleteKey(GetLevelKey(level));
    }
    
}
