using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
    [SerializeField] private GameObject popupWin;
    [SerializeField] private GameObject popupReward;
    [SerializeField] private GameObject levelSelectorPanel;
    [SerializeField] private GameObject ShopUI;

    public static event EventHandler OnPopupAppered;
    // Start is called before the first frame update


    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable()
    {
        LevelManager.OnCompleteLevel += LevelManager_OnCompleteLevel;
    }
   
    private void OnDisable()
    {
        LevelManager.OnCompleteLevel -= LevelManager_OnCompleteLevel;
    }

    private void LevelManager_OnCompleteLevel()
    {
        StartCoroutine(PopupAppearAnim(1f, popupWin));
    }

    public void ToggleGameUI(bool enable)
    {
        Transform GameBG = transform.Find("Game");
        GameBG.gameObject.SetActive(enable);
    }

    IEnumerator PopupAppearAnim(float time, GameObject popup)
    {
        yield return new WaitForSeconds(time);
        Animator animator = popup.GetComponent<Animator>();
        popup.SetActive(true);
        animator.Play("PopupAppear");
        OnPopupAppered?.Invoke(this, EventArgs.Empty);

    }

    public void ToggleUILevelSelector(bool enable) {
        levelSelectorPanel.SetActive(enable);
    }

    public void ToggleShopUI(bool enable)
    {
        ShopUI.SetActive(enable);
    }
}
