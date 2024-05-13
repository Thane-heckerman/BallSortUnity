using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public Camera camera;
    private float maxPageXPos = 200;
    private float minX = 0f;

    private int index = 1;
    private int maxIndex = 3;
    private void Awake()
    {
        Instance = this;
    }

    public void MoveToNextPageOfLevelList()
    {
        Debug.Log("clicked");
        if (!CanMove(index +1)) return;
        else
        {
            Debug.Log("moved forward");
            index += 1;
            float xPosition = transform.position.x + GetScreenSizeInVector().x;
            Mathf.Clamp(xPosition, minX, maxPageXPos);
            camera.transform.DOMove(new Vector3(xPosition, 0, -10), 1.5f);
        }
    }

    public void MoveToBackPageOfLevelList()
    {

        Debug.Log("clicked");
        if (!CanMove(index-1)) return;
        else
        {
            Debug.Log("moved backward");
            index -= 1;
            float xPosition = transform.position.x - GetScreenSizeInVector().x;
            Mathf.Clamp(xPosition, minX, maxPageXPos);
            camera.transform.DOMove(new Vector3(xPosition, 0, -10), 1.5f);
        }
    }

    private Vector2 GetScreenSizeInVector()
    {
        Vector2 screenSizeInVector = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)) -
            Camera.main.ScreenToWorldPoint(new Vector2(0, 0)) ;
        return screenSizeInVector;
    }

    private bool CanMove(int target)
    {
        return (target >= 0 && target <maxIndex);
    }
}
