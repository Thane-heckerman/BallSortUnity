using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIShopBtn : MonoBehaviour
{
    
    [SerializeField] private ShopItem item;

    private Image img;

    [SerializeField] private int index;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    void OnEnable()
    {
        //GetComponent<Button>().onClick.AddListener(() =>
        //{
            
        //});
    }

    public void SetSprite() {
        IColorableComponent icolorableComponent = GetComponent<IColorableComponent>();
        transform.Find("sprite").GetComponent<Image>().sprite = icolorableComponent.GetRandomSpriteInAList(Index);
    }

}
