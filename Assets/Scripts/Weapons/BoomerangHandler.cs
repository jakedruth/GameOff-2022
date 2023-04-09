using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangHandler : ItemHandler
{
    public override string PrefabPath => "Prefabs/Boomerang";

    private readonly Boomerang _prefab;

    private readonly int _maxBoomerangCount;
    private int _boomerangCount;
    public bool CanThrow { get { return _boomerangCount < _maxBoomerangCount; } }

    public BoomerangHandler(ItemController itemController, PlayerController playerController, Animator animator, params object[] args) : base(itemController, playerController, animator)
    {
        _prefab = Resources.Load<Boomerang>(PrefabPath);
        _maxBoomerangCount = (int)args[0];
    }

    public override void StartAction()
    {
        InputKey = true;
        if (!CanThrow)
            return;

        _playerController.TemporaryDisableInput(0.25f);
        _animator.SetTrigger("Attack2");

        Boomerang boomerang = Object.Instantiate(_prefab, _itemController.spawnPoint.position, _itemController.spawnPoint.rotation);
        boomerang.SetOnHitCallBack(OnHit);
        boomerang.InitParameters(_playerController);
        _boomerangCount++;
    }

    public override void EndAction()
    {
        InputKey = false;
    }

    public void OnHit(Boomerang boomerang, Collider collider)
    {
        string otherTag = collider.gameObject.tag;
        switch (otherTag)
        {
            case "Player":
                if (boomerang.phase == Boomerang.Phase.MOVE)
                    break;
                Object.Destroy(boomerang.gameObject);
                _boomerangCount--;
                break;
            case "Switch":
                boomerang.ReturnImmediately();
                Switch s = collider.gameObject.GetComponent<Switch>();
                s.ActivateSwitch();
                break;
            case "Pickup":
                Pickup pickup = collider.gameObject.GetComponent<Pickup>();
                pickup.DisableCollision();
                boomerang.GrabItem(pickup.transform);
                break;
            //case "Pot":
            case "Enemy":
                boomerang.ReturnImmediately();
                Actor hit = collider.gameObject.GetComponent<Actor>();
                hit?.ApplyDamage(_prefab.damage);
                break;
            default:
                boomerang.ReturnImmediately();
                break;
        }
    }
}
