using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RestartBtn : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() => LevelManager.Instance.LoadLevel(LevelManager.Instance.GetCurrentLevel()));
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
