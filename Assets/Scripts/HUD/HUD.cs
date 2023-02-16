using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public HealthBar HealthBar { get; private set; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        HealthBar = GetComponentInChildren<HealthBar>();
    }
}
