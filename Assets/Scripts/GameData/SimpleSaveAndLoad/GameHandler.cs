using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameHandler : MonoBehaviour
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/saves/";
    public static bool Initialized;

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Initialized = true;
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        else Initialized = true;
    }

    public static void Save(string saveString) {
        if (!Initialized)
        {
            Init();
        }
        File.WriteAllText(SAVE_FOLDER + "save_" + ".txt", saveString);
    }

    public static string Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.txt");
        FileInfo mostRecentFile = null;
        foreach (var fileInfo in fileInfos) {
            if(mostRecentFile == null) {
                mostRecentFile = fileInfo;
            }
            else
            {
                if(fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileInfo;
                }
            }

        }

        if(mostRecentFile != null) {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        }

        else
        {
            return null;
        }
    }

}

public class SaveObject
{
    public int levelReached;
}
