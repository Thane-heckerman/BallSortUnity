using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]

public class LevelData
{
    public int intLevelNum;
    public int timeLimit;
    public Goal goalType;
    public List<GameFieldData> fields = new List<GameFieldData>();
    // for creating new level

    public LevelData DeepCopy(int level)
    {
        var other = (LevelData)MemberwiseClone();
        other.intLevelNum = intLevelNum;
        other.timeLimit = timeLimit;
        other.goalType = goalType;
        other.fields = new List<GameFieldData>();
        for (var i = 0; i < fields.Count; i++)
        {
            other.fields.Add(fields[i].DeepCopy());
        }
        return other;
    }

    public GameFieldData AddNewGameFieldData()
    {
        GameFieldData newField = new GameFieldData();
        fields.Add(newField);
        return newField;
    }

    public void RemoveGameFieldData(int i)
    {
        fields.RemoveAt(i);
    }
}

[System.Serializable]
public class GameFieldData
{
    public int maxTubeNumber;
    public int maxBallNumer;
    public List<TubesForEditor> tubes = new List<TubesForEditor>();

    public GameFieldData DeepCopy()
    {
        var other = (GameFieldData)MemberwiseClone();
        other.tubes = new List<TubesForEditor>();
        for( var i = 0; i < tubes.Count; i++)
        {
            other.tubes[i].DeepCopy();
        }
        return other;
    }
}

[System.Serializable]
public class TubesForEditor
{
    public int maxBallPosNumber;

    public List<BallPosLayer> listBallPost;

    public TubesForEditor DeepCopy()
    {
        var other = (TubesForEditor)MemberwiseClone();
        return other;
    }

    public ItemForEditor GetItem(int ballPost)
    {
        return listBallPost[ballPost].GetItem();
    }

    public void Initialize()
    {
        for (int i = 0; i < maxBallPosNumber; i++)
        {
            listBallPost.Add(new BallPosLayer(i));
        }
    }
}

[System.Serializable]
public class BallPosLayer
{
    public int index;
    public ItemForEditor itemForEditor;

    public BallPosLayer(int index)
    {
        this.index = index;
    }

    public ItemForEditor GetItem()
    {
        return itemForEditor;
    }

    public void SetItem(ItemForEditor item)
    {
        itemForEditor = item;
    }

    public void Clear()
    {
        itemForEditor = null;
    }
}

[System.Serializable]
public class ItemForEditor {
    //public BallTypeSO ballType;
    //public Texture2D texture2D;
    //public IColorableComponent colors;
    //public GameObject ball;
    public BallColor color;

    public ItemForEditor DeepCopy()
    {
        var other = (ItemForEditor)MemberwiseClone();
        return other;
    }

}