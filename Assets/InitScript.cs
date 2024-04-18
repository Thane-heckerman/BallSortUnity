using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class InitScript : MonoBehaviour
{
    public static InitScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("launched") == 0)
        {
            // first time launch
            PlayerPrefs.SetInt("launched", 1);
            PlayerPrefs.Save();
        }


    }
    // loadlevel trong levelmanager bằng playerprefs
    // subcribe levelclicked để lưu cho playerpref key là openlevel
}
