using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    //TODO: Move Boomerang stats to here from handler
    // Stats like damage, move time, distance, acceleration, wait time

    private Action<Boomerang, Collider> onHitCallBack;

    public enum Phase
    {
        MOVE,
        WAIT,
        RETURN,
    }

    public Phase phase { get; private set; }
    private float _timer;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _yOffset;

    private float _moveTime;
    private float _speed;
    private float _acceleration;
    private float _waitTime;

    public List<Transform> grabbedItems;

    public void InitParameters(Actor actor, float moveTime, float distance, float acceleration, float waitTime)
    {
        SetPhase(Phase.MOVE);

        _direction = transform.forward;
        _yOffset = transform.position.y;

        _moveTime = moveTime;
        _speed = distance / moveTime;
        _acceleration = acceleration;
        _waitTime = waitTime;

        _velocity = _direction * _speed;
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        switch (phase)
        {
            case Phase.MOVE:
                if (_timer > _moveTime)
                {
                    SetPhase(Phase.WAIT);
                }
                break;
            case Phase.WAIT:
                _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _acceleration * Time.deltaTime);

                if (_timer > _waitTime)
                {
                    // Set the Phase
                    SetPhase(Phase.RETURN);
                }
                break;
            case Phase.RETURN:
                Vector3 dir = PlayerController.instance.GetCenter() - transform.position;
                dir.Normalize();
                Vector3 targetVel = dir * _speed;

                _velocity = Vector3.MoveTowards(_velocity, targetVel, _acceleration * Time.deltaTime);

                if (_timer > 10)
                    Destroy(gameObject);

                break;
            default:
                break;
        }

        transform.position += _velocity * Time.deltaTime;

        if (grabbedItems.Count > 0)
        {
            for (int i = grabbedItems.Count - 1; i >= 0; i--)
            {
                Transform item = grabbedItems[i];
                if (item == null)
                {
                    grabbedItems.RemoveAt(i);
                    continue;
                }

                item.position = transform.position;
            }
        }
    }

    public void ReturnImmediately()
    {
        SetPhase(Phase.RETURN);
        Vector3 dir = PlayerController.instance.GetCenter() - transform.position;
        dir.Normalize();
        _velocity = dir * _speed;
    }

    public void SetPhase(Phase nextPhase)
    {
        phase = nextPhase;
        _timer = 0;

        // Get collider with the owner and self
        Collider c1 = GetComponent<Collider>();
        Collider c2 = PlayerController.instance.GetComponent<Collider>();

        // Ignore collisions during the first phase
        Physics.IgnoreCollision(c1, c2, phase == Phase.MOVE);
    }

    public void SetOnHitCallBack(Action<Boomerang, Collider> callback)
    {
        onHitCallBack = callback;
    }

    public void OnTriggerEnter(Collider other)
    {
        onHitCallBack(this, other);
    }
}


