using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditorInternal;
using DataStorage;
using UnityEngine.UI;

public class IColorableComponent : MonoBehaviour, IColorable
{
    public Ball ballRef;
    public BallTypeListSO ballTypeList;
    private SpriteRenderer sr;
    private Image image;
    public List<LevelSprite> Sprites = new List<LevelSprite>();

    const string ACTIVE_BALL_LIST = "ACTIVE_BALL_LIST";

    public int ActiveBallList
    {
        get => GameData.Get(ACTIVE_BALL_LIST, 0);
        set => GameData.Set(ACTIVE_BALL_LIST, value);
    }

    private void Awake()
    {
        ballRef = GetComponent<Ball>();
    }

    public void SetSprite(BallColor color, int index) //set theo enum
    {
        ballTypeList = GetSprites(index);
        Debug.Log(ballTypeList.name);
        Sprite sprite = ballTypeList.ballTypeList.Where(b => b.color == color).FirstOrDefault().sprite;
        Debug.Log(sprite.name);
        SetSprite(sprite);
    }

    private void SetBallRefType(BallTypeSO ballType) {

        ballRef.ballData.ballType = ballType;

    }

    private void SetBallColor(BallColor color) {
        ballRef.ballData.color = color;
    }

    public void GetActiveBallTypeList()
    {
        ballTypeList = GetSprites(ActiveBallList);
    }

    public void SetSprite(Sprite sprite)// interface 
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            sr = GetComponent<SpriteRenderer>();
            Debug.Log("has SpriteRenderer");
            sr.sprite = sprite;
            sr.sortingOrder = 3;
        }
        else
        {
            var image = GetComponent<Image>();
            image.sprite = sprite;
            Debug.Log("spirte name after changing" + image.sprite.name);
        }

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
