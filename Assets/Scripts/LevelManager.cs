using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get => instance; }
    public TextAsset jsonData;


    [System.Serializable]
    public class Level
    {
        public int level;
        public List<BallList> tubes;
        
    }

    [System.Serializable]
    public class LevelList
    {
        public List<Level> levels;
    }

    [System.Serializable]
    public class BallList
    {
        public List<int> ballList;
    }

    public LevelList levelList = new LevelList();

    private void Awake()
    {
        LevelManager.instance = this;
        this.ParsingDataLevel();
    }

    private LevelList ParsingDataLevel()
    {
        if (jsonData == null)
        {
            Debug.Log("missing json data");
            
        }
        string jsonString = jsonData.text;

        // Parse JSON thành đối tượng của lớp MyData
        this.levelList = JsonUtility.FromJson<LevelList>(jsonString);
        return this.levelList;
        
        
    }

    
}

