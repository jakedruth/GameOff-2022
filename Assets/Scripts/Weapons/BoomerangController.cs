using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    private Actor _owner;
    private Boomerang _boomerangPrefab;

    [SerializeField] private Transform _spawnPoint;
    private Boomerang _instance;

    [Header("Boomerang Values")]
    [SerializeField] private float _damage;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _distance;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _waitTime;

    void Awake()
    {
        _boomerangPrefab = Resources.Load<Boomerang>("Prefabs/Boomerang");
    }

    public void SetOwner(Actor actor)
    {
        _owner = actor;
    }

    public void StartAttack()
    {
        if (_instance != null)
            return;

        _instance = Instantiate(_boomerangPrefab, _spawnPoint.position, _spawnPoint.rotation);
        _instance.SetOnHitCallBack(OnHit);
        _instance.InitParameters(_owner, _moveTime, _distance, _acceleration, _waitTime);
    }

    public void EndAttack()
    {
        //Destroy(_instance.gameObject);
        //_instance = null;
    }

    public void OnHit(Collision collision)
    {
        string otherTag = collision.gameObject.tag;

        switch (otherTag)
        {
            case "Player":
                Destroy(_instance.gameObject);
                _instance = null;
                break;
            case "Switch":
                _instance.ReturnImmediately();
                Switch s = collision.gameObject.GetComponent<Switch>();
                s.ActivateSwitch();
                break;
            case "Enemy":
                _instance.ReturnImmediately();
                Actor hit = collision.gameObject.GetComponent<Actor>();

                if (hit != null && hit != _owner)
                {
                    hit.ApplyDamage(_damage);
                }
                break;
            case "Pickup":
                Pickup pickup = collision.gameObject.GetComponent<Pickup>();
                pickup.DisableCollision();
                _instance.grabbedItems.Add(pickup.transform);
                break;
            default:
                _instance.ReturnImmediately();
                break;
        }
    }
}
