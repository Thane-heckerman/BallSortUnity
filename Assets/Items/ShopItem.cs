using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataStorage;

public class ShopItem : Item, IColectable
{
    protected int costAmount = 175;
    private Image img;
    [SerializeField] private GameObject ownedGameObject;
    public static event EventHandler<OnPreviewValueChangedEventArgs> OnPreviewValueChanged;
    public class OnPreviewValueChangedEventArgs
    {
        public int value;
        public int cost;
    }

    [SerializeField] private int index;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            ShopUIPreview.Instance.UpdateSprite(Index);
            OnPreviewValueChanged?.Invoke(this, new OnPreviewValueChangedEventArgs
            {
                value = Index,
                cost = costAmount
            }) ;
        });
    }

    public void SetSprite()
    {
        IColorableComponent icolorableComponent = GetComponent<IColorableComponent>();
        transform.Find("sprite").GetComponent<Image>().sprite = icolorableComponent.GetRandomSpriteInAList(Index);
    }

    protected override void Awake()
    {
        item.SetItemKey(Index.ToString());
    }

    public void Collect()
    {
        if (UIShopManager.Instance.coinsOwned >= costAmount)
        {
            UIShopManager.Instance.Spend(costAmount);
            isOwned = true;
            UpdateStatus();
        }
        else Debug.Log("not enough coin");
    }

    public void UpdateStatus()
    {
        if(Index == GameData.Get("ACTIVE_BALL_LIST",0)) {
            ownedGameObject.SetActive(true);
            Debug.Log("index = " + Index + "active ball type = " + GameData.Get("ACTIVE_BALL_LIST", 0));
        }
        else
        {
            ownedGameObject.SetActive(isOwned);
        }
    }
}
