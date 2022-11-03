using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Action<Collision> onHitCallBack;

    public void SetOnHitCallBack(Action<Collision> callback)
    {
        onHitCallBack = callback;
    }

    public void OnCollisionEnter(Collision other)
    {
        onHitCallBack(other);
    }
}
