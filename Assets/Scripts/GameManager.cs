using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public enum GameState {
    NONE,
    OnPopBall,
    OnPopBallCompleted,
    OnFallingBall,
    OnFallingBallCompleted,
    OnGetBallBack,
    OnGetBallBackCompleted,
}

public class GameManager : MonoBehaviour
{

    private int tubeForAdding = 1;
    private GameState gameState;
    public static GameManager Instance { get; private set; }
    public LevelManager levelManager;
    private Camera mainCamera;
    private List<Transform> tubePrefabs = new List<Transform>();
    private GameFieldData gameFieldData;
    public Tube selectedTube ;
    public Tube targetTube ;
    private List<Ball> runtimeListBall = new List<Ball>();
    private List<Tube> runtimeListTube = new List<Tube>();

    private int levelSecondRowTubeNumber;

    //public event EventHandler<OnTubeReceiveBallArgument> OnTubeReceiveBall;
    //public class OnTubeReceiveBallArgument
    //{
    //    public List<Ball> ballList;
    //}
    //public event EventHandler<OnTubeClickEventArgument> OnTubeClickEvent;

    //public class OnTubeClickEventArgument {
    //    public List<int> indexList;
    //}

    private List<Vector2> subTubePoses = new List<Vector2>();
    public List<Ball> ballTemp;
    private BallTypeSO tempBallType;
    [SerializeField] private TubeRuntimeSetList tubeRuntimeSetList;
    [SerializeField] private BallRuntimeSetList ballRuntimeSetList;
    public int currentLevel;
    public int TubesNumber;
    private int maxTubeInOneLine = 8;
    private int rows;
    private float xSpacing ;
    private float offSet = .5f;
    private bool isCompleteLevel;
    //public LevelManager levelManager;

    private void Awake()
    {
        Instance = this;
        gameFieldData = levelManager.levelDataContainer.levelData.fields[0];
        foreach (var tube in gameFieldData.tubes)
        {
            tubePrefabs.Add(tube.TubePrefab.transform);
        }
    }

    void Start()
    {
        mainCamera = Camera.main;

        LoadLevel();
        gameState = GameState.NONE;
        
        

    }
    public void LoadLevel()
    {
        TubesNumber = levelManager.levelDataContainer.levelData.fields[0].tubes.Count;
        InitGamePlay(TubesNumber);

    }

    private void InitGamePlay(int tubesNumber)
    {

        if (tubesNumber <= maxTubeInOneLine)
        {
            SpawnTubeRow(tubesNumber, 0,0);
        }

        float screenHeight = Screen.height;
        if (tubesNumber > maxTubeInOneLine)
        {
            int startIndex = 0;
            rows = 2;
            float rowHeight = screenHeight / 3;
            Vector2 position = mainCamera.ScreenToWorldPoint(new Vector2(0, rowHeight));
            List<float> yPositionList = new List<float>() { -position.y,position.y};
            int remainingTube = tubesNumber;
            if (tubesNumber % 2 != 0)
            {
                
                int lineOneTube = tubesNumber / 2 + 1;
                for (int i = 0; i < rows; i++)
                {
                    float spawnY = yPositionList[i];
                    
                    SpawnTubeRow(lineOneTube, spawnY, startIndex);
                    remainingTube -= lineOneTube;
                    ;
                    if (remainingTube > 0)
                    {
                        lineOneTube = remainingTube;
                        startIndex = tubesNumber - lineOneTube + 1;
                    }
                }
            }
            
            if (tubesNumber % 2 == 0)
            {
                int lineOneTube = tubesNumber / 2;
                for (int i = 0; i < rows; i++)
                {
                    float spawnY = yPositionList[i]; 
                    SpawnTubeRow(lineOneTube, spawnY, startIndex); 
                    remainingTube -= lineOneTube;
                    
                    if (remainingTube > 0)
                    {
                        lineOneTube = remainingTube;
                        startIndex = tubesNumber - lineOneTube + 1;
                    }

                }
            }
        }
    }

