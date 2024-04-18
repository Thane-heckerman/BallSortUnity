using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopContentSpawner : BaseSpawner
{
    private int ballListCount;

    protected override void Awake()
    {
        base.Awake();
        IColorableComponent colorableComponent = prefabs[0].GetComponent<IColorableComponent>();
        ballListCount = colorableComponent.Sprites.Count;
        Debug.Log(ballListCount); //6
    }

    private void OnEnable()
    {
        SpawnContent();
    }

    protected override void LoadPrefab()
    {
        base.LoadPrefab();
        //prefabs.Add(Resources.Load<Transform>("BallPrefabPreview"));
    }
    public void SpawnContent()
    {
        for (int i = 0; i < ballListCount ; i++)
        {
            Transform pf = Spawn();
            UIShopBtn uiShopBtn = pf.GetComponent<UIShopBtn>();
            uiShopBtn.Index = i;
            uiShopBtn.SetSprite();
        }

    }

    public override Transform Spawn()
    {
        Transform pf = base.Spawn();
        pf.SetParent(this.transform);
        return pf;
    }
    //private void SetSprite()
    //{
    //    foreach
    //}

    public override Transform Spawn(Transform prefab)
    {
        return base.Spawn(prefab);
    }
}
