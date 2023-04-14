using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Throwable
{
    private ParticleSystem _particleSystem;

    public Actor Actor { get; private set; }
    [SerializeField] private SpawnPool_SO _spawnPool;

    protected override void Awake()
    {
        base.Awake();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _particleSystem.gameObject.SetActive(false);
        Actor = GetComponent<Actor>();
        Actor.onDeath.AddListener(HandleOnDeath);
    }

    private void HandleOnDeath()
    {
        _particleSystem.transform.SetParent(null);
        _particleSystem.gameObject.SetActive(true);

        if (_spawnPool == null)
            return;

        GameObject prefab = _spawnPool.GetRandomPrefab();

        if (prefab == null)
            return;

        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