    private void SpawnTubeRow(int tubeNeedSpawn ,  float spawnY, int startIndex)
    {
        xSpacing = CalculateSpacing(tubeNeedSpawn);
        float screenHalfWidth = mainCamera.aspect * mainCamera.orthographicSize;
        // calculate spacing between tube based on screen size
        for (int i = 0; i < tubeNeedSpawn; i++)
        {
            float xPosition = (i * xSpacing) - ((tubeNeedSpawn - 1) * xSpacing / 2.0f);
            xPosition = Mathf.Clamp(xPosition, -screenHalfWidth + tubePrefabs[i].transform.localScale.x / 2.0f + offSet, screenHalfWidth - tubePrefabs[i].transform.localScale.x / 2.0f - offSet);
            Vector3 position = new Vector3(xPosition, spawnY, 0);
            Tube tube = Tube.Create(tubePrefabs[i], position, startIndex);
            tube.tubeData.SetIndex(startIndex);
            tube.InitBallPos();
            List<TubesForEditor> tubesForEditors = gameFieldData.tubes;
            List<BallPos> listBallPos = tube.GetBallPosList();
            for (int j = 0; j < listBallPos.Count; j++)
            {
                if (tubesForEditors[i].listBallPost[j].itemForEditor.ballType != null)
                {
                    Ball ball = Ball.Create(listBallPos[j].transform);
                    IColorableComponent colorableComponent = ball.GetComponent<IColorableComponent>();
                    ball.ballData.SetBallType(tubesForEditors[i].listBallPost[j].itemForEditor.ballType);
                    ball.SetIndex(j);
                    listBallPos[j].ballPosData.SetBallObj(ball);
                    listBallPos[j].ballPosData.SetData(tube.GetTubeIndex(), j, listBallPos[j].transform.position) ;
                    colorableComponent.SetTypeSprite(tubesForEditors[i].listBallPost[j].itemForEditor.ballType);
                    runtimeListBall.Add(ball);
                }
            }
            startIndex++;
            runtimeListTube.Add(tube);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Tube>() != null)
            {
                OnTubeClicked(hit.collider.GetComponent<Tube>());
            }
        }

