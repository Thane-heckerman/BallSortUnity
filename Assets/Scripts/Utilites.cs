using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilites 
{
    public static float objWidth;
    public static float objHeight;

    public static float CalculateXSpacing(int objNumber, Transform objPrefab,Transform panel)
    {
        float xSpacingInPixels = (panel.localScale.x - (objPrefab.transform.localScale.x * objNumber)) / objNumber;
        Vector3 xspacing = panel.TransformPoint(new Vector3(xSpacingInPixels, 0, 0));

        Vector3 screenWidthBottomLeft = (new Vector3(0, 0, 0));
        float xSpacing = Vector3.Distance(panel.TransformPoint(Vector3.left), xspacing);
        xSpacing = Mathf.Clamp(xSpacing, objPrefab.transform.localScale.x, 2f); // tuỳ theo setup của object
        return xSpacing;
    }

    public static float CalculateYSpacing(int row,Transform objPrefab, Transform panel)
    {
        int partOfTheObjectNeedToDivide = row + 1;

        float rowHeight = panel.localScale.y / partOfTheObjectNeedToDivide ;
        Vector2 position = panel.TransformPoint(new Vector2(0, rowHeight)); // => vector3 
        float ySpacing = Vector2.Distance(position, panel.transform.TransformPoint(Vector3.right * 2));
        return ySpacing;
    }
}

