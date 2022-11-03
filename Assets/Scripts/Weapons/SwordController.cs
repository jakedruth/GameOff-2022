using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Actor _owner;
    private Sword _swordPrefab;

    [SerializeField] private Transform _spawnPoint;
    private Sword _instance;
    [SerializeField] private float _damage;

    void Awake()
    {
        // TODO: Make this string public and changeable?
        _swordPrefab = Resources.Load<Sword>("Prefabs/Sword");
    }

    public void SetOwner(Actor actor)
    {
        _owner = actor;
    }

    public void StartAttack()
    {
        _instance = Instantiate(_swordPrefab, _spawnPoint, false);
        _instance.SetOnHitCallBack(OnHit);
    }

    public void EndAttack()
    {
        Destroy(_instance.gameObject);
    }

    public void OnHit(Collision collision)
    {
        Debug.Log("Hit Collider: " + collision.gameObject.name);
        Actor hit = collision.gameObject.GetComponent<Actor>();

        if (hit != null && hit != _owner)
        {
            hit.ApplyDamage(_damage);

            // Push the target away
            // TODO: Should move the position quickly overtime (0.1s?) instead of instantly
            // hit.transform.position += transform.forward * 2;
        }
    }
}
