using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lock : MonoBehaviour
{
    // Components
    private Transform _prompt;
    private TMPro.TMP_Text _promptText;

    // Values
    [SerializeField] private bool _requireKey;
    private bool _displayPrompt;

    // Events
    public UnityEngine.Events.UnityEvent onUnlock;

    void Awake()
    {
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
        Debug.Log(input.currentControlScheme);
        DeviceDisplaySettings_SO displaySetting = DisplayManager.instance.FindDisplaySetting(input.currentControlScheme);
        _promptText.text = $"Press {displaySetting.interactTag}";
    }

    public void Unlock()
    {
        if (_requireKey)
        {
            if (PlayerController.instance.KeyCount == 0)
                return;

            PlayerController.instance.KeyCount--;
            Debug.Log($"Using key. New key count: {PlayerController.instance.KeyCount}");
        }

        onUnlock.Invoke();
        SetDisplayPrompt(false);
        Destroy(this);
    }

    private void SetDisplayPrompt(bool value)
    {
        _displayPrompt = value;
        _prompt.localScale = Vector3.one * (_displayPrompt ? 1f : 0);
    }

    void OnDestroy()
    {
        PlayerController.instance?.OnInteractEvent.RemoveListener(Unlock);
        SetDisplayPrompt(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        other.GetComponent<PlayerController>().OnInteractEvent.AddListener(Unlock);
        SetDisplayPrompt(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        other.GetComponent<PlayerController>().OnInteractEvent.RemoveListener(Unlock);
        SetDisplayPrompt(false);
    }
}
