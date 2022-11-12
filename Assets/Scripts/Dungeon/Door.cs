using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Base Door Properties")]
    [SerializeField] private float _animateDoorTime;
    [SerializeField] private float _moveDistance;
    public bool isOpen { get; private set; }
    public UnityEngine.Events.UnityEvent onDoorOpened;

    public void OpenDoor()
    {
        if (!isOpen)
            HandleOpenDoor();
    }

    private void HandleOpenDoor()
    {
        isOpen = true;
        onDoorOpened.Invoke();
        StartCoroutine(AnimateOpenDoor());
    }

    private IEnumerator AnimateOpenDoor()
    {
        isOpen = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down * _moveDistance;
        float timer = 0;

        while (timer < _animateDoorTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _animateDoorTime);

            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }
    }
}
