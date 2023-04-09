using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Action<Collider> onHitCallBack;

    [Header("Value")]
    [SerializeField] private int _damage;
    public int Damage { get { return _damage; } }
    [SerializeField] private float _pushBackDistance;
    public float PushBackDistance { get { return _pushBackDistance; } }


    public void SetOnHitCallBack(Action<Collider> callback)
    {
        onHitCallBack = callback;
    }

    public void OnTriggerEnter(Collider other)
    {
        onHitCallBack(other);
    }
}
