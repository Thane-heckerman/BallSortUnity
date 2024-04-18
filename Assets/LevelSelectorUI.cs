using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSelectorUI : MonoBehaviour
{
    private string levelString;

    private void OnEnable()
    {
        levelString = transform.Find("level_text").GetComponent<TextMeshPro>().text;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("clicked");
            transform.parent.gameObject.SetActive(false);
        });
    }

}
