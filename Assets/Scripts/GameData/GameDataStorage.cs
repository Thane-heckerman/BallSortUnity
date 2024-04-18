using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DataStorage
{
    public class GameDataStorage
    {
        string BackupPath => path + "-bak";
        string path;

        PersistentData data;

        public GameDataStorage(string name)
        {
            path = GetDataPath(name);
            var bakPath = BackupPath;

            if (File.Exists(bakPath) && !File.Exists(path))
            {
                File.Move(bakPath, path);
            }

            if (!File.Exists(path))
            {
                data = new PersistentData();
                return;
            }

            try
            {
                using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    data = Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }

            if (data == null)
            {
                data = new PersistentData();
            }
        }

        public void Save()
        {
            var bakPath = BackupPath;
            var tmpPath = path + "-tmp";

            try
            {
                if (File.Exists(path))
                {
                    if (File.Exists(bakPath)) File.Delete(bakPath);
                    File.Move(path, bakPath);
                }

                using (var stream = new FileStream(tmpPath, FileMode.Create))
                {
                    Serialize(data, stream);
                }

                File.Move(tmpPath, path);
                File.Delete(bakPath);
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Saving {0} error {1} {2}", path, e.Message, e.StackTrace));
                throw;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                Serialize(data, stream);
            }

            Debug.LogFormat("Saving {0} successfully", path);
        }

        string GetDataPath(string name)
        {
            var persistentDataPath = GetPersistentDataPath();
            if (!Directory.Exists(persistentDataPath))
            {
                Directory.CreateDirectory(persistentDataPath);
            }
            return Path.Combine(persistentDataPath, name);
        }

        public static string GetPersistentDataPath()
        {
#if UNITY_EDITOR
            return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "TempDataStorage");
#else
            return Application.persistentDataPath;
#endif
        }

        public object this[string key]
        {
            get => data.TryGetValue(key, out var pd) ? pd : default;
            set => data.Set(key, value);
        }

        public bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public void Remove(string key)
        {
            data.Remove(key);
        }

        public void Clear()
        {
            data.Clear();
        }

        void Serialize(object d, Stream stream)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, d);
        }

        PersistentData Deserialize(Stream stream)
        {
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream) as PersistentData;
        }

        T To<T>(object input, T defaultValue)
        {
            var result = defaultValue;

            if (typeof(T).IsEnum)
            {
                result = (T)Enum.ToObject(typeof(T), To(input, Convert.ToInt32(defaultValue)));
            }
            else
            {
                result = (T)Convert.ChangeType(input, typeof(T));
            }

            return result;
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            if (data.TryGetValue(key, out var value))
            {
                return To(value, defaultValue);
            }

            return defaultValue;
        }

        public void Set<T>(string key, T value)
        {
            this[key] = value;
        }

        public void Load(IDataPersistent persistentData, bool root = false)
        {
            data.Load(persistentData, root);
        }

        public void Store(IDataPersistent persistentData, bool root = false)
        {
            data.Store(persistentData, root);
        }
    }
}

