using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private Actor _actor;
    private Animator _animator;

    public Transform spawnPoint;
    private readonly ItemHandler[] items = new ItemHandler[3];

    public void InitReferences(Actor actor, Animator animator)
    {
        _actor = actor;
        _animator = animator;
    }

    public void SetItem<T>(int index) where T : ItemHandler
    {
        items[index] = Activator.CreateInstance(typeof(T), this, _actor, _animator) as ItemHandler;
    }

    public void ItemActionStarted(int index)
    {
        items[index].StartAction();
    }

    public void ItemActionEnded(int index)
    {
        items[index].EndAction();
    }

    public void HandleMovement(ref Vector3 input, ref float speed)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i]?.HandleMovement(ref input, ref speed);
        }
    }
}

public abstract class ItemHandler
{
    protected Actor _owner;
    protected Animator _animator;
    protected ItemController _controller;

    public abstract string PrefabPath { get; }
    public bool InputKey { get; protected set; }

    public ItemHandler(ItemController controller, Actor actor, Animator animator)
    {
        _controller = controller;
        _owner = actor;
        _animator = animator;
    }

    public abstract void HandleMovement(ref Vector3 input, ref float speed);
    public abstract void StartAction();
    public abstract void EndAction();
}
