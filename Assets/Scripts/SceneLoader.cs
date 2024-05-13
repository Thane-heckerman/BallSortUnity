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


public class SceneLoader : MonoBehaviour //  refactor code vif quas khawms
{
    public static SceneLoader Instance { get; private set; }
    private Canvas canvas;
    public Animator animator;
    public static event EventHandler OnLoadGameScene;
    public bool loading;


    private void Awake()
    {
        Instance = this;
    }
    public void Load(LoadSceneData data)
    {
        DontDestroyOnLoad(this.gameObject);

        Debug.Log("loading");

        StartCoroutine(LoadingRoutine(data.sceneName, data.isLoading, data.conditions));
    }

    private void OnEnable()
    {
        canvas = animator.GetComponent<Canvas>();
    }

    public void LoadScene()
    {
        StartCoroutine(OnStartGame());// gọi từ button start
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
        OnLoadGameScene?.Invoke(this, EventArgs.Empty);
        SetAnimClose();
        var time = animator.GetCurrentAnimatorStateInfo(0).length;
        var ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        ao.allowSceneActivation = false;
        //yield return new WaitUntil(() => time > 1.0f);
        yield return new WaitForSeconds(time);
        if (conditions != null)
        {
            yield return new WaitUntil(conditions);
        }
        canvas.transform.Find("GameBG").gameObject.SetActive(enable);
        ao.allowSceneActivation = true;
        Debug.Log("log in Loading Function ");
        SetAnimOpen();
        loading = false;
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
        SetAnimClose();
        var time = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
        SetAnimOpen();
        var time1 = animator.GetCurrentAnimatorClipInfo(0).Length;
        loading = false;
    }

    public void OnLoadSceneComplete()
    {
        SetAnimOpen();
        //animator.ResetTrigger("Stop");
        loading = false;
        canvas.transform.Find("GameBG").gameObject.SetActive(false);
    }

    private void SetAnim(string from, string to)
    {
        animator.ResetTrigger(from);
        animator.SetTrigger(to);
    }

    private void SetAnimClose()
    {
        animator.ResetTrigger("Stop");
        animator.SetTrigger("Start");
    }

    private void SetAnimOpen()
    {
        animator.ResetTrigger("Start");
        animator.SetTrigger("Stop");
    }
}
