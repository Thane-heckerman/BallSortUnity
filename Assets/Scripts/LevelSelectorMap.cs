using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class LevelSelectorMap : MonoBehaviour
{
    private string levelString;

    public int Number;
    public bool IsLocked;
    //public Object LevelScene;
    public int StarsCount;
    public Transform StarsHoster;
    public Transform Star1;
    public Transform Star2;
    public Transform Star3;

    public static event EventHandler<OnLevelClickedEventArgs> OnLevelClicked;
    public class OnLevelClickedEventArgs : EventArgs
    {
        public int number;
    }

    private void Awake()
    {

    }

    public void UpdateLevelState(int starsCount, bool isLocked)
    {
        StarsCount = starsCount;
        UpdateStars(isLocked ? 0 : starsCount);
        IsLocked = isLocked;
        //Debug.Log("islock " + isLocked);
        SetLockStatus(isLocked);
    }

    private void SetLockStatus(bool enable)
    {
        transform.Find("Lock").gameObject.SetActive(enable);
        transform.Find("BG").gameObject.SetActive(!enable);
    }

    public void UpdateStars(int starsCount)
    {
        Star1?.gameObject.SetActive(starsCount >= 1);
        Star2?.gameObject.SetActive(starsCount >= 2);
        Star3?.gameObject.SetActive(starsCount >= 3);

    }

    private void OnMouseDown()
    {
        if (!IsLocked)
        {
            //transform.parent.gameObject.SetActive(false);
            levelString = transform.Find("level_text").GetComponent<TextMeshPro>().text;
            Number = int.Parse(levelString);
            OnLevelClicked?.Invoke(this, new OnLevelClickedEventArgs { number = Number });
        }
        return;
    }

    public void SetLevelText(int index)
    {
        transform.Find("level_text").GetComponent<TextMeshPro>().text = index.ToString();
    }

    public int GetIntLevelText()
    {
        levelString = transform.Find("level_text").GetComponent<TextMeshPro>().text;
        Number = int.Parse(levelString);
        return Number;
    }
}
