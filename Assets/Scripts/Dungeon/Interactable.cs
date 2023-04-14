using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public const string InteractLayerName = "Interactable";
    [SerializeField] protected bool _isInteractable = true;
    public bool IsInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

    public abstract bool TryInteract();
    protected abstract void Interact();

    protected void Reset()
    {
        _isInteractable = true;
        SetLayerRecursive(transform);
    }

    private void SetLayerRecursive(Transform parent)
    {
        parent.gameObject.layer = LayerMask.NameToLayer(InteractLayerName);
        foreach (Transform child in parent)
        {
            SetLayerRecursive(child);
        }
    }

    public bool ToggleIsInteractable()
    {
        return _isInteractable = !_isInteractable;
    }
}
