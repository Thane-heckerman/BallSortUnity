using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointManager : MonoBehaviour
{
    //singleton
    public static PointManager Instance { get => instance; }
    private static PointManager instance;
    public TextMeshProUGUI startScoreText;
    

    int startScore = 4000;
    private void Start()
    {
        PointManager.instance = this;
        startScoreText.text = startScore.ToString() + " Points";
    }

    public void PointCount()
    {
        startScore -= 20;
        startScoreText.text = startScore.ToString() + " Points";
    }
}
