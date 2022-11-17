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
        if (_spawnPool == null)
            return;

        GameObject prefab = _spawnPool.GetRandomPrefab();

        if (prefab == null)
            return;

        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
