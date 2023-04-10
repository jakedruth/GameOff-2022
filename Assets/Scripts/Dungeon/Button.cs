using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Components
    private MeshFilter meshFilter;

    [Header("Mesh Values")]
    [SerializeField] private Mesh onMesh;
    [SerializeField] private Mesh offMesh;

    public bool CurrentState { get; private set; }
    public UnityEngine.Events.UnityEvent onButtonDown;
    public ButtonStateChanged onButtonStateChanged;

    protected void Awake()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
    }

    public void SetState(bool value)
    {
        if (CurrentState == value)
            return;

        CurrentState = value;
        meshFilter.mesh = CurrentState ? onMesh : offMesh;

        onButtonStateChanged.Invoke(CurrentState);
        if (CurrentState)
            onButtonDown.Invoke();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SetState(true);
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SetState(false);
    }
}

[System.Serializable] public class ButtonStateChanged : UnityEngine.Events.UnityEvent<bool> { }
