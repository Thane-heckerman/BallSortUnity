using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopContentSpawner : BaseSpawner
{
    private int ballListCount;
    [SerializeField] private Transform target;
    protected override void Awake()
    {
        base.Awake();
        IColorableComponent colorableComponent = prefabs[0].GetComponent<IColorableComponent>();
        ballListCount = colorableComponent.Sprites.Count;
    }

    private void OnEnable()
    {
    }

    protected override void LoadPrefab()
    {
        base.LoadPrefab();
        //prefabs.Add(Resources.Load<Transform>("BallPrefabPreview"));
    }
    public List<ShopItem> SpawnContent()
    {
        Debug.Log("spawn");
        List<ShopItem> shopItems = new List<ShopItem>();
        for (int i = 0; i < ballListCount ; i++)
        {
            Transform pf = Spawn();
            pf.SetParent(target);
            ShopItem shopItem = pf.GetComponent<ShopItem>();
            shopItem.Index = i;
            shopItem.Init();
            shopItem.UpdateStatus();
            shopItems.Add(shopItem);
            //coupling
        }
        return shopItems;
    }

    public override Transform Spawn()
    {
        Transform pf = base.Spawn();
        pf.SetParent(this.transform);
        return pf;
    }

    public override Transform Spawn(Transform prefab)
    {
        return base.Spawn(prefab);
    }
}
