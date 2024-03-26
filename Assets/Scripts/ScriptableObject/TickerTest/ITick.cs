using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITick 
{
    void EarlyTick();
    void Tick();
    void FixedTick();
    void LateTick();
}
