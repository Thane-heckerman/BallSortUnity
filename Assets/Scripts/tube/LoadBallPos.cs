using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBallPos : MonoBehaviour
{
    public int tubeNumber = 2;
    
    public List<Transform> ballSpawnPos;
    public GameObject[] tubePositions;
    public GameObject tubePrefab;

    private void Awake()
    {
        this.SpawnTube();
    }
   
    

    public void InitiateBallSpawnPos()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("BallPos"))
            {
                ballSpawnPos.Add(transform.GetChild(i));
            }
        }

        Debug.Log("số lượng ballpos trong tube là: " + ballSpawnPos.Count);//checked
    }

    void SpawnTube()
    {
        //TubeList Tubes = new TubeList();
        foreach (GameObject tubePos in tubePositions)
        {
            GameObject cloneTube = Instantiate(this.tubePrefab, tubePos.transform.position, tubePos.transform.rotation);
        }

        this.InitiateBallSpawnPos();
    }


}
