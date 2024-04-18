using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBtn : MonoBehaviour
{
    public SceneLoader mainMenu;
    [SerializeField] private LoadingSceneEvent loadingSceneEvent;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        loadingSceneEvent.Raise(new LoadSceneData
        {
            sceneName = "GameScene",
            conditions = () => true,
            isLoading = true
        })) ;
    }
}
