using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    private Action<Collision> onHitCallBack;

    public enum Phase
    {
        MOVE,
        WAIT,
        RETURN,
    }

    private Actor _owner;
    private Phase phase;
    private float _timer;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _yOffset;

    private float _moveTime;
    private float _speed;
    private float _acceleration;
    private float _waitTime;

    public void InitParameters(Actor actor, float moveTime, float distance, float acceleration, float waitTime)
    {
        _owner = actor;
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
                Vector3 dir = _owner.transform.position - transform.position;
                dir.y += _yOffset;
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
    }

    public void ReturnImmediately()
    {
        SetPhase(Phase.RETURN);
        Vector3 dir = _owner.transform.position - transform.position;
        dir.y += _yOffset;
        dir.Normalize();
        _velocity = dir * _speed;
    }

    public void SetPhase(Phase nextPhase)
    {
        phase = nextPhase;
        _timer = 0;

        // Get collider with the owner and self
        Collider c1 = GetComponent<Collider>();
        Collider c2 = _owner.GetComponent<Collider>();

        // Ignore collisions during the first phase
        Physics.IgnoreCollision(c1, c2, phase == Phase.MOVE);

    }

    public void SetOnHitCallBack(Action<Collision> callback)
    {
        onHitCallBack = callback;
    }

    public void OnCollisionEnter(Collision other)
    {
        onHitCallBack(other);
    }
}

