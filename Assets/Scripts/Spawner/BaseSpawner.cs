using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawn
{
    public Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation);
}

public abstract class BaseSpawner : MonoBehaviour, ISpawn
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
