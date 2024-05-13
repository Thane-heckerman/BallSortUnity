using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private LoadingSceneEvent loadingSceneEvent;
    [SerializeField] private GameObject sceneLoaderGO;

    private void Init()
    {
        Instantiate(sceneLoaderGO);
        loadingSceneEvent.Raise(new LoadSceneData
        {
            sceneName = "StartScene",
            isLoading = true,
            conditions = () => true,
        }) ;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
   
}
