using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopManager : MonoBehaviour
{
    [SerializeField] private Transform ballPreviewTransform;
    [SerializeField] private Transform shopContentTransform;

    private void OnEnable()
    {
        ballPreviewTransform.GetComponent<ShopUIPreview>().Init();
    }

}
