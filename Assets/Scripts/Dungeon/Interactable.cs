using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public const string InteractLayerName = "Interactable";
    [SerializeField] private bool _isInteractable = true;

    public bool IsInteractable { get { return _isInteractable; } protected set { _isInteractable = value; } }

    public bool TryInteract(InteractController controller)
    {
        if (!IsInteractable)
            return false;

        bool success = CanInteract(controller);
        if (success)
            Interact(controller);

        return success;
    }
    protected abstract bool CanInteract(InteractController controller);
    protected abstract void Interact(InteractController controller);

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
