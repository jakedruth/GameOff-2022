using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    //TODO: Move Sword stats to here from handler
    // Stats like damage and push back

    private Action<Collider> onHitCallBack;

    public void SetOnHitCallBack(Action<Collider> callback)
    {
        onHitCallBack = callback;
    }

    public void OnTriggerEnter(Collider other)
    {
        onHitCallBack(other);
    }
}
