using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DataStorage;
using System;
using System.IO;


public class ShopUIPreview : MonoBehaviour
{
    public static ShopUIPreview Instance { get; private set; }
    private Transform tubePrefab;
    private BallSpawner ballSpawner;
    private TubeSpawner tubeSpawner;
    private int activeBallTypeList;
    private Transform tubePreviewTransform;
    private List<BallPos> ballPoses;
    private List<Transform> balls = new List<Transform>();
    private bool Initilized;
    const string PREVIEW_BALL_TYPE_LIST_INDEX = "PREVIEW_BALL_TYPE_INDEX";
    private List<BallColor> previewBallColor = new List<BallColor>()
                            { BallColor.RED,BallColor.GREEN,BallColor.YELLOW,BallColor.PURPLE};

    public int PreviewBallTypeListIndex
    {
        get => GameData.Get(PREVIEW_BALL_TYPE_LIST_INDEX, 0);
        set => GameData.Set(PREVIEW_BALL_TYPE_LIST_INDEX, value);
    }

    private void Awake()
    {
        Instance = this;
        tubeSpawner = GetComponent<TubeSpawner>();
        ballSpawner = GetComponent<BallSpawner>();
    }


    private void OnEnable()
    {
        UIShopManager.OnShopUIEnable += UIShopManager_OnShopUIEnable;
    }

    private void UIShopManager_OnShopUIEnable(object sender, EventArgs e)
    {
        SpawnTubePreview();
        SpawnBallPreview(ballPoses);
        SetColorBallPreview(balls);
        activeBallTypeList = GameData.Get("ACTIVE_BALL_LIST", 0);
        UpdateSprite(activeBallTypeList);
    }

    private void OnDisable()
    {
        
        UIShopManager.OnShopUIEnable -= UIShopManager_OnShopUIEnable;
    }

    public void Clear()
    {
        if(ballPoses != null)
        {
            ballPoses.Clear();
        }
        if(balls.Count != 0) {
        balls.Clear();
        }
        tubePreviewTransform = null;
        if(tubePrefab != null)
        {
            Destroy(tubePrefab.gameObject);
        }
        tubePrefab = null;
        PreviewBallTypeListIndex = 0;
    }

    private void SpawnTubePreview() {
        tubePrefab = tubeSpawner.Spawn(tubeSpawner.prefabs[0], tubeSpawner.pos, Quaternion.identity);
        //tubePrefab.transform.localScale = new Vector2 (1.2f,1.2f);
        tubePrefab.SetParent(this.transform);
        Tube tube = tubePrefab.GetComponent<Tube>();
        tube.InitBallPos(tube.testingSprite);
        ballPoses = tubePrefab.GetComponent<Tube>().GetBallPosList();
    }

    // refactor to use ballspawner
    private void SpawnBallPreview(List<BallPos> ballPoses)
    {
        foreach (var ballPos in ballPoses) {
            Transform ballTransform = ballSpawner.Spawn(ballSpawner.prefabs[0], ballPos.transform.position, Quaternion.identity);
            ballTransform.SetParent(ballPos.transform);
            //ballTransform.localScale = tubePrefab.localScale;
            balls.Add(ballTransform);
            
        }
    }

    public void Toggle(bool enable) {
        gameObject.SetActive(enable);
    }

    private void SetRandomBallColor()
    {
        var array = Enum.GetValues(typeof(BallColor));
        System.Random random = new System.Random();
        List<BallColor> assignedColors = new List<BallColor>();

        foreach (var ball in balls)
        {
            BallColor color;

            do
            {
                color = (BallColor)array.GetValue(random.Next(array.Length));
            } while (assignedColors.Contains(color));

            ball.GetComponent<Ball>().ballData.SetBallColor(color);
            assignedColors.Add(color);
        }
    }

    private void SetColorBallPreview(List<Transform> balls)
    {
        if (balls.Count > 0)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].GetComponent<Ball>().ballData.SetBallColor(previewBallColor[i]);
            }
        }
        else Debug.Log("No ball");
    }

    private List<Ball> GetBalls()
    {
        return tubePrefab.GetComponent<Tube>().GetBalls(); 
    }

    public void UpdateSprite(int index)
    {
        Debug.Log("instanceID" + GetInstanceID());
        GameData.Set(PREVIEW_BALL_TYPE_LIST_INDEX, index);
        Debug.Log("preview ball list :" + GameData.Get(PREVIEW_BALL_TYPE_LIST_INDEX, 0));
        foreach ( var ball in balls) {
            ball.GetComponent<IColorableComponent>().SetSprite(ball.GetComponent<Ball>().ballData.Color, index);
            
        }

    }

    
}
