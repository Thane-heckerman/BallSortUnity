using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DataStorage;
public class ShopUIPreview : MonoBehaviour
{
    // Singleton
    [SerializeField] private Transform tubePrefab;

    private int activeBallTypeList;
    private Transform tubePreviewTransform;
    private List<BallPos> ballPoses;
    private List<Transform> balls = new List<Transform>();
    private bool Initilized;
    const string ACTIVE_BALL_TYPE_LIST_INDEX = "ACTIVE_BALL_TYPE_INDEX";

    public int ActiveBallTypeListIndex
    {
        get => GameData.Get(ACTIVE_BALL_TYPE_LIST_INDEX, 0);
        set => GameData.Set(ACTIVE_BALL_TYPE_LIST_INDEX, value);
    }

    private void Awake()
    {
        balls = transform.Cast<Transform>().ToList();
    }

    private void OnEnable()
    {
        activeBallTypeList = ActiveBallTypeListIndex;
        SetRandSprite(activeBallTypeList);
    }

    public void Toggle(bool enable) {
        gameObject.SetActive(enable);
    }

    public void SetRandSprite(int index)
    {
        foreach(var ball in balls)
        {
            var randSprite = ball.GetComponent<IColorableComponent>().GetRandomSpriteInAList(index);
            ball.GetComponent<Image>().sprite = randSprite;
        }
    }
}
