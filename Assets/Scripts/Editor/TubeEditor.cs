using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TubeTemplate),true)]
public class TubeEditor : Editor
{
    public BallTypeListSO ballTypeList;
    Texture2D ball;
    private GameObject targetEditorObject;
    private GameObject prefab;

    

    
}
