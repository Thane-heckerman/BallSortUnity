using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GiftSpawner: BaseSpawner
{

    private Transform giftPrefab;

    private List<Vector2> giftPoses;

    [SerializeField] protected GiftCtrl giftCtrl;

    private void Start()
    {
        giftPrefab = Resources.Load<Transform>("GiftPrefab");
        Spawn();
    }

    public override Transform Spawn()
    {
        Transform GiftGO = Instantiate(giftPrefab,transform.position, Quaternion.identity);
        GiftGO.SetParent(this.transform);
        return GiftGO;
    }

}

