using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private bool _triggerRoomOnce;
    private bool _roomTriggerIsActive;
    [SerializeField] private float _disablePlayerInputTime;
    public UnityEngine.Events.UnityEvent onBeginTriggerRoom;
    public UnityEngine.Events.UnityEvent onEndTriggerRoom;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        BeginTriggerRoom();
    }

    public void BeginTriggerRoom()
    {
        if (_roomTriggerIsActive)
            return;

        _roomTriggerIsActive = true;
        StartCoroutine(DisableInput(_disablePlayerInputTime));
        onBeginTriggerRoom.Invoke();
    }

    public void EndTriggerRoom()
    {
        if (!_roomTriggerIsActive)
            return;

        if (_triggerRoomOnce)
            _roomTriggerIsActive = false;

        StartCoroutine(DisableInput(_disablePlayerInputTime));
        onEndTriggerRoom.Invoke();
    }

    private IEnumerator DisableInput(float timer)
    {
        PlayerController.instance.SetPlayerInputMap("Cutscene");

        yield return new WaitForSeconds(timer);

        PlayerController.instance.SetPlayerInputMap("Player");
    }
}
