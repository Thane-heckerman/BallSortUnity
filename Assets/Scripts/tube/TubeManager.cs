using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeManager : MonoBehaviour
{
    public GameCtrl gameCtrl;
    public int tubeNum;// để spawn tube
    public GameObject tubePrefab;// để spawn tube
    public List<GameObject> tubesClone; // để ballspawn truy cập spawn ball script
    private Vector2 tubeSpawnPos;

    private void Awake()
    {
        this.gameCtrl = GetComponent<GameCtrl>();
    }

    public List<GameObject> SpawnTube(GameObject tubeSpawner, int level)
    {
        this.tubeNum = LevelManager.Instance.levelList.levels[level].tubes.Count;
        GameObject tubeClone = Instantiate(this.tubePrefab, tubeSpawner.transform.position, tubeSpawner.transform.rotation);
        tubeClone.SetActive(true);
        this.tubesClone.Add(tubeClone);
        return this.tubesClone;
    }
    // get ball pos list chuyển sang ball spawn script

    // Spawn quả bóng với màu sắc tương ứng
    //vòng lặp qua từng phần tử của ballposlist và spawn prefab theo số theo giải thuật của gpt

   
}
