using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public enum GameScenePopup
{
    None,
    popupWin,
    popupReward,
    levelSelectorPanel,
    ShopUI,
    GameUI,
    LosePanel,
}
public class PopupManager : MonoBehaviour
{

    public static PopupManager Instance { get; private set; }
    [SerializeField] private GameObject popupWin;
    [SerializeField] private GameObject popupReward;
    [SerializeField] private GameObject levelSelectorPanel;
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject GameUI;

    [SerializeField] private GameObject losePanel;
    private int totalStars;
    public List<GameObject> popupList;
    public GameScenePopup popup;
    public static event EventHandler OnPopupAppered;
    public GameObject currentActivePopup;
    // Start is called before the first frame update


    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this.gameObject);

        popupList = new List<GameObject>() { popupReward, popupWin, levelSelectorPanel, ShopUI, GameUI };
        LevelManager.OnCompleteLevel += LevelManager_OnCompleteLevel;
        LevelManager.OnStartGame += LevelManager_OnStartGame;
        SceneLoader.OnLoadGameScene += SceneLoader_OnLoadGameScene;
    }

    void OnEnable()
    {
        foreach (var popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
    }

    private void SceneLoader_OnLoadGameScene(object sender, EventArgs e)
    {
        TogglePopup(currentActivePopup, false);
    }

    private void LevelManager_OnStartGame()
    {

    }

    private void OnDestroy()
    {
        LevelManager.OnCompleteLevel -= LevelManager_OnCompleteLevel;
        LevelManager.OnStartGame -= LevelManager_OnStartGame;
        SceneLoader.OnLoadGameScene -= SceneLoader_OnLoadGameScene;
    }

    private void LevelManager_OnCompleteLevel()
    {
        TogglePanel(GameScenePopup.popupWin, true);
        OnPopupAppered?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleUILevelSelector(bool enable)
    {
        currentActivePopup.SetActive(!enable);
        levelSelectorPanel.SetActive(enable);
    }

    private void TogglePopup(GameObject popup, bool enable)
    {
        switch (enable)
        {
            case true:
                if (currentActivePopup != null) TogglePopup(currentActivePopup, false);
                popup.SetActive(true);
                currentActivePopup = popup;
                break;
            case false:
                popup.SetActive(false);
                break;
        }

    }

    public void TogglePanel(GameScenePopup popup, bool enable)
    {
        switch (popup)
        {
            case GameScenePopup.popupWin:
                TogglePopup(popupWin, enable);
                break;
            case GameScenePopup.popupReward:
                Debug.Log("popup reward = " + enable);
                TogglePopup(popupReward, enable);
                break;
            case GameScenePopup.levelSelectorPanel:
                TogglePopup(levelSelectorPanel, enable);
                LevelMapManager.Instance.ToggleLevels(enable);
                Debug.Log("level toggle " + enable);
                break;
            case GameScenePopup.ShopUI:
                TogglePopup(ShopUI, enable);
                break;
            case GameScenePopup.GameUI:
                TogglePopup(GameUI, enable);
                break;

            case GameScenePopup.LosePanel:
                TogglePopup(losePanel, enable);
                break;
        }
    }



    //private void ResetCurrentActivePopup()
    //{
    //    currentActivePopup.SetActive(false);
    //    currentActivePopup = null;
    //}

    //private void SetCurrentActivePopup(GameObject popup)
    //{
    //    currentActivePopup = popup;
    //}
}
