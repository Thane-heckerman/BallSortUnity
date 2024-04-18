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
        levelString = transform.Find("level_text").GetComponent<TextMeshPro>().text;
        Number = int.Parse(levelString);
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
        transform.Find("Unlocked").gameObject.SetActive(!enable);
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
            Debug.Log("level clicked");
            transform.parent.gameObject.SetActive(false);
            OnLevelClicked?.Invoke(this, new OnLevelClickedEventArgs { number = Number });
        }
        return;
    }


}
