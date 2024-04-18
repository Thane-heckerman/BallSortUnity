using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelMakerEditor : EditorWindow
{
    public int currentLevel = 1;
    public LevelData levelData;
    [MenuItem("BallSort/LevelMakerEditor")]
    public static void ShowWindow()
    {
        var window = (LevelMakerEditor)EditorWindow.GetWindow(typeof(LevelMakerEditor), false, "level maker editor");
    }

    private void Initialize()
    {
        var ball = Resources.Load<GameObject>("BallPrefab").GetComponent<Ball>();
        var balls = ball.GetComponent<IColorableComponent>().GetSprites(currentLevel).ballTypeList.Select(i => new ItemForEditor
        {
            ball = ball.gameObject,
            ballType = ball.ballData.ballType,
            color = i.color,

        }).ToList();
    }

    private void OnFocus()
    {
        if (LoadLevel(currentLevel))
        {
            Debug.Log(LoadLevel(currentLevel));
        }
        else Debug.Log("null");
    }

    private bool LoadLevel(int currentLevel)
    {
        levelData = ScriptableLevelManager.LoadLevel(currentLevel);
  
        if (levelData != null)
        {
            Initialize();
            return true;
        }

        return false;
    }
}
