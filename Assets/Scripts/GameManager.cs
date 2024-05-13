using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using DataStorage;
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
    public LevelData levelData = new LevelData();
    public static event EventHandler OnWin;
    private int tubeForAdding = 1;
    public GameState gameState { get; set; }
    public static GameManager Instance { get; private set; }
    private Camera mainCamera;
    [SerializeField] private List<Transform> tubePrefabs = new List<Transform>();
    private GameFieldData gameFieldData;
    public Tube selectedTube ;
    public Tube targetTube ;
    private List<Ball> runtimeListBall = new List<Ball>();
    private List<Tube> runtimeListTube = new List<Tube>();
    private List<Vector2> subTubePoses = new List<Vector2>();
    public List<Ball> ballTemp;
    private BallTypeSO tempBallType;
    [SerializeField] private TubeRuntimeSetList tubeRuntimeSetList;
    [SerializeField] private BallRuntimeSetList ballRuntimeSetList;
    public int TubesNumber;
    private int maxTubeInOneLine = 5;
    private int rows;
    private float xSpacing ;
    private float offSet = .5f;
    private int target;

    
    //public LevelManager levelManager;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        mainCamera = Camera.main;
        Instance = this;
        
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    private void SetWinCondition()
    {
        target = gameFieldData.maxBallNumer;
    }


    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded()
    {
        gameState = GameState.NONE;
    }

    public void InitGameField()  
    {
        tubePrefabs.Add(Resources.Load<Transform>("TubesPrefab"));

        gameFieldData = levelData.fields[0];
        TubesNumber = gameFieldData.tubes.Count;
        SetWinCondition();
        SetTubePosition(TubesNumber);
        InitTube();
        InitBall();
    }

    
    private void InitBall() // let ball spawner do the thing
    {
        for (int i = 0; i < runtimeListTube.Count; i++)
        {
            List<TubesForEditor> tubesForEditors = gameFieldData.tubes;
            List<BallPos> listBallPos = runtimeListTube[i].GetBallPosList();
            for (int j = 0; j < tubesForEditors[i].listBallPost.Count; j++)
            {
                if (tubesForEditors[i].listBallPost[j].itemForEditor.color != BallColor.NONE)
                {
                    Ball ball = Ball.Create(listBallPos[j].transform);
                    IColorableComponent colorableComponent = ball.GetComponent<IColorableComponent>();
                    colorableComponent.SetSprite(tubesForEditors[i].listBallPost[j].itemForEditor.color, colorableComponent.ActiveBallList);
                    ball.SetIndex(j);
                    ball.transform.SetParent(this.transform);
                    listBallPos[j].ballPosData.SetBallObj(ball);
                    listBallPos[j].ballPosData.SetData(runtimeListTube[i].GetTubeIndex(), j, listBallPos[j].transform.position);
                    runtimeListBall.Add(ball);
                }
                else break;
            }
        }
    }

    private void InitTube()
    {
        List<Vector2> positions = SetTubePosition(TubesNumber);
        for (int i = 0; i < positions.Count; i++)
        {
            Tube tube = Tube.Create(tubePrefabs[0], positions[i],i);
            tube.tubeData.SetIndex(i);
            // spawn ball pos
            tube.InitBallPos(tube.testingSprite);
            runtimeListTube.Add(tube);
            tube.transform.SetParent(this.transform);
        }
        
    }

    public List<Vector2> SetTubePosition(int tubeNeedSpawn)
    {
        List<Vector2> tubePositions = new List<Vector2>();
        if (tubeNeedSpawn <= maxTubeInOneLine)
        {
            tubePositions = SetTubePositionForEachRow(tubeNeedSpawn, 0, 0);
        }

        // spawn tube position
        float screenHeight = Screen.height;
        if (tubeNeedSpawn > maxTubeInOneLine)
        {
            int startIndex = 0;
            rows = 2;
            float rowHeight = screenHeight / 3;
            Vector2 position = mainCamera.ScreenToWorldPoint(new Vector2(0, rowHeight));
            List<float> yPositionList = new List<float>() { -position.y, position.y };
            int remainingTube = tubeNeedSpawn;
            if (tubeNeedSpawn % 2 != 0)
            {

                int lineOneTube = tubeNeedSpawn / 2 + 1;
                for (int i = 0; i < rows; i++)
                {
                    float spawnY = yPositionList[i];
                    tubePositions.AddRange(SetTubePositionForEachRow(lineOneTube, spawnY, startIndex));
                    remainingTube -= lineOneTube;
                    
                    if (remainingTube > 0)
                    {
                        lineOneTube = remainingTube;
                        startIndex = tubeNeedSpawn - lineOneTube + 1;
                    }
                }
            }

            if (tubeNeedSpawn % 2 == 0)
            {
                int lineOneTube = tubeNeedSpawn / 2;
                for (int i = 0; i < rows; i++)
                {
                    float spawnY = yPositionList[i];
                    tubePositions.AddRange(SetTubePositionForEachRow(lineOneTube, spawnY, startIndex));
                    remainingTube -= lineOneTube;

                    if (remainingTube > 0)
                    {
                        lineOneTube = remainingTube;
                        startIndex = tubeNeedSpawn - lineOneTube + 1;
                    }

                }
            }
        }
        return tubePositions;
    }

    public List<Vector2> SetTubePositionForRefactoring(int tubeNeedSpawn)
    {
        List<Vector2> tubePositions = new List<Vector2>();
        if (tubeNeedSpawn <= maxTubeInOneLine)
        {
            tubePositions = SetTubePositionForEachRow(tubeNeedSpawn, 0, 0);
            return tubePositions;
        }

        // spawn tube position
        float screenHeight = Screen.height;
        
        int startIndex = 0;
        // calculate rows
        if(tubeNeedSpawn % maxTubeInOneLine == 0)
        {
            rows = tubeNeedSpawn / maxTubeInOneLine;
            Debug.Log("rows" + rows);
        }

        else
        {
            rows = (tubeNeedSpawn / maxTubeInOneLine) + 1;
            Debug.Log("rows" + rows);
        }

        // calculate yposition for each row
        float rowHeight = screenHeight / (rows + 1);

        List<Vector2> yPositionList = new List<Vector2>();
        for (int i = 1; i<= rows; i++)
        {
            yPositionList.Add(mainCamera.ScreenToWorldPoint(new Vector2(0, rowHeight * i)));
            Debug.Log("yposlist count" + yPositionList.Count);
            Debug.Log(yPositionList[i-1]);
        }


        int remainingTube = tubeNeedSpawn;
        int index = 0;
        // spawn tube base on calculated position
        while (remainingTube > 0)
        {
            int lineOneTube;


            if (remainingTube % rows != 0)
            {
                lineOneTube = (remainingTube / rows) + 1;
            }

            else
            {
                lineOneTube = remainingTube / rows;
            }
            Debug.Log("tube in 1 line" + lineOneTube + "line index " + index);
            float spawnY = yPositionList[index].y;
            tubePositions.AddRange(SetTubePositionForEachRow(lineOneTube, spawnY, startIndex));
            index++;
            remainingTube -= lineOneTube;
            rows -= 1;
            if (remainingTube > 0)
            {
                startIndex = tubeNeedSpawn - lineOneTube + 1;
            }

        }
        return tubePositions;
    }

    private List<Vector2> SetTubePositionForEachRow(int tubeNeedSpawn, float spawnY, int startIndex)
    {
        List<Vector2> tubePostitions = new List<Vector2>();
        xSpacing = CalculateSpacing(levelData);
        float screenHalfWidth = mainCamera.aspect * mainCamera.orthographicSize;
        // calculate spacing between tube based on screen size
        for (int i = 0; i < tubeNeedSpawn; i++)
        {
            //spawn tube
            float xPosition = (i * xSpacing) - ((tubeNeedSpawn - 1) * xSpacing / 2.0f);
            xPosition = Mathf.Clamp(xPosition, -screenHalfWidth + tubePrefabs[0].transform.localScale.x / 2.0f + offSet, screenHalfWidth - tubePrefabs[0].transform.localScale.x / 2.0f - offSet);
            Vector3 position = new Vector3(xPosition, spawnY, 0);
            startIndex++;
            tubePostitions.Add(position);
        }
        return tubePostitions;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Tube>() != null)
            {
                OnTubeClicked(hit.collider.GetComponent<Tube>());
            }
        }

        CheckGameState();
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
                        targetTube.CheckIsCompletedTube();

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

                if (!runtimeListTube[tubeIndex].tubeData.isCompleted)
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
                    else ResetSelection();
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

                targetTube = runtimeListTube.Where(t=>t.index == tubeIndex).Select(t=>t).First();

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
    
    private float CalculateSpacing(LevelData level)
    {
        var tubeNumber = levelData.fields[0].tubes.Count;
        float xSpacingInPixels = (Screen.width - (tubePrefabs[0].localScale.x * tubeNumber)) / tubeNumber;
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
        return runtimeListTube[runtimeListTube.Count - 1];
    }

    public void AddTube()
    {
        int TubesNumberAfterAdd = TubesNumber + tubeForAdding;
        if (TubesNumberAfterAdd == runtimeListTube.Count ) return;
        subTubePoses = SetTubePosition(TubesNumberAfterAdd);
        Tube tube = Tube.Create(tubePrefabs[0], subTubePoses[subTubePoses.Count - 1], TubesNumberAfterAdd - 1);
        tube.transform.SetParent(this.transform);
        tube.InitBallPos(tube.testingSprite);
        runtimeListTube.Add(tube);
        RepositionTubes();
    }

    private void RepositionTubes()
    {
        for (int i = 0; i< runtimeListTube.Count -1; i++)
        {
            Debug.Log($"tube num {i} repositioned");
            runtimeListTube[i].Reposition(subTubePoses[i]);
        }
    }

    public bool IsCompleteLevel()
    {
        int count = 0;
        foreach (var tube in runtimeListTube)
        {
            if (tube.tubeData.isCompleted)
            {
                count++;
            }
            
        }
        
        if (count == target)
        {
            LevelManager.Instance.LevelState = LevelState.Complete;
            return true;
        }
        else return false;
    }

    //check level complete and make a popup
    public void ResetLevel()
    {
        foreach(var tube in runtimeListTube)
        {
            Destroy(tube.gameObject);
        }
        foreach (var ball in runtimeListBall)
        {
            Destroy(ball.gameObject);
        }
        runtimeListBall.Clear();
        runtimeListTube.Clear();
        subTubePoses.Clear();
        Destroy(gameObject);
    }
}    
