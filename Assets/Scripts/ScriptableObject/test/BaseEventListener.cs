using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<TEvent,TResponse> : MonoBehaviour , IEventListener
    where TEvent : BaseEvent
    where TResponse : UnityEvent
{
    [SerializeField] protected TResponse response;
    [SerializeField] TEvent Event;

    public void OnEventRaised()
    {
        response.Invoke();
    }

    public void OnEnable()
    {
        Event.AddListener(this);
    }

    public void OnDisable()
    {
        Event.RemoveListener(this);
    }
}

public class BaseEventListener<TType,TEvent,TResponse> : MonoBehaviour,IEventListener<TType>
    where TEvent : BaseEvent<TType>
    where TResponse : UnityEvent<TType>
{
    [SerializeField] protected TResponse response;
    [SerializeField] TEvent Event;

    public void OnEventRaised(TType data)
    {
        response.Invoke(data);
    }
    public void OnEnable()
    {
        Event.AddListener(this);
    }

    public void OnDisable()
    {
        Event.RemoveListener(this);
    }
}