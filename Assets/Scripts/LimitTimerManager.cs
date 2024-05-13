using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LimitTimerManager : MonoBehaviour
{
    private float startTime;
    private LevelData levelData;
    public float remainingTime;
    private void Awake()
    {
        LevelManager.OnStartGame += LevelManager_OnStartGame;
        Counter.Instance.CompleteCountDownCallBack += LevelManager.Instance.SetLevelStars;
    }

    private void OnDestroy()
    {
        LevelManager.OnStartGame -= LevelManager_OnStartGame;
        Counter.Instance.CompleteCountDownCallBack -= LevelManager.Instance.SetLevelStars;
    }

    private void LevelManager_OnStartGame()
    {
        StartCoroutine(Counter.Instance.CountDown(LevelManager.Instance.levelData.timeLimit, () => GameManager.Instance.IsCompleteLevel() == true || LevelManager.Instance.LevelState != LevelState.Playing,
       new Action<float>(UpdateText)));
        Debug.Log("tick counter");
    }

    private void UpdateText(float time)
    {
        var text = GetComponent<TextMeshProUGUI>();
        Counter.Instance.UpdateText(text, time);
    }

}
