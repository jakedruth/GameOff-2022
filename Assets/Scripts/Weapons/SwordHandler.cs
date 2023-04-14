using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordHandler : ItemHandler
{
    public override string PrefabPath => "Prefabs/Sword";

    private readonly Sword _prefab;
    private Sword _instance;

    public SwordHandler(ItemController controller, PlayerController playerController, Animator animator) : base(controller, playerController, animator)
    {
        _prefab = Resources.Load<Sword>(PrefabPath);
    }

    public override void HandleMovement(ref Vector3 input, ref float speed)
    {
        if (InputKey)
            speed = 0;
    }

    public override void HandleAction(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                StartAction();
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                EndAction();
                break;
        }
    }

    protected override void StartAction()
    {
        InputKey = true;
        if (_instance != null)
            return;


        _animator.SetBool("Attack1", true);

        _instance = Object.Instantiate(_prefab, _itemController.spawnPoint, false);
        _instance.SetOnHitCallBack(OnHit);
    }

    protected override void EndAction()
    {
        InputKey = false;
        _animator.SetBool("Attack1", false);

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
            case "Prop":
            case "Enemy":
                Actor otherActor = collider.gameObject.GetComponent<Actor>();
                bool tookDamage = otherActor.ApplyDamage(_prefab.Damage);

                if (tookDamage && otherActor is Enemy enemy)
                    enemy.ApplyPushBack(_playerController.transform.forward, _prefab.PushBackDistance);

                break;
            default:
                break;
        }
    }
}
