using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LevelSpawner : BaseSpawner<LevelSelectorMap>
{
    public Vector2 startPosition;
    private float spacingX = 2f;
    private float spacingY = 1f;
    private int boardHeight = 4;
    private int boardWidth = 4;
    private Grid<LevelSelectorMap> levels;
    private int startLevel = 1;
    [SerializeField] private int levelNumber;


    private void OnEnable()
    {
        
    }


    public override List<LevelSelectorMap> SpawnABunch()
    {
        Vector3 origin = new Vector3(-boardWidth * 0.5f , boardHeight * 0.5f , 0);
         for (int y = 0; y < boardHeight; y++)
            {
            for (int x = 0; x < boardWidth;x++)
            {
                if (spawnedCount < levelNumber)
                {
                    Transform levelTransform = Spawn(prefabs[0], new Vector3(origin.x + x*spacingX, origin.y - y*spacingY, 0), Quaternion.identity);
                    LevelSelectorMap level = levelTransform.GetComponent<LevelSelectorMap>();
                    levelTransform.localScale = new Vector3(.7f, .7f, .7f);
                    levelTransform.SetParent(this.transform);
                    list.Add(level);
                    
                    spawnedCount++;
                    level.SetLevelText(spawnedCount);
                    level.Number = spawnedCount;
                }
                else break;
            }
        }
        return list;
    }

    public void SetLevelToSpawn(int number)
    {
        this.levelNumber = number;
    }

    private int GetLastLevel()
    {
        int maxCurrentLoadedLevel = list.Max(l => l.GetIntLevelText());
        return maxCurrentLoadedLevel;
    }
}
