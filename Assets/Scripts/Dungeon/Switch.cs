using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool IsTriggered { get; private set; }
    [SerializeField] private SwapMaterial _swapMaterial;
    [SerializeField] private GameObject _light;
    [SerializeField] private FloatingGameObject _orb;

    public void HandleSwitchOn()
    {
        _swapMaterial?.Swap();
        _light?.SetActive(true);
        _orb?.SetIsOn(true);
    }

    public UnityEngine.Events.UnityEvent onActivate;

    [ContextMenu("activate Switch")]
    public void ActivateSwitch()
    {
        if (!IsTriggered)
        {
            IsTriggered = true;
            onActivate.Invoke();
            HandleSwitchOn();
        }
    }
}
