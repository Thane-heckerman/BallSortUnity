using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TubeTemplate),true)]
public class TubeEditor : Editor
{
    public BallTypeListSO ballTypeList;
    TubeTemplate targets;
    Texture2D ball;
    private GameObject targetEditorObject;
    private GameObject prefab;

    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        serializedObject.Update();
        ballTypeList = Resources.Load<BallTypeListSO>(typeof(BallTypeListSO).Name);
        targets = (TubeTemplate)target;
        
        GUILayout.BeginVertical();
        {
            if(targets.ballPosFEList.Count == 0)
            {
                targets.Init();
            }
            for (int i = 0; i< targets.ballPosFEList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                {

                    if (GUILayout.Button(GetValue(i), GUILayout.Height(25), GUILayout.Width(25)))
                    {
                        RandomBallType(i);
                    }
                    EditorGUILayout.LabelField("ballType");
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }

    GUIContent GetValue(int index) // return ballpos
    {
        var item = targets;

        var value = item.GetBallPos(index);

        if (value.isContainBall)
        {
            return new GUIContent("", "contain ball");
        }
        if (!value.isContainBall) 
        {
            return new GUIContent("x", "Not containBall");
        }
        return new GUIContent(ball, "ball");
    }

    public void RandomBallType(int i)
    {
        bool isContainBall = targets.GetBallPos(i).isContainBall;
        if (isContainBall)
        {
            targets.GetBallPos(i).ball.ballType = ballTypeList.ballTypeList[Random.Range(0, ballTypeList.ballTypeList.Count)];
        }
    }

}
