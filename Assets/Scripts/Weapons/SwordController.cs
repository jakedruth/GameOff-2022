using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Actor _owner;
    private Sword _swordPrefab;

    [SerializeField] private Transform _spawnPoint;
    private Sword _instance;

    [Header("Sword Value")]
    [SerializeField] private int _damage;
    [SerializeField] private float _pushBackDist;

    public bool inputKey { get; set; }

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
        inputKey = true;
        if (_instance != null)
            return;

        _instance = Instantiate(_swordPrefab, _spawnPoint, false);
        _instance.SetOnHitCallBack(OnHit);

        Collider c1 = GetComponent<Collider>();
        Collider c2 = _instance.GetComponent<Collider>();

        Physics.IgnoreCollision(c1, c2, true);
    }

    public void EndAttack()
    {
        inputKey = false;

        Destroy(_instance.gameObject);
        _instance = null;
    }

    public void OnHit(Collider collider)
    {
        string otherTag = collider.gameObject.tag;

        switch (otherTag)
        {
            case "Switch":
                Switch s = collider.gameObject.GetComponent<Switch>();
                s.ActivateSwitch();
                break;
            case "Pot":
            case "Enemy":
                Actor actor = collider.gameObject.GetComponent<Actor>();
                bool tookDamage = actor.ApplyDamage(_damage);

                if (tookDamage && actor is Enemy enemy)
                    enemy.ApplyPushBack(transform.forward, _pushBackDist);

                break;
            default:
                break;
        }


    }
}
