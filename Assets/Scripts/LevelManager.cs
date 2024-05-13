using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using DataStorage;
using System.Linq;

public enum LevelState
{
    Map,
    Prepare,
    Playing,
    Pausing,
    Complete,
    Lost,
}

public class ChangeLevelData
{
    public bool isEnable;
    public Func<bool> conditions;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelChangeEvent levelChangeEvent;
    [SerializeField] private BaseEvent OnLoadSceneComplete;
    public int score;
    public int step;
    public delegate void GameStateEvent();
    public static event GameStateEvent OnStartGame;
    public static event GameStateEvent OnCompleteLevel;
    public static event GameStateEvent OnLevelLoaded;

    public static event GameStateEvent OnLevelLose;
    public bool levelLoaded;
    public static int currentLevel;
    private GameManager gameManager;
    [SerializeField] private Transform gfPrefab;
    [SerializeField] private Transform counter;
    [SerializeField] private PopupManager popupManager;
    private int stars;
    public float timePass;
    public GameManager GameManager { get { return gameManager; } }
    public LevelData levelData;
    public GameObject levelsMap;
    private bool openGiftPanel;
    private LevelState levelState;
    public LevelState LevelState
    {
        get
        {
            return levelState;
        }
        set
        {
            levelState = value;
            switch (value)
            {
                case LevelState.Map:
                    LevelMapManager.Instance.EnableMap(true);
                    OnLoadSceneComplete.Raise();
                    break;
                case LevelState.Prepare:
                    StartCoroutine(PrepareLevel(currentLevel));
                    if(levelLoaded == true)
                    {
                        OnLevelLoaded?.Invoke();
                        LevelState = LevelState.Playing;
                    }
                    //}
                    break;
                case LevelState.Playing:
                    //PlaySoung();
                    break;
                case LevelState.Pausing:
                    //stop coroutine count time
                    break;

                case LevelState.Complete:
                    SetLevelStars();
                    CompleteLevel(currentLevel,stars);
                    OnCompleteLevel?.Invoke();
                    break;
                case LevelState.Lost:
                    PopupManager.Instance.TogglePanel(GameScenePopup.LosePanel, true);
                    OnLevelLose?.Invoke();
                    break;
            }
        }
    }

    public static LevelManager Instance { get; private set; }

    const string LEVEL_ID_KEY = "LEVEL_ID";
    const string PLAY_STATE_KEY = "PLAY_STATE";
    const string COMPLETED_LEVEL_COUNT_KEY = "COMPLETED_LEVEL_COUNT";

    public string LevelID
    {
        get => GameData.Get(LEVEL_ID_KEY, "");
        set => GameData.Set(LEVEL_ID_KEY, value);
    }

    public int GetStarCount(string id) => GameData.Get($"Level_{id}_StarCount", 0);
    public void SetStarCount(string id, int count) => GameData.Set($"Level_{id}_StarCount", count);

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    private void Start()
    {
        LevelState = LevelState.Map;
    }
    private void OnEnable()
    {
        LevelSelectorMap.OnLevelClicked += LevelSelectorMap_OnLevelClicked;
    }

    private void OnDisable()
    {
        LevelSelectorMap.OnLevelClicked -= LevelSelectorMap_OnLevelClicked;
    }

    private void LevelSelectorMap_OnLevelClicked(object sender, LevelSelectorMap.OnLevelClickedEventArgs e)
    {
        currentLevel = e.number;
        Debug.Log("change to prepare state");
        LevelID = e.number.ToString();
        LevelState = LevelState.Prepare;
    }

    IEnumerator PrepareLevel(int number)
    {
        Debug.Log("level clicked " + number);
        levelLoaded = false;
        score = 0;
        step = 0;
        yield return new WaitForSeconds(.3f);
        LevelMapManager.Instance.EnableMap(false);
        levelChangeEvent.Raise(new ChangeLevelData
        {
            isEnable = false,
            conditions = () => true
        });
        yield return new WaitForSeconds(2.3f);
        LoadLevel(number);

        levelLoaded = true;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    //public void SetLevelState(LevelState levelState)
    //{
    //    this.levelState = levelState;
    //}

    public void LoadLevel(int currentLevel)
    {
        levelData = LoadingManager.LoadLevel(currentLevel, levelData);
        var gameFieldGO = Instantiate(gfPrefab, Camera.main.transform.position,Quaternion.identity);
        gameFieldGO.SetParent(this.gameObject.transform);
        gameManager  = gameFieldGO.GetComponent<GameManager>();
        gameFieldGO.GetComponent<GameManager>().levelData = levelData;
        gameFieldGO.GetComponent<GameManager>().InitGameField();
        levelState = LevelState.Playing;
        ToggleGameUI(true);
        OnStartGame?.Invoke();
        //OnSceneLoadDone.Raise();
    }

    private void CompleteLevel(int number,int starCount)
    {
        PlayerPrefManager.SaveLevelStarsCount(number, starCount);
        //levelsMap.GetComponent<LevelMapManager>().UpdateMapLevelsStatus();// đưa vào levelstate.Map
        SetStarCount(number.ToString(), starCount);
        GameData.Save();
    }

    public void NextLevel()
    {
        popupManager.TogglePanel(GameScenePopup.popupWin, false);
        currentLevel++;
        LevelID = currentLevel.ToString();
        GameManager.Instance.ResetLevel();
        LevelState = LevelState.Prepare;
    }

    public void ToggleLevelMaps(bool enable)
    {
        levelsMap.GetComponent<LevelMapManager>().ToggleLevels(enable);
    }

    private void ToggleGameUI(bool enable)
    {
        popupManager.TogglePanel(GameScenePopup.GameUI, enable);
    }

    public void SetLevelStars()
    {
        GetTimePassed();
        stars = 0;

        for (int i = 3; i >= 1; i--)
        {
            if ((int)timePass <= i)
            {
                stars++;
            }
        }
        Debug.Log(stars);
    }

    public int GetLevelStarsAfterCompleted()
    {
        return stars;
    }

    public void BackToMap()
    {
        //if (gameManager != null) gameManager.ResetLevel(); // if there is a spawned Game
        LevelState = LevelState.Map;
    }

    private void GetTimePassed()
    {
        timePass = Counter.Instance.GetRemainingTime();
    }
    private void RestartLevel()
    {
        if (GameManager != null)
        {
            GameManager.Instance.ResetLevel();
        }
        levelState = LevelState.Prepare;
    }
}

