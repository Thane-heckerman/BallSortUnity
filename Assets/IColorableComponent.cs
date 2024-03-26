using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IColorableComponent : MonoBehaviour
{
    public BallData ballData;
    public BallTypeListSO ballTypeList;
    private SpriteRenderer sr;
    public List<LevelSprite> Sprites = new List<LevelSprite>();

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetColor() //set theo enum
    {
        BallTypeSO ball = GetBallData();
        BallColor color = ball.color;
        sr.sprite = ballTypeList.ballTypeList.Where(c => c.color == color)
                                                     .Select(s => s.sprite).FirstOrDefault();
    }

    public void SetTypeSprite(BallTypeSO ballType)
    {
        ballData.ballType = ballType;
        sr.sprite = ballType.sprite;
        sr.sortingOrder = 3;
    }

    public BallTypeSO GetBallData()
    {
        return ballData.ballType;
    }

    public BallTypeListSO GetSprites(int currentLevel)
    {
        if (currentLevel == 0) currentLevel = 1;
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
