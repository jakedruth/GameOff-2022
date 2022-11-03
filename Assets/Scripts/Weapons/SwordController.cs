using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Pawn _owner;
    private Sword _swordPrefab;

    [SerializeField] private Transform _spawnPoint;
    private Sword _instance;
    [SerializeField] private float _damage;

    void Awake()
    {
        // TODO: Make this string public and changeable?
        _swordPrefab = Resources.Load<Sword>("Prefabs/Sword");
    }

    public void SetOwner(Pawn pawn)
    {
        _owner = pawn;
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

    public void OnHit(Collider collider)
    {
        Debug.Log("Hit Collider: " + collider.gameObject.name);
        Actor hit = collider.GetComponent<Actor>();

        if (hit != null && hit != _owner)
        {
            hit.TakeDamage(_damage, _owner);
        }
    }
}
