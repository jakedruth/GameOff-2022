using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHandler : ItemHandler
{
    public override string PrefabPath => "Prefabs/Sword";

    private readonly Sword _prefab;
    private Sword _instance;

    [SerializeField] private int _damage = 5;
    [SerializeField] private float _pushBackDist = 3;

    public SwordHandler(ItemController controller, Actor actor, Animator animator) : base(controller, actor, animator)
    {
        _prefab = Resources.Load<Sword>(PrefabPath);
    }

    public override void HandleMovement(ref Vector3 input, ref float speed)
    {
        if (InputKey)
            speed = 0;
    }

    public override void StartAction()
    {
        InputKey = true;
        if (_instance != null)
            return;

        _instance = Object.Instantiate(_prefab, _controller.spawnPoint, false);
        _instance.SetOnHitCallBack(OnHit);
    }

    public override void EndAction()
    {
        InputKey = false;

        Object.Destroy(_instance.gameObject);
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
                    enemy.ApplyPushBack(actor.transform.forward, _pushBackDist);

                break;
            default:
                break;
        }
    }
}
