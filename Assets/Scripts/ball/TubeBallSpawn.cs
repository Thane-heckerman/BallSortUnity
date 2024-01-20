using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Numerics;
// gán vào trong ballPost để tự spawn nếu không có ball tại vị trí đó hoặc gán vào tube
public class TubeBallSpawn : MonoBehaviour
{ 
    public List<GameObject> listBall; //gán cho list ball data
    public List<GameObject> ballPosList;
    [SerializeField] private GameObject selectedBall;
    [SerializeField] protected GameObject ballTemp;
    public GameObject ballUpPos;
    [SerializeField] protected GameObject newBallPos;
    public bool isCompleted = false;
    [SerializeField] private BallTypeListSO ballTypeList;
    [SerializeField] private BallTypeSO ballType;
    public List<GameObject> positions;
    private void Awake()
    {
        this.ballTypeList = Resources.Load<BallTypeListSO>(typeof(BallTypeListSO).Name);
        Debug.Log(ballTypeList);
    }

    public List<GameObject> GetBallPos()
    {
        foreach (Transform childTransform in gameObject.transform)
        {
            if (childTransform.CompareTag("BallPos"))
            {
                this.ballPosList.Add(childTransform.gameObject);
                
            }
            else
            {
                Debug.LogWarning("No 'BallPos' found in child transform.");
            }
        }
        return this.ballPosList;
    }

    public void Spawn(List<int> tube)//spawn balll phải viết cho ông thứ i
    {
        for (int i = 0; i < tube.Count ; i++)
        {
            GameObject ballClone = Instantiate(ballTypeList.ballTypeList[tube[i]].prefab.gameObject, ballPosList[i].transform.position, ballPosList[i].transform.rotation);
            ballClone.SetActive(true);
            this.listBall.Add(ballClone);
        }
    }

    private GameObject GetLastBallInTube()
    {
        this.selectedBall= this.listBall[this.listBall.Count-1];
        this.ballTemp = this.selectedBall;
        this.RemoveSelectedBall(this.selectedBall);
        return this.ballTemp;
    }
    private void RemoveSelectedBall(GameObject selectedBall)
    {
        this.listBall.Remove(selectedBall);
        this.selectedBall = null;
    }

    public GameObject MoveBallUp()
    {
        positions.Add(this.ballUpPos);
        this.GetLastBallInTube();
        this.ballTemp.GetComponent<BallMovement>().StartMoveBall(positions);
        positions.Clear();
        return this.ballTemp;
    }

    // Receive Ball
    public void ReceiveBall(GameObject ballTemp)
    {
        // ballList.count == 4 dont receive ball
        BallMovement ball = ballTemp.GetComponent<BallMovement>();
        this.listBall.Add(ballTemp);
        //Debug.Log("số ball trong targetTube là:" + this.listBall.Count);
        int newPos = this.GetIndexOfBallPos(ballTemp);
        positions.Add(this.ballUpPos);
        positions.Add(this.ballPosList[newPos]);
        ball.StartMoveBall(positions);

        //ballTemp.GetComponent<BallMovement>().StartMoveBall(this.ballPosList[newPos].transform.position);
        
        //ballTemp.transform.DOPunchPosition(new Vector3(0, 1, 0), 4);
        this.ballTemp = null;

        // positions.Clear();
    }

    // Gọi hàm này sau khi gọi receive ball
    private int GetIndexOfBallPos( GameObject ballTemp)
    {
        int newPos = this.listBall.IndexOf(ballTemp);
        return newPos;
    }

    // move transform of selectball aka ballTemp to newBallPos
    // call BallMovement function bcs every movement of ball is stored in this script
    // should divide this file into 2 file pop ball and receive ball

    public bool CheckCompletedTube()
    {
        
        List<string> listBallsName = new List<string>();

        listBallsName = this.GetBallsName();

        // condition is the ballList count and the checkNameOfBallInList count is equal
        int count = 0;

        for (int i = 1; i < this.listBall.Count; i++)
        {
            if (listBallsName[i] == listBallsName[0])
            {
                count += 1;
                if (count == 3)
                {
                    this.isCompleted = true;
                    Debug.Log(gameObject.name + "đã completed");
                    return true;
                }
            }
            
        }
        return false;

    }

    private List<string> GetBallsName()
    {
        List<string> listBallsName = new List<string>();

        for (int i = 0; i < this.listBall.Count; i++)
        {
            string ballName = this.listBall[i].name;
            Debug.Log(ballName);
            listBallsName.Add(ballName);
        }
        return listBallsName;
    }
}

