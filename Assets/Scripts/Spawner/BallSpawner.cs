using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : BaseSpawner
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override Transform Spawn(Transform prefab, Vector3 position, Quaternion rotation)
    {
        Transform ballObj = base.Spawn(prefab, position,rotation);

        return ballObj;
    }

}
