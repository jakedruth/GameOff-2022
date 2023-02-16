using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public HealthBar HealthBar { get; private set; }
    public KeyCounter KeyCounter { get; private set; }

    protected void Awake()
    {
        if (instance == null)
            instance = this;

        HealthBar = GetComponentInChildren<HealthBar>();
        KeyCounter = GetComponentInChildren<KeyCounter>();
    }
}
