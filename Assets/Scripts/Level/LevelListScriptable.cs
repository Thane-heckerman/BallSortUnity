using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/LevelListScriptable")]
public class LevelListScriptable : ScriptableObject
{
    public List<LevelDataContainer> LevelDataContainers;
}
