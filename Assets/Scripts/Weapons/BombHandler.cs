using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombHandler : ItemHandler
{
    public override string PrefabPath => "Prefabs/Objects/Bomb";

    private readonly Bomb _prefab;
    private Bomb _instance;

    public BombHandler(ItemController itemController, PlayerController playerController, Animator animator) : base(itemController, playerController, animator)
    {
        _prefab = Resources.Load<Bomb>(PrefabPath);
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

        int bombCount = _playerController.inventory.bombCount.Get();
        if (bombCount <= 0)
            return;

        Vector3 point = _itemController.transform.TransformPoint(Vector3.forward * 1.2f);
        _instance = Object.Instantiate(_prefab, point, _itemController.spawnPoint.rotation);
        _instance.OnExplode.AddListener(OnInstanceExplode);
    }

    protected override void EndAction()
    {
        InputKey = false;
    }

    private void OnInstanceExplode()
    {
        _instance = null;
    }
}
