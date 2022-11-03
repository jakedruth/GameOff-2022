using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Action<Collider> onHitCallBack;

    public void SetOnHitCallBack(Action<Collider> callback)
    {
        onHitCallBack = callback;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other);
    }

    public void OnTriggerEnter(Collider other)
    {
        onHitCallBack(other);
    }
}
