using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
abstract public class Pickup : MonoBehaviour
{
    public abstract void OnPlayerEnter(PlayerController pc);

    void Reset()
    {
        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.center.Set(0, 1, 0);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter(other.GetComponent<PlayerController>());
            Destroy(gameObject);
        }
    }
}
