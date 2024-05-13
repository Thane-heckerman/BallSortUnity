using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanelCtrl : MonoBehaviour
{
    [SerializeField] private GameObject stars;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        PopupManager.OnPopupAppered += PopupManager_OnPopupAppered;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        ToggleAllStar(true);
    }
    private void OnDestroy()
    {
        PopupManager.OnPopupAppered -= PopupManager_OnPopupAppered;
    }

    private void PopupManager_OnPopupAppered(object sender, System.EventArgs e)
    {
        StartCoroutine(StarsAppear(LevelManager.Instance.GetLevelStarsAfterCompleted()));
        levelText.text = "Level" + " " + LevelManager.Instance.GetCurrentLevel();
    }

    public IEnumerator StarsAppear(int number)
    {
        for (int i = 0; i <= number; i++)
        {
            if (i == 0) continue;
            if (i == 1)
            {
                stars.transform.Find("One").Find("inactive").gameObject.SetActive(false);
            }
            if (i == 2)
            {
                stars.transform.Find("Two").Find("inactive").gameObject.SetActive(false);
            }
            if (i == 3)
            {
                stars.transform.Find("Three").Find("inactive").gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.45f);
        }
        yield return null;
    }

    private void ToggleAllStar(bool enable)
    {
        for (int i = 0; i < stars.transform.childCount; i++)
        {
            stars.transform.GetChild(i).Find("inactive").gameObject.SetActive(enable);
        }
    }
}
