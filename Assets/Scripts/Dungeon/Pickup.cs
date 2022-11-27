using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
abstract public class Pickup : MonoBehaviour
{
    protected Rigidbody _rb;
    public UnityEngine.Events.UnityEvent OnPickedUp;

    public abstract void handleOnPlayerEnter(PlayerController pc);

    void Reset()
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.center.Set(0, 1, 0);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        _rb?.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickedUp.Invoke();
            handleOnPlayerEnter(other.GetComponent<PlayerController>());
            Destroy(gameObject);
        }
    }

    public void DisableCollision()
    {
        _rb.isKinematic = true;
    }

    void OnDestroy()
    {
        OnPickedUp.RemoveAllListeners();
    }
}
