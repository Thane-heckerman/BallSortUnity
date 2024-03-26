using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventListener 
{
    public void OnEventRaised();
}

public interface IEventListener<in TType>
{
    public void OnEventRaised(TType value);
}
