using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(IColorableComponent), true)]
public class IcolorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        IColorableComponent data = (IColorableComponent)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Sprites"));
        
        serializedObject.ApplyModifiedProperties();

    }
    private void OnDrawGizmos()
    {
        
    }
    private void OnSceneGUI()
    {
        IColorableComponent colorable = (IColorableComponent)target;
        
    }

    
}
