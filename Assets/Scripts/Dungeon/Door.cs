using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform _doorCollider;

    [Header("Door Properties")]
    [SerializeField] private bool _isOpen;
    [SerializeField] private float _animateDoorTime;
    [SerializeField] private float _moveDistance;
    public UnityEngine.Events.UnityEvent onDoorOpened;

    void Awake()
    {
        _doorCollider = transform.GetChild(0);
        if (_isOpen)
            _doorCollider.localPosition = Vector3.down * _moveDistance;
    }

    public void ToggleDoorState()
    {
        SetDoorState(!_isOpen);
    }

    public void SetDoorState(bool isOpen)
    {
        if (_isOpen == isOpen)
            return;

        _isOpen = isOpen;
        onDoorOpened.Invoke();
        StartCoroutine(AnimateDoor());
    }

    private IEnumerator AnimateDoor()
    {
        Vector3 closedPos = Vector3.zero;
        Vector3 openPos = Vector3.down * _moveDistance;

        Vector3 startPos = _isOpen ? closedPos : openPos;
        Vector3 endPos = _isOpen ? openPos : closedPos;
        float timer = 0;

        while (timer < _animateDoorTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _animateDoorTime);

            _doorCollider.localPosition = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }
    }
}



