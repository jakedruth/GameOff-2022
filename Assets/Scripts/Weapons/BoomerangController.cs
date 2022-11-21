using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    private Actor _owner;
    private Boomerang _boomerangPrefab;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _maxBoomerangCount;
    private int _boomerangCount;
    public bool CanThrow { get { return _boomerangCount < _maxBoomerangCount; } }

    [Header("Boomerang Values")]
    [SerializeField] private float _damage;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _distance;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _waitTime;

    public bool inputKey { get; set; }

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
        inputKey = true;
        if (!CanThrow)
            return;

        Boomerang boomerang = Instantiate(_boomerangPrefab, _spawnPoint.position, _spawnPoint.rotation);
        boomerang.SetOnHitCallBack(OnHit);
        boomerang.InitParameters(_owner, _moveTime, _distance, _acceleration, _waitTime);
        _boomerangCount++;
    }

    public void EndAttack()
    {
        inputKey = false;
    }

    public void OnHit(Boomerang boomerang, Collider collider)
    {
        string otherTag = collider.gameObject.tag;

        switch (otherTag)
        {
            case "Player":
                Destroy(boomerang.gameObject);
                _boomerangCount--;
                break;
            case "Switch":
                boomerang.ReturnImmediately();
                Switch s = collider.gameObject.GetComponent<Switch>();
                s.ActivateSwitch();
                break;
            case "Enemy":
                boomerang.ReturnImmediately();
                Actor hit = collider.gameObject.GetComponent<Actor>();
                hit?.ApplyDamage(_damage);
                break;
            case "Pickup":
                Pickup pickup = collider.gameObject.GetComponent<Pickup>();
                pickup.DisableCollision();
                boomerang.grabbedItems.Add(pickup.transform);
                break;
            default:
                boomerang.ReturnImmediately();
                break;
        }
    }
}
