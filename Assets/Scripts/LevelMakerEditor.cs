using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelMakerEditor : EditorWindow
{
    [MenuItem("BallSort/LevelMakerEditor")]
    public static void ShowWindow()
    {
        var window = (LevelMakerEditor)EditorWindow.GetWindow(typeof(LevelMakerEditor), false, "level maker editor");
    }
}
