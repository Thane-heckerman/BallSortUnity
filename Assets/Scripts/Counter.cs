using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Counter : MonoBehaviour
{
    private TextMeshProUGUI txt;
    private LevelData levelData;
    public int lastTime = 0;
    public int startTime;
    private int minutes;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LevelManager.OnStartGame += OnStartGame;
        LevelManager.OnCompleteLevel += LevelManager_OnCompleteLevel;    
    }

    private void LevelManager_OnCompleteLevel()
    {
        StopCoroutine(CountDown());
        lastTime = minutes;
    }

    public int GetRemainingTime()
    {
        return lastTime;
    }

    public int GetTimePassedInMinutes()
    {
        return startTime - GetRemainingTime();
    }

    private void OnDisable()
    {
        LevelManager.OnStartGame -= OnStartGame;
        LevelManager.OnCompleteLevel -= LevelManager_OnCompleteLevel;
    }
    private void OnStartGame()
    {
        levelData = LevelManager.Instance.levelData;
        startTime = Mathf.FloorToInt(levelData.timeLimit/60F);
        Enable();
    }

    public void SetEnable(bool enable)
    {
        gameObject.SetActive(enable);
    }

    private void Enable()
    {
         StartCoroutine(CountDown());
    }


    private void UpdateText()
    {
        minutes = Mathf.FloorToInt(levelData.timeLimit / 60F);
        var seconds = Mathf.FloorToInt(levelData.timeLimit - minutes * 60);
        txt.text = "" + $"{minutes:00}:{seconds:00}";
        //txt.text = levelData.timeLimit.ToString();
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            UpdateText();
            levelData.timeLimit--;
            if(GameManager.Instance.IsCompleteLevel())
            {
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }

    }
}
