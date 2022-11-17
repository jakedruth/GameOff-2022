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

    void Awake()
    {
        Rigidbody rb = transform.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
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