        CheckGameState();
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("RM pressed");
            AddTube();
        }
    }

    private void CheckGameState()
    {
        switch (gameState)
        {
            case GameState.NONE:
            case GameState.OnPopBall:
                {
                    if (selectedTube != null)
                    {
                        if (CheckBusyStateOfTube(selectedTube))
                        {
                            gameState = GameState.OnPopBallCompleted;
                            Debug.Log("onPopBallCompleted");
                        }
                    }
                }
                break;
            case GameState.OnFallingBall:
                {
                    if (CheckBusyStateOfTube(selectedTube) && CheckBusyStateOfTube(targetTube))
                    {
                        gameState = GameState.OnFallingBallCompleted;
                        Debug.Log("onFallingBallCompleted");
                        ResetSelection();
                    }
                    else return;
                }
                break;

            case GameState.OnFallingBallCompleted:
                {
                    gameState = GameState.NONE;
                    Debug.Log("GameState == none");
                }
                break;

            case GameState.OnGetBallBack:
                {
                    if(CheckBusyStateOfTube(selectedTube))
                    {
                        if(targetTube != null)
                        {
                            if (CheckBusyStateOfTube(targetTube))
                            {
                                gameState = GameState.OnGetBallBackCompleted;
                                ResetSelection();
                            }
                        }
                        else
                        {
                            gameState = GameState.OnGetBallBackCompleted;
                            ResetSelection();
                        }
                        
                    }
                    
                }
                break;
            case GameState.OnGetBallBackCompleted:
                {
                    gameState = GameState.NONE;
                }
                break;
        }
    }

    private bool CheckBusyStateOfTube(Tube tube)
    {
        return tube.IsNotBusy();
    }

    public void OnTubeClicked(Tube tube)
    {
        int tubeIndex = tube.tubeData.index;
        switch (gameState)
        {
            case GameState.NONE:

                if (!runtimeListTube[tubeIndex].IsCompletedTube())
                {
                    selectedTube = runtimeListTube[tubeIndex];
                    if (runtimeListTube[tubeIndex].CanPopBall())
                    {
                        runtimeListTube[tubeIndex].PopBall();
                        gameState = GameState.OnPopBall;
                        //runtimeListTube[tubeIndex].tubeState = TubeState.Busy;
                        ballTemp = tube.GetAllSameColorNeighBorBall();
                        tempBallType = ballTemp[0].ballData.ballType;
                    }
                }
                break;

            case GameState.OnPopBallCompleted:

                if (tubeIndex == -1)
                {
                    Debug.Log("tube index = -1");
                    break;
                }

                if(tubeIndex == selectedTube.index)
                {
                    Debug.Log("sameTube");
                    GetTheBallBackToTube(selectedTube);
                    break;
                }

                targetTube = runtimeListTube[tubeIndex];

                if(targetTube != null && targetTube.CanReceiveBall(ballTemp.Count,tempBallType))
                {
                    Debug.Log("đã moved ball");
                    ClearAllPopedBallPos();
                    StartCoroutine(MoveBallsToOtherTube(targetTube));
                }
                break;
        }
    }

    private void GetTheBallBackToTube(Tube tube)
    {
        Debug.Log("poped ball back");
        tube.GetLastBallPopedBack();
        gameState = GameState.OnGetBallBack;
    }
    
    private float CalculateSpacing(int tubeNumber)
    {
        float xSpacingInPixels = (Screen.width - (tubePrefabs[0].transform.localScale.x * tubeNumber)) / tubeNumber;
        Vector3 xspacing = mainCamera.ScreenToWorldPoint(new Vector3(xSpacingInPixels, 0, 0));
        Vector3 screenWidthBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float xSpacing = Vector3.Distance(screenWidthBottomLeft, xspacing);
        xSpacing = Mathf.Clamp(xSpacing, tubePrefabs[0].transform.localScale.x, 2f);
        return xSpacing;
    }

    private void ClearAllPopedBallPos()
    {
        if(ballTemp.Count != 0)
        {
            foreach(var i in GetPopedBallIndex())
            {
                selectedTube.ClearPopedBallPos(i);
            }
        }
    }
    private List<int> GetPopedBallIndex()
    {
        List<int> indexes = new List<int>();
        foreach(var ball in ballTemp)
        {
            var index = ball.ballData.index;
            indexes.Add(index);

        }
        return indexes;
    }

    IEnumerator MoveBallsToOtherTube(Tube tube)
    {

        for (int i = 0; i < ballTemp.Count; i++)
        {
            BallPos ballPos = tube.GetBallPosList()[tube.GetAllBallInTubeCount()];
            BallMovement ballMove = ballTemp[i].GetComponent<BallMovement>();
            ballMove.StartCoroutine(ballMove.MoveToTarget(tube.GetUpBallPos().position, ballPos.ballPosData.position));
            //assign data ball to ball pos
            ballPos.AssignBall(ballTemp[i]);
            yield return new WaitForSeconds(.2f);
        }
        gameState = GameState.OnFallingBall;
    }

    private void ResetSelection()
    {
        this.targetTube = null;
        this.selectedTube = null;
        ballTemp = null;
        tempBallType = null;
    }

    private Tube GetLastTube() {
        return runtimeListTube[TubesNumber - 1];
    }

    private List<Tube> GetSecondRowTubes()
    {
        List<Tube> secondRow = new List<Tube>();
        for (int i = TubesNumber - 1; i >=TubesNumber/2 +1; i--)
        {
            secondRow.Add(runtimeListTube[i]);
        }
        return secondRow;
    }

    public void AddTube()
    {
        if(TubesNumber > maxTubeInOneLine)
        {
            GetSubTubesPositionsForAdding(GetSecondRowTubes().Count + tubeForAdding); // creat sub poses 
            int TubesNumberAfterAdd = TubesNumber + tubeForAdding;
            Tube tube = Tube.Create(tubePrefabs[0], subTubePoses[subTubePoses.Count - 1], TubesNumberAfterAdd - 1);
            tube.InitBallPos();
            List<BallPos> listBallPos = tube.GetBallPosList();
            runtimeListTube.Add(tube);
            RepositionTubes();
        }
        if (TubesNumber < maxTubeInOneLine)
        {
            GetSubTubesPositionsForAdding(TubesNumber + tubeForAdding); // creat sub poses
            int TubesNumberAfterAdd = TubesNumber + tubeForAdding;
            Tube tube = Tube.Create(tubePrefabs[0], subTubePoses[subTubePoses.Count - 1], TubesNumberAfterAdd - 1);
            tube.InitBallPos();
            runtimeListTube.Add(tube);
            RepositionTubes();
        }
        // refactor needed
    }
    public void GetSubTubesPositionsForAdding(int tubeNumberAfterAdd)
    {
        
        float screenHalfWidth = mainCamera.aspect * mainCamera.orthographicSize;
        float xSubSpacing = CalculateSpacing(tubeNumberAfterAdd);
        for (int i = 0; i < tubeNumberAfterAdd; i++)
        {
            float xPosition = (i * xSubSpacing) - ((tubeNumberAfterAdd -1 ) * xSubSpacing / 2.0f);
            xPosition = Mathf.Clamp(xPosition, -screenHalfWidth + tubePrefabs[0].transform.localScale.x / 2.0f + offSet, screenHalfWidth - tubePrefabs[0].transform.localScale.x / 2.0f - offSet);
            Vector3 position = new Vector3(xPosition, GetLastTube().gameObject.transform.position.y, 0);
            subTubePoses.Add(position);
        }

        Debug.Log(subTubePoses.Count);
        
    }

    private void RepositionTubes()
    {
        if(TubesNumber > maxTubeInOneLine)
        {
            for (int i = 0; i < GetSecondRowTubes().Count; i++)
            {
                GetSecondRowTubes()[i].Reposition(subTubePoses[i]);
            }
        }
        else
        {
            for (int i = 0; i< runtimeListTube.Count - 1; i++)
            {
                runtimeListTube[i].Reposition(subTubePoses[i]);
            }
        }
    }

    private bool IsCompleteLevel()
    {
        return isCompleteLevel;
    }

    //check level complete and make a popup 

}    
