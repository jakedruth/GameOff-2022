using System;
using UnityEngine;

[Serializable]
// Requires Unity 2020.1+
public struct Optional<T>
{
    [SerializeField] private bool enabled;
    [SerializeField] private T data;

    public bool Enabled => enabled;
    public T Data => data;

    public Optional(T initialValue)
    {
        enabled = true;
        data = initialValue;
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public void SetData(T data)
    {
        this.data = data;
    }
}