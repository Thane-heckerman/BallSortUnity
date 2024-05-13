using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu]
public class BaseEvent : ScriptableObject
{
    private List<IEventListener> eventListeners = new List<IEventListener>();

    public void Raise()
    {
        for(int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised();
        }
    }

    public void AddListener(IEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        eventListeners.Add(listener);
    }

    public void RemoveListener(IEventListener listener)
    {
        if (eventListeners.Contains(listener))
        eventListeners.Remove(listener);
    }
}

public class  BaseEvent<TType> : ScriptableObject
{
    private List<IEventListener<TType>> eventListeners = new List<IEventListener<TType>>();

    public void Raise(TType data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(data);
        }
    }

    public void AddListener(IEventListener<TType> listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void RemoveListener(IEventListener<TType> listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
[CustomEditor(typeof(BaseEvent), true)]
public class BaseEventEditor : Editor
{
    BaseEvent baseEvent;

    void OnEnable()
    {
        baseEvent = target as BaseEvent;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Raise"))
        {
            baseEvent.Raise();
        }
    }
}