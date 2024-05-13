using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class Counter : MonoBehaviour
{
    public static Counter Instance
    {
        get ; private set;
    }
    private TextMeshProUGUI txt;
    private LevelData levelData;
    private  float lastTime ;
    private float startTime;
    private int minutes;
    public  System.Action CompleteCountDownCallBack;
    private float remainingTime;

    private void Awake()
    {
        Instance = this;
    }


    public float GetRemainingTime()
    {
        return remainingTime;
    }

    private void SetRemainingTime()
    {
        remainingTime = (startTime - lastTime) / 60F;
    }

    private void SetLasTime(float time)
    {
        lastTime = time;
    }// for testing
    private void OnDisable()
    {

    }

    private void OnEnable()
    {
         
    }

    private void ResetTime()
    {
        lastTime = 0;
        startTime = 0;
        //minutes = 0;
    }

    public void UpdateText(TextMeshProUGUI txt , float time)
    {
        minutes = Mathf.FloorToInt(time / 60F);
        var seconds = Mathf.FloorToInt(time - minutes * 60);
        txt.text = "" + $"{minutes:00}:{seconds:00}";
    }

    public IEnumerator CountDown(float time, Func<bool> condition, Action<float> UpdateTextDuringCountDownCoroutine = null)
    {
        startTime = time;
        while (time > 0)
        {
            time--;
            UpdateTextDuringCountDownCoroutine?.Invoke(time);
            if (condition())
            {
                SetLasTime(time);
                SetRemainingTime();
                //OnStopCountDown?.Invoke(this, EventArgs.Empty);
                CompleteCountDownCallBack();
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
        //CompleteCountDownCallBack();
    }


}
