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
    private Vector3 _closedPos;
    private Vector3 _openPos;

    void Awake()
    {
        _doorCollider = transform.GetChild(0);
        _closedPos = _doorCollider.localPosition;
        _openPos = _closedPos + Vector3.down * _moveDistance;

        if (_isOpen)
            _doorCollider.localPosition = _openPos;
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
        float timer = 0;

        while (timer < _animateDoorTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / _animateDoorTime);
            float k = _isOpen ? 1 - t : t;

            _doorCollider.localPosition = Vector3.Lerp(_openPos, _closedPos, k);

            yield return null;
        }
    }
}



