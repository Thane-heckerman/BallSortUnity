using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCtrl : MonoBehaviour
{
    [SerializeField] private GiftSpawner giftSpawner;
    public GiftSpawner GiftSpawner { get => giftSpawner; }
    [SerializeField] private GiftLayoutSpawner giftLayoutSpawner;
    public GiftLayoutSpawner GiftLayoutSpawner { get => GiftLayoutSpawner; }

}
