using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ShopContentManager : MonoBehaviour
{
    [SerializeField] private List<ShopItem> shopItems;
    private ShopContentSpawner shopContentSpawner;

    private void Awake()
    {
        shopContentSpawner = GetComponent<ShopContentSpawner>();
    }

    private void OnEnable()
    {
        shopItems = shopContentSpawner.SpawnContent();
        BuyBtn.OnBuyShopItem += BuyBtn_OnBuyShopItem;
    }

    private void BuyBtn_OnBuyShopItem(object sender, System.EventArgs e)
    {
        shopItems.Where(i => i.Index == ShopUIPreview.PreviewBallTypeListIndex).
                  FirstOrDefault().GetComponent<IColectable>().Collect();
    }
}
