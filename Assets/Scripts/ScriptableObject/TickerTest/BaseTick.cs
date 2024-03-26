using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTick : MonoBehaviour, ITick
{
    [SerializeField] TickCollection<ITick> earlyTickCollection;
    [SerializeField] TickCollection<ITick> tickCollection;
    [SerializeField] TickCollection<ITick> fixedTickCollection;
    [SerializeField] TickCollection<ITick> lateTickCollection;



    public void EarlyTick()
    {
        throw new System.NotImplementedException();
    }

    public void FixedTick()
    {
        throw new System.NotImplementedException();
    }

    public void LateTick()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }
}
