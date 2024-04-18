using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace DataStorage
{
    public class Storage
    {
        string path;

        public Storage(string name)
        {

        }

        public string GetDataPath(string name)
        {
            var path = GetPersistentDataPath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(GetPersistentDataPath());
            }
            return Path.Combine(path, name);
        }

        public static string GetPersistentDataPath()
        {
            return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "TempDataStorage");
        }
    }
}

