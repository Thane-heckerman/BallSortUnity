using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddBtn : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Button addButton = GetComponent<Button>();
        addButton.onClick.AddListener(() => GameManager.Instance.AddTube());
    }
}
