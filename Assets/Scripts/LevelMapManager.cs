using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening.Core.Easing;
using DataStorage;
using Unity.VisualScripting;

public class LevelMapManager : MonoBehaviour
{
    public static LevelMapManager Instance { get; private set; }
    [SerializeField] private LevelListScriptable levelList;
    [SerializeField] private List<LevelSelectorMap> mapLevels;
    public int LevelCount { get => levelList.LevelDataContainers.Count; }

    private int totalStar = 0;
    public List<LevelSelectorMap> GetMapLevels()
    {
        if (mapLevels.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                mapLevels.Add(transform.GetChild(i).GetComponent<LevelSelectorMap>());
            }
        }
        return mapLevels;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        LevelManager.OnCompleteLevel += OnCompleteLevel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ResetLevelsInfor();
        }
    }

    private void OnLevelLoaded()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
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
        foreach (var maplevel in GetMapLevels())
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

    public void ResetLevelsInfor()
    {
        foreach (var level in mapLevels)
        {
            Debug.Log("level number" + level.Number);
            MapProgressManager.ClearLevelStarCount(level.Number);
        }
    }

    public void GetLevelsStarCountInfo()
    {
        foreach (var level in GetMapLevels())
        {
            MapProgressManager.SaveLevelStar(level.Number, 0);
            Debug.Log("level stars" + level.Number + MapProgressManager.GetLevelStarCount(level.Number));
            UpdateMapLevelsStatus();
        }
    }


    public void ToggleLevels(bool enable)
    {
        Debug.Log(mapLevels.Count);
        foreach (var level in mapLevels)
        {
            level.gameObject.SetActive(enable);
        }
    }
    public void EnableMap(bool enable)
    {
        StartCoroutine(EnableMapState(enable));
    }

    IEnumerator EnableMapState(bool enable)
    {
        if (LevelManager.Instance.GameManager != null)
        {
            GameManager.Instance.ResetLevel();
        }

        switch (enable)
        {
            case true:
                if (CanOpenGiftPanel())
                {
                    PopupManager.Instance.TogglePanel(GameScenePopup.popupReward, true);
                    yield return new WaitForSeconds(.3f);
                    Debug.Log("can open reward panel is true");
                }
                yield return new WaitUntil(() => !PopupManager.Instance.transform.Find("GiftPanel").GetComponent<GiftManager>().isBusy);
                //levelsMap.GetComponent<LevelMapManager>().ToggleLevels(true);
                PopupManager.Instance.TogglePanel(GameScenePopup.levelSelectorPanel, true);

                if (GetMapLevels().Count == 0)
                {
                    GetComponent<LevelSpawner>().SetLevelToSpawn(levelList.LevelDataContainers.Count);
                    GetComponent<LevelSpawner>().SpawnABunch();
                }
                else
                {
                    ToggleLevels(true);
                }
                //{
                //    foreach (var level in levelsMap.GetComponent<LevelMapManager>().mapLevels)
                //    {
                //        level.gameObject.SetActive(enable);
                //    }
                //}
                UpdateMapLevelsStatus();

                break;

            case false:
                //levelsMap.GetComponent<LevelMapManager>().ToggleLevels(false);
                PopupManager.Instance.TogglePanel(GameScenePopup.levelSelectorPanel, false);
                ToggleLevels(false);
                break;
        }

    }
    private bool CanOpenGiftPanel()
    {
        // first played game and open game
        if (totalStar == 0)
        {
            totalStar = GetTotalStarsCount();
            Debug.Log("total star when start game" + totalStar);
            return false;
        }

        // total stars != 0 and just completed a level 
        else
        {
            Debug.Log("total stars when game opened and completed a level" + totalStar);
            if (totalStar != GetTotalStarsCount() && GetTotalStarsCount() % 6 == 0)
            {
                totalStar = GetTotalStarsCount();
                Debug.Log("open reward panel");
                return true;
            }

        }

        return false;
    }

    private int GetTotalStarsCount()
    {
        int total = 0;
        foreach (var level in levelList.LevelDataContainers)
        {
            if (MapProgressManager.GetLevelStarCount(level.levelData.intLevelNum) == 0)
            {
                break;
            }
            var stars = MapProgressManager.GetLevelStarCount(level.levelData.intLevelNum);
            total += stars;
        }
        return total;
    }

}

public static class MapProgressManager
{
    public static string GetLevelKey(int number)
    {
        return string.Format("Level.{0:000}.StarsCount", number);
    }

    public static int GetLevelStarCount(int number)
    {
        return PlayerPrefs.GetInt(GetLevelKey(number), 0);
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
