using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBtn : MonoBehaviour
{
    [SerializeField] private LoadingSceneEvent loadingSceneEvent;
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        Debug.Log("clicked"));
    }

    public void OnClick()
    {
        Debug.Log("onclicked");
        SceneLoader.Instance.Load(new LoadSceneData
        {
            sceneName = "GameScene",
            conditions = () => true,
            isLoading = true
        });
    }
}
