using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RuntimeGameObjectList")]
public abstract class RunsetSO : ScriptableObject
{
    private List<GameObject> items = new List<GameObject>();
    public List<GameObject> Items => items;

    public void Add(GameObject item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
        }
    }

    public void Remove(GameObject item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }
    
}


public abstract class GenericRunTimeSetSO<TType> : ScriptableObject
{
    private List<TType> items = new List<TType>();
    public List<TType> Items => items;

    public void Add(TType item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
        }
    }

    public void Remove(TType item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
        }
    }

    public int GetCount()
    {
        return Items.Count;
    }
}
