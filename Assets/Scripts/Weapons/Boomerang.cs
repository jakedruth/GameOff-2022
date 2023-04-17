using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    private Action<Boomerang, Collider> onHitCallBack;

    public enum Phase { MOVE, WAIT, RETURN, }
    public Phase phase { get; private set; }

    public int damage;
    public float moveTime;
    public float distance;
    public float acceleration;
    public float waitTime;

    private PlayerController _owner;
    private float _speed;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _timer;

    private List<Transform> _grabbedItems;

    public void InitParameters(PlayerController pc)
    {
        _owner = pc;
        _grabbedItems = new List<Transform>();

        SetPhase(Phase.MOVE);
        _direction = transform.forward;
        _speed = distance / moveTime;
        _velocity = _direction * _speed;
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        switch (phase)
        {
            case Phase.MOVE:
                if (_timer > moveTime)
                {
                    SetPhase(Phase.WAIT);
                }
                break;
            case Phase.WAIT:
                _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, acceleration * Time.deltaTime);

                if (_timer > waitTime)
                {
                    // Set the Phase
                    SetPhase(Phase.RETURN);
                }
                break;
            case Phase.RETURN:
                Vector3 dir = _owner.GetCenter() - transform.position;
                dir.Normalize();
                Vector3 targetVel = dir * _speed;

                _velocity = Vector3.MoveTowards(_velocity, targetVel, acceleration * Time.deltaTime);

                if (_timer > 10) // Could not reach the player some how, just delete the boomerang
                    Destroy(gameObject);

                break;
            default:
                break;
        }

        transform.position += _velocity * Time.deltaTime;

        if (_grabbedItems.Count > 0)
        {
            for (int i = _grabbedItems.Count - 1; i >= 0; i--)
            {
                Transform item = _grabbedItems[i];
                if (item == null)
                {
                    _grabbedItems.RemoveAt(i);
                    continue;
                }

                item.position = transform.position;
            }
        }
    }

    public void ReturnImmediately()
    {
        SetPhase(Phase.RETURN);
        Vector3 dir = _owner.GetCenter() - transform.position;
        dir.Normalize();
        _velocity = dir * _speed;
    }

    public void GrabItem(Transform newItem)
    {
        _grabbedItems.Add(newItem);
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

    public void SetOnHitCallBack(Action<Boomerang, Collider> callback)
    {
        onHitCallBack = callback;
    }

    public void OnTriggerEnter(Collider other)
    {
        onHitCallBack(this, other);
    }

    internal void DropPickupItems()
    {
        for (int i = _grabbedItems.Count - 1; i >= 0; i--)
        {
            Pickup pickup = _grabbedItems[i].GetComponent<Pickup>();
            pickup?.EnableCollision();
        }
    }
}


