using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditorInternal;
using DataStorage;

public class IColorableComponent : MonoBehaviour, IColorable
{
    public Ball ballRef;
    public BallTypeListSO ballTypeList;
    private SpriteRenderer sr;
    public List<LevelSprite> Sprites = new List<LevelSprite>();

    const string ACTIVE_BALL_LIST = "ACTIVE_BALL_LIST";
    public int ActiveBallList
    {
        get => GameData.Get(ACTIVE_BALL_LIST, 0);
        set => GameData.Set(ACTIVE_BALL_LIST, value);
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ballRef = GetComponent<Ball>();
    }

    private void OnEnable()
    {
        GetActiveBallTypeList();
    }

    public void SetSprite(BallColor color) //set theo enum
    {
        ballRef.ballData.color = color;
        ballRef.ballData.SetBallType(ballTypeList.ballTypeList.Where(b => b.color == color ).FirstOrDefault());
        SetSprite(ballRef.ballData.ballType.sprite);
    }

    private void GetActiveBallTypeList()
    {
        ballTypeList = GetSprites(ActiveBallList);
    }

    public void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
        sr.sortingOrder = 3;
    }

    public BallTypeSO GetBallData()
    {
        return ballRef.ballData.ballType;
    }

    public BallTypeListSO GetSprites(int currentLevel)
    {
        if (Sprites.Any(i => i.level == currentLevel))
            return Sprites.First(i => i.level == currentLevel).ballTypeListPerLevel;
        return Sprites[0].ballTypeListPerLevel ;
    }

    public BallTypeSO GetSprite(int level, int color)
    {
        var list = GetSprites(level);
        if (color < list.ballTypeList.Count) return list.ballTypeList[color];
        else if (list.ballTypeList.Any()) return list.ballTypeList[0];
        return null;
    }

    public Sprite GetRandomSpriteInAList(int index)
    {
        BallTypeListSO ballTypeList = GetSprites(index);
        Sprite rand = ballTypeList.ballTypeList[Random.Range(0, ballTypeList.ballTypeList.Count)].sprite;
        return rand;
    }


    //public BallTypeListSO GetSpritesOrAdd(int level)
    //{
    //    if(Sprites.All(i => i.level != level))
    //    {
    //        var sprites = Sprites[0].ballTypeListPerLevel;
    //        var other = new BallTypeListSO(sprites.ballTypeList.Count);
    //        for (var i = 0; i < sprites.ballTypeList.Count; i++) other[i] = sprites.ballTypeList[i];
    //        Sprites.Add(new LevelSprite { level = level, ballTypeListPerLevel = other }); 
    //    }
    //    return GetSprites(level);
    //}
}
