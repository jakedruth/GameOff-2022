using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public Actor actor { get; private set; }
    [SerializeField] private SpawnPool_SO _spawnPool;

    void Awake()
    {
        actor = GetComponent<Actor>();
        actor.onDeath.AddListener(SpawnItem);
    }

    private void SpawnItem()
    {
        GameObject prefab = _spawnPool.GetRandomPrefab();
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
