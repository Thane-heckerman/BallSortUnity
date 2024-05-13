using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HideBtn : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() => Hide());
    }
    private void Hide()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
