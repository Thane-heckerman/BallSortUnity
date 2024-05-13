using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataStorage;
public class ShopItem : Item, IColectable
{
    [SerializeField] private ShopItemSO shopItemSO;
    public int costAmount;
    private Image img;
    [SerializeField] private GameObject ownedGameObject;

    public static event EventHandler<OnPreviewValueChangedEventArgs> OnPreviewValueChanged;
    public class OnPreviewValueChangedEventArgs
    {
        public int value;
        public int cost;
        public bool ownedStatus;
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (isOwned)
            {
                SetActiveBallType(Index);
            }
            ShopUIPreview.Instance.UpdateSprite(Index);
            OnPreviewValueChanged?.Invoke(this, new OnPreviewValueChangedEventArgs
            {
                value = Index,
                cost = costAmount,
                ownedStatus = isOwned
            });
        });
    }
    public void SetSprite()
    {
        IColorableComponent icolorableComponent = GetComponent<IColorableComponent>();
        transform.Find("sprite").GetComponent<Image>().sprite = icolorableComponent.GetRandomSpriteInAList(Index);
    }

    public void Init()
    {
        SetItemKey();
        Debug.Log("item" + Index + ItemKey);
        GetItemInfo();
        
        SetSprite();
    }

    private void GetItemInfo()
    {
        costAmount = shopItemSO.costAmount;
    }

    private void SetItemKey()
    {
        ItemKey = string.Format($"{item.itemType}_{Index}");
    }

    public void Collect()
    {
        isOwned = true;
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        ownedGameObject.SetActive(isOwned);
    }


    private void SetActiveBallType(int index)
    {
        GameData.Set("ACTIVE_BALL_LIST", index);
    }

}
