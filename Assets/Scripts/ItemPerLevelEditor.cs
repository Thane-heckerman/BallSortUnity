#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemPerLevelEditor : EditorWindow
{

    private static GameObject prefab;
    private static int numLevel;

    public static void ShowWindow()
    {
        ItemPerLevelEditor window = (ItemPerLevelEditor)EditorWindow.GetWindow(typeof(ItemPerLevelEditor), true);
        prefab = Resources.Load<GameObject>("BallPrefab");
        //numLevel = level;
    }

    [System.Obsolete]
    private void OnGUI()
    {
        if (prefab)
        {
            GUILayout.BeginVertical();
            {
                var sprs = prefab.GetComponent<IColorableComponent>().GetSprites(numLevel);
                for (var index = 0; index<sprs.ballTypeList.Count; index++)
                {
                    var spr = sprs.ballTypeList[index];
                    sprs.ballTypeList[index] = (BallTypeSO)EditorGUILayout.ObjectField(spr, typeof(Sprite), GUILayout.Width(50), GUILayout.Height(50));
                    if (sprs.ballTypeList[index] != spr)
                    {
                        PrefabUtility.SavePrefabAsset(prefab);
                    }
                }
            }
            GUILayout.EndVertical();
        }
        else
        {
            Close();
        }
    }
}
#endif