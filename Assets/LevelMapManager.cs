using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class LevelMapManager : MonoBehaviour
{
    private List<LevelSelectorMap> mapLevels = new List<LevelSelectorMap>();

    private List<LevelSelectorMap> GetMapLevels()
    {
        if(mapLevels.Count == 0) {
            mapLevels = FindObjectsOfType<LevelSelectorMap>().OrderBy(ml => ml.Number).ToList();
        }
        return mapLevels;
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        mapLevels = GetMapLevels();
        UpdateMapLevelsStatus();
        //LevelManager.OnLevelLoaded += OnLevelLoaded;
        LevelManager.OnCompleteLevel += OnCompleteLevel;
    }

    private void OnLevelLoaded()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        LevelManager.OnCompleteLevel -= OnCompleteLevel;
        //LevelManager.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnCompleteLevel()
    {
        UpdateMapLevelsStatus();
    }

    public void UpdateMapLevelsStatus()
    {
        foreach (var maplevel in mapLevels)
        {
            maplevel.UpdateLevelState(MapProgressManager.GetLevelStarCount((maplevel.Number)), IsLevelLocked(maplevel.Number));
        }
    }

    private bool IsLevelLocked(int number)
    {
        return number > 1 && MapProgressManager.GetLevelStarCount(number - 1) == 0;
    }

    public LevelSelectorMap GetLevelMap(int number)
    {
        return GetMapLevels().SingleOrDefault(ml => ml.Number == number);
    }

    public void CompleteLevel(int number, int starsCount)
    {
        int currentStarCount = MapProgressManager.GetLevelStarCount(number);
        int maxStarCount = Mathf.Max(currentStarCount, starsCount);
        MapProgressManager.SaveLevelStar(number, maxStarCount);
    }

}

public static class MapProgressManager {
    public static string GetLevelKey(int number)
    {
        return string.Format("Level.{0:000}.StarsCount",number);
    }

    public static int GetLevelStarCount(int number)
    {
        return PlayerPrefs.GetInt(GetLevelKey(number),0);
    }

    public static void SaveLevelStar(int number, int stars)
    {
        PlayerPrefs.SetInt(GetLevelKey(number), stars);
    }

    public static void ClearLevelStarCount(int level)
    {
        PlayerPrefs.DeleteKey(GetLevelKey(level));
    }
}
