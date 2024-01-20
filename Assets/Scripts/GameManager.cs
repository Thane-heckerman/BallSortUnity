using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Camera mainCamera;
    public List<GameObject> tubeSpawners;
    public GameCtrl gameCtrl;
    public GameObject selectedTube = null;
    [SerializeField] protected GameObject targetTube = null;
    public List<GameObject> listTubes;
    public int currentLevel;
    [SerializeField] protected GameObject ballTemp;
    [SerializeField] private int numberTubesToCompleteLvl;
    private int completedTubes = 0;
    

    public LevelManager levelManager;
    // bổ sung hàm LoadLevel(int levelIndex)
    void Start()
    {
        mainCamera = Camera.main;
        this.gameCtrl = GetComponent<GameCtrl>();

        this.currentLevel = LevelManager.Instance.levelList.levels[currentLevel].level;
        int totalBall = 0;
        //spawn tube
        for (int i = 0; i < LevelManager.Instance.levelList.levels[this.currentLevel - 1].tubes.Count; i++)
        {
            this.listTubes = this.gameCtrl.tubeManager.SpawnTube(this.tubeSpawners[i], this.currentLevel - 1);

        }
        //spawn ball in tube
        for (int i = 0; i < this.listTubes.Count; i++)
        {
            List<int> ballList = LevelManager.Instance.levelList.levels[this.currentLevel - 1].tubes[i].ballList;
            if (ballList.Count != 0)
            {
                totalBall += ballList.Count;
                this.listTubes[i].GetComponent<TubeBallSpawn>().Spawn(ballList);

            }
        }
        
        this.numberTubesToCompleteLvl = totalBall / 4;
        Debug.Log("số ball đã spawn là: " + totalBall);

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            // move ball up
            if (hit.collider != null)
            {

                if (hit.collider.tag == "tube" && hit.collider.GetComponent<TubeBallSpawn>().isCompleted == false)
                {
                    HandleClick(hit);
                    CheckCompleteLevel();
                }
                else return;
            }
            else return;
        }
    }
    private void ResetSelection()
    {
        this.targetTube = null;
        this.selectedTube = null;
    }

    private void HandleClick(RaycastHit2D hit)
    {
        //if (targetTube)
        if (selectedTube == null && targetTube == null && hit.collider.GetComponent<TubeBallSpawn>().listBall.Count != 0)
        {
            this.RemoveBall(hit);
        }
        else if (selectedTube != null && targetTube == null)
        {

            this.OnReceveiveBall(hit);
            if (hit.collider.gameObject.GetComponent<TubeBallSpawn>().listBall.Count == 4)
            {
                if (hit.collider.gameObject.GetComponent<TubeBallSpawn>().CheckCompletedTube())
                {
                    this.completedTubes += 1;
                    Debug.Log("số tube đã completed là:" + this.completedTubes);
                }

            }
        }
        else return;
    }

    private void RemoveBall(RaycastHit2D hit)
    {
        this.selectedTube = hit.collider.transform.gameObject;
        this.ballTemp = this.selectedTube.GetComponent<TubeBallSpawn>().MoveBallUp();
    }

    private void OnReceveiveBall(RaycastHit2D hit)
    {
        this.targetTube = hit.collider.transform.gameObject;
        TubeBallSpawn targetedTube = targetTube.GetComponent<TubeBallSpawn>();

        if (targetedTube.listBall.Count != 4)// điều kiện để chỉ khi nào targeted tube có số ball < 4 mới nhận thêm ball
        {
            this.targetTube.GetComponent<TubeBallSpawn>().ReceiveBall(this.ballTemp);
            // count point when receive ball
            PointManager.Instance.PointCount();
            this.ResetSelection();
        }
        else
        {
            this.targetTube = null;
        }
    }

    private void CheckCompleteLevel()
    {
        if (numberTubesToCompleteLvl == completedTubes)
        {
            Debug.Log("đã hoàn thành level nên popup một cái thông báo gameover ở đây");
        }
        else return;
    }

}    
