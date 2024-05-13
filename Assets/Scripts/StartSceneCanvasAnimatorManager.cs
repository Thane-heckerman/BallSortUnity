using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneCanvasAnimatorManager : MonoBehaviour
{
    private Animator buttonPlayAnimator;
    private Animator logoAnimator;
    private Animator buttonQuitAnimator;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {

        logoAnimator = transform.Find("Logo").GetComponent<Animator>();
        buttonPlayAnimator = transform.Find("Play").GetComponent<Animator>();
        buttonQuitAnimator = transform.Find("Quit").GetComponent<Animator>();
        SceneLoader.OnLoadGameScene += SceneLoader_OnLoadGameScene;
    }

    private void OnDestroy()
    {
        SceneLoader.OnLoadGameScene -= SceneLoader_OnLoadGameScene;
    }

    private void SceneLoader_OnLoadGameScene(object sender, System.EventArgs e)
    {
        buttonPlayAnimator.SetTrigger("Start");
        logoAnimator.SetTrigger("Start");
        buttonQuitAnimator.SetTrigger("Start");
    }
}
