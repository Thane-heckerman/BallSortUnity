using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public Transform tubePre;
    List<Vector2> poses;
    public GameManager gameManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            poses = gameManager.SetTubePositionForRefactoring(7);
            foreach (var pos in poses)
            {
                Tube.Create(tubePre, pos, 0);
            }

        }

    }

}
