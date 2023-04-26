using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public HUDHealthBar HealthBar { get; private set; }
    public HUDKeyCounter KeyCounter { get; private set; }

    protected void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        HealthBar = GetComponentInChildren<HUDHealthBar>();
        KeyCounter = GetComponentInChildren<HUDKeyCounter>();
    }

    void Start()
    {
        // Health Bar
        HealthBar.SetMaxHeatCount(Mathf.RoundToInt(PlayerController.instance.Actor.maxHP));
        HealthBar.SetHealth(Mathf.RoundToInt(PlayerController.instance.Actor.CurrentHP));

        // Key Counter
        KeyCounter.SetKeyCount(PlayerController.instance.inventory.keyCount.Get());

        InitListeners();
    }

    private void InitListeners()
    {
        PlayerController.instance.Actor.OnTakeDamage.AddListener(HandleHealthChanged);
        PlayerController.instance.inventory.keyCount.OnSlotUpdated.AddListener(HandleKeyCountUpdated);
    }

    private void HandleHealthChanged()
    {
        HealthBar.SetHealth(Mathf.RoundToInt(PlayerController.instance.Actor.CurrentHP));
    }

    private void HandleKeyCountUpdated(int newValue)
    {
        KeyCounter.SetKeyCount(newValue);
    }
}
