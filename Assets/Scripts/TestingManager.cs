using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingManager : MonoBehaviour
{
    public GameObject gameObjectPrefab;
    private Camera mainCamera;

    // Start is called before the first frame update
    private void Start()
    {   
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(gameObjectPrefab, mainCamera.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
        }
    }

    public void TestingAwakeEvent()
    {
        Debug.Log("On Awake Event Raised");
    }
}
