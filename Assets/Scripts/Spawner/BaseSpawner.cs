using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseSpawner : MonoBehaviour
{
    public List<Transform> prefabs;

    public int spawnedCount = 0;

    protected virtual void Awake()
    {
        this.LoadPrefab();
    }


    protected virtual void LoadPrefab()
    {
        
    }

    protected virtual Transform GetPrefabByName(string prefabName)
    {
        foreach ( var prefab in prefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null;
    }

    public virtual Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        Transform prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found: " + prefabName);
            return null;
        }

        return this.Spawn(prefab, spawnPos, rotation);
    }

    public virtual Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newPrefab = Instantiate(prefab,spawnPos,rotation);

        this.spawnedCount++;

        return newPrefab;
    }

    public virtual Transform Spawn(Transform prefab)
    {
        Transform newPrefab = Instantiate(prefab, transform.position, Quaternion.identity);

        this.spawnedCount++;

        return newPrefab;
    }


    public virtual Transform Spawn()
    {
        Transform newPrefab = Instantiate(prefabs[0], transform.position, Quaternion.identity);
        return newPrefab;
    }

}

public abstract class BaseSpawner<T> : MonoBehaviour 
{
    public int numberToSpawn;

    public List<Transform> prefabs;

    public int spawnedCount = 0;


    public Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform obj = Instantiate(prefab, spawnPos, rotation);
        return obj;
    }

    public T Spawn()
    {
        Transform obj = Instantiate(prefabs[0]);
        T typeObj = obj.GetComponent<T>();
        return typeObj;
    }

    public virtual List<T> SpawnABunch()
    {
        List<T> list = new List<T>();

        for ( int i = 0; i < numberToSpawn; i++)
        {
            Transform obj = Instantiate(prefabs[0]);
            list.Add(obj.GetComponent<T>());
        }

        return list;
    }
}
