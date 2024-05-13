using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPageBtn : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => CameraManager.Instance.MoveToNextPageOfLevelList());
    }

}
