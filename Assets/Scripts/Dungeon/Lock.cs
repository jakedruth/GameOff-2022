using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private bool _requireKey;

    public UnityEngine.Events.UnityEvent onUnlock;

    public void Unlock()
    {
        Debug.Log("Attempting Unlock");
        if (_requireKey)
        {
            if (PlayerController.instance.keys == 0)
                return;

            PlayerController.instance.keys--;
            Debug.Log($"Using key. New key count: {PlayerController.instance.keys}");
        }

        onUnlock.Invoke();
        Destroy(this);
    }

    void OnDestroy()
    {
        PlayerController.instance.onInteractEvent.RemoveListener(Unlock);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Lock on Enter");
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>().onInteractEvent.AddListener(Unlock);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Lock on Exit");
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>().onInteractEvent.RemoveListener(Unlock);
    }
}
