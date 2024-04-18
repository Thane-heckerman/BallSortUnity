using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftLayoutSpawner : BaseSpawner
{
    [SerializeField] private GiftCtrl giftCtrl;

    [SerializeField] private Transform emptyPrefab;

    public List<Vector2> poses;

    private GridLayoutGroup grid;


    public List<Vector2> GetAllChild()
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i< transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        foreach (var pos in children)
        {
            poses.Add(pos.position);
        }
        return poses;
    }
    
}
