using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Door))]
public class Lock : Interactable
{
    // Components
    private Door _door;
    private Transform _prompt;
    private TMPro.TMP_Text _promptText;

    // Values
    [SerializeField] private bool _requireKey;
    private bool _displayPrompt;

    // Events
    public UnityEngine.Events.UnityEvent onUnlock;

    void Awake()
    {
        _door = GetComponent<Door>();
        _prompt = transform.GetChild(1);
        _promptText = _prompt.GetComponentInChildren<TMPro.TMP_Text>();
    }

    void Start()
    {
        PlayerController.instance.ControlsChangedEvent.AddListener(UpdateText);

        _prompt.rotation = Camera.main.transform.rotation;
        SetDisplayPrompt(false);
        UpdateText(PlayerController.instance.CurrentPlayerInput);
    }

    private void UpdateText(PlayerInput input)
    {
        DeviceDisplaySettings_SO displaySetting = DisplayManager.instance.FindDisplaySetting(input.currentControlScheme);
        _promptText.text = $"Press {displaySetting.interactTag}";
    }

    private void SetDisplayPrompt(bool value)
    {
        _displayPrompt = value;
        _prompt.localScale = Vector3.one * (_displayPrompt ? 1f : 0);
    }

    void OnDestroy()
    {
        //PlayerController.instance?.OnInteractEvent.RemoveListener(Unlock);
        SetDisplayPrompt(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        SetDisplayPrompt(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        SetDisplayPrompt(false);
    }

    protected override bool CanInteract(InteractController controller)
    {
        if (_requireKey)
        {
            int keyCount = PlayerController.instance.inventory.key.Get();
            if (keyCount == 0)
                return false;

            PlayerController.instance.inventory.key.Set(keyCount - 1);
            Debug.Log($"Using key. New key count: {PlayerController.instance.inventory.key.Get()}");
        }

        return true;
    }

    protected override void Interact(InteractController controller)
    {
        PlayerController.instance.TemporaryDisableInput(_door.GetAnimateDoorTime());
        _door.SetDoorState(true);
        onUnlock.Invoke();
        SetDisplayPrompt(false);
        Destroy(this);
    }
}
