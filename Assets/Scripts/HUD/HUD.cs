using System.ComponentModel;
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
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        HealthBar = GetComponentInChildren<HealthBar>();
        KeyCounter = GetComponentInChildren<KeyCounter>();
    }

    void Start()
    {
        // Health Bar
        HealthBar.SetMaxHeatCount(Mathf.RoundToInt(PlayerController.instance.actor.maxHP));
        HealthBar.SetHealth(Mathf.RoundToInt(PlayerController.instance.actor.CurrentHP));

        // Key Counter
        KeyCounter.SetKeyCount(PlayerController.instance.inventory.key.Get());

        InitListeners();
    }

    private void InitListeners()
    {
        PlayerController.instance.actor.OnTakeDamage.AddListener(HandleHealthChanged);
        PlayerController.instance.inventory.key.OnSlotUpdated.AddListener(HandleKeyCountUpdated);
    }

    private void HandleHealthChanged()
    {
        HealthBar.SetHealth(Mathf.RoundToInt(PlayerController.instance.actor.CurrentHP));
    }

    private void HandleKeyCountUpdated(int newValue)
    {
        KeyCounter.SetKeyCount(newValue);
    }
}
