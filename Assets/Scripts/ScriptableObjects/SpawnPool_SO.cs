using System.Runtime.Versioning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDR.Utils;


[CreateAssetMenu(fileName = "Spawn Pool", menuName = "GameOff-2022/SpawnPool")]
public class SpawnPool_SO : ScriptableObject
{
    public List<WeightedElement<GameObject>> spawnList;

    public GameObject GetRandomPrefab()
    {
        return spawnList.GetRandomElement();
    }
}
