using UnityEngine;
using UnityEngine.UI;

public class UIShopBtn : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => PopupManager.Instance.TogglePanel(GameScenePopup.ShopUI, true));
    }
}
