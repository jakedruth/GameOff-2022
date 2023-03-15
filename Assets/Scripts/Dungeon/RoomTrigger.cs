using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private bool _triggerRoomOnce;
    private bool _didOnce;
    private bool _roomTriggerIsActive;
    [SerializeField] private float _disablePlayerInputTime;
    public UnityEngine.Events.UnityEvent onBeginTriggerRoom;
    public UnityEngine.Events.UnityEvent onEndTriggerRoom;

    protected void Reset()
    {
        BoxCollider trigger = GetComponent<BoxCollider>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<BoxCollider>();
        }

        trigger.isTrigger = true;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        BeginTriggerRoom();
    }

    public void BeginTriggerRoom()
    {
        if (_roomTriggerIsActive || (_triggerRoomOnce && _didOnce))
            return;

        _roomTriggerIsActive = true;
        PlayerController.instance.TemporaryDisableInput(_disablePlayerInputTime);
        onBeginTriggerRoom.Invoke();
    }

    public void EndTriggerRoom()
    {
        if (!_roomTriggerIsActive || (_triggerRoomOnce && _didOnce))
            return;

        _roomTriggerIsActive = false;
        _didOnce = true;

        PlayerController.instance.TemporaryDisableInput(_disablePlayerInputTime);
        onEndTriggerRoom.Invoke();
    }
}
