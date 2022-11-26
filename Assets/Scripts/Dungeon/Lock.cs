using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private bool _requireKey;

    public UnityEngine.Events.UnityEvent onUnlock;

    public void Unlock()
    {
        if (_requireKey)
        {
            if (PlayerController.instance.KeyCount == 0)
                return;

            PlayerController.instance.KeyCount--;
            Debug.Log($"Using key. New key count: {PlayerController.instance.KeyCount}");
        }

        onUnlock.Invoke();
        Destroy(this);
    }

    void OnDestroy()
    {
        PlayerController.instance?.OnInteractEvent.RemoveListener(Unlock);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>().OnInteractEvent.AddListener(Unlock);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerController>().OnInteractEvent.RemoveListener(Unlock);
    }
}
