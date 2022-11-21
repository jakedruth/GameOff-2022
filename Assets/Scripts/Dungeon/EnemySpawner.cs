using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I hate the name Enemy Room Handler. It should be Enemy Spawner....
[RequireComponent(typeof(BoxCollider))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool _spawnOnce;
    private bool _didOnce;
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;
    private int _activeEnemyCount;
    public UnityEngine.Events.UnityEvent onAllEnemiesDestroyed = new UnityEngine.Events.UnityEvent();

    public void SpawnEnemies()
    {
        if (_spawnOnce && _didOnce)
            return;

        _didOnce = true;

        _activeEnemyCount = 0;
        for (int i = 0; i < _enemySpawnPoints.Length; i++)
        {
            Actor enemy = _enemySpawnPoints[i].SpawnEnemy(transform);
            _activeEnemyCount++;
            enemy.onDeath.AddListener(() =>
            {
                _activeEnemyCount--;
                if (_activeEnemyCount <= 0)
                {
                    onAllEnemiesDestroyed.Invoke();
                }
            });
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_enemySpawnPoints == null)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < _enemySpawnPoints.Length; i++)
        {
            Vector3 point = transform.TransformPoint(_enemySpawnPoints[i].point);
            Gizmos.DrawRay(point, Vector3.up * 2);
        }
    }
}

[System.Serializable]
public struct EnemySpawnPoint
{
    public Actor enemy;
    public Vector3Int point;

    public Actor SpawnEnemy(Transform transform)
    {
        Actor spawn = Object.Instantiate(enemy, transform.TransformPoint(point), Quaternion.identity);
        return spawn;
    }
}
