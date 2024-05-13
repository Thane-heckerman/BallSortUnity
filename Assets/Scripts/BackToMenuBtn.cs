using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BackToMenuBtn : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => BackToMenu());
    }

    public void BackToMenu()
    {
        SceneLoader.Instance.Load(new LoadSceneData
        {
            sceneName = "StartScene",
            isLoading = true,
            conditions = () => true,
        });
    }

}
