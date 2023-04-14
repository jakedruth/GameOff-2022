using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemController : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator _animator;

    public Transform spawnPoint;
    private readonly ItemHandler[] items = new ItemHandler[3];

    protected void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            items[i] = null;
        }
    }

    public void InitReferences(PlayerController playerController, Animator animator)
    {
        _playerController = playerController;
        _animator = animator;
    }

    public void SetItem<T>(int index, params object[] args) where T : ItemHandler
    {
        if (args.Length == 0)
            items[index] = Activator.CreateInstance(typeof(T), this, _playerController, _animator) as ItemHandler;
        else
            items[index] = Activator.CreateInstance(typeof(T), this, _playerController, _animator, args) as ItemHandler;
    }

    public void HandleItemAction(int index, InputAction.CallbackContext context)
    {
        if (items[index] == null)
            return;

        items[index].HandleAction(context);
    }

    public void HandleMovement(ref Vector3 input, ref float speed)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;

            items[i].HandleMovement(ref input, ref speed);
        }
    }
}

public abstract class ItemHandler
{
    protected ItemController _itemController;
    protected PlayerController _playerController;
    protected Animator _animator;

    public abstract string PrefabPath { get; }
    public bool InputKey { get; protected set; }

    public ItemHandler(ItemController itemController, PlayerController playerController, Animator animator)
    {
        _itemController = itemController;
        _playerController = playerController;
        _animator = animator;
    }

    public virtual void HandleMovement(ref Vector3 input, ref float speed) { }
    public abstract void HandleAction(InputAction.CallbackContext context);
    protected abstract void StartAction();
    protected abstract void EndAction();
}
