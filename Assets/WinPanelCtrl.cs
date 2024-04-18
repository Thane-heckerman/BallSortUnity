using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WinPanelCtrl : MonoBehaviour
{
    [SerializeField] private GameObject stars;
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        PopupManager.OnPopupAppered += PopupManager_OnPopupAppered;
    }

    private void OnDisable()
    {
        PopupManager.OnPopupAppered -= PopupManager_OnPopupAppered;
    }

    private void PopupManager_OnPopupAppered(object sender, System.EventArgs e)
    {
        StartCoroutine(StarsAppear(LevelManager.Instance.GetLevelStarsAfterCompleted()));
        levelText.text = "Level" + " " +  LevelManager.Instance.GetCurrentLevel();
    }

    public IEnumerator StarsAppear(int number)
    {
        for(int i = 0; i<= number; i++)
        {
            if (i == 0) continue;
            if(i == 1)
            {
                yield return new WaitForSeconds(.2f);
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
    }
}
