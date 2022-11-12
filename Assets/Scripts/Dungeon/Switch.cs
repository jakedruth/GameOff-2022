using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool isTriggered { get; private set; }
    public UnityEngine.Events.UnityEvent onActivate;

    [ContextMenu("activate Switch")]
    public void ActivateSwitch()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            onActivate.Invoke();
        }
    }
}
