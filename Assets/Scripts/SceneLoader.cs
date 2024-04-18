using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class LoadSceneData {
    public bool isLoading;
    public string sceneName;
    public Func<bool> conditions;
}


public class SceneLoader : MonoBehaviour
{
    public Animator animator;
    public static event EventHandler OnLoadGameScene;
    public bool loading;
    private Canvas canvas;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Load(LoadSceneData data)
    {
        StartCoroutine(LoadingRoutine(data.sceneName, data.isLoading, data.conditions));
    }

    private void OnEnable()
    {
        canvas = animator.GetComponent<Canvas>();
        canvas.sortingOrder = -1;
    }

    public void LoadScene() {

        StartCoroutine(OnStartGame());
    }

    public IEnumerator OnStartGame()
    {
        animator.SetTrigger("Start");
        OnLoadGameScene?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene("GameScene");

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadingRoutine(string sceneName, bool enable, Func<bool> conditions)
    {
        if (loading) yield break;
        loading = true;
        canvas.transform.Find("GameBG").gameObject.SetActive(enable);
        animator.SetTrigger("Start");
        var time = animator.GetCurrentAnimatorStateInfo(0).length;
        
        var ao = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
        ao.allowSceneActivation = false;
        //yield return new WaitUntil(() => time > 1.0f);
        yield return new WaitForSeconds(time);
        if (conditions != null)
        {
            yield return new WaitUntil(conditions);
        }
        ao.allowSceneActivation = true;
    }

    public void OnChangeLevel(ChangeLevelData data)
    {
        StartCoroutine(ChangeLevelRoutine(data.isEnable, data.conditions));
    }

    IEnumerator ChangeLevelRoutine(bool enable, Func<bool> conditions)
    {
        if (loading) yield break;
        loading = true;
        canvas.transform.Find("GameBG").gameObject.SetActive(enable);
        animator.SetTrigger("Start");
        var time = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Stop");
        var time1 = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
        loading = false;
    }

    public void OnLoadSceneComplete()
    {
        animator.SetTrigger("Stop");
        loading = false;
        canvas.transform.Find("GameBG").gameObject.SetActive(false);
    }

    // viêt hàm load scene truyền vào loadingscenedata
}
