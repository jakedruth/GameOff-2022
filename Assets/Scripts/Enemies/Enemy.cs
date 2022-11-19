using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    // Components
    public Actor actor { get; private set; }
    private Rigidbody _rigidBody;
    [SerializeField] private Vector3 _drag;
    private Vector3 _moveVel;
    private Vector3 _pushBackVel;

    void Reset()
    {
        tag = "Enemy";
    }

    void Awake()
    {
        actor = GetComponent<Actor>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Apply Drag
        _pushBackVel.x /= 1 + _drag.x * Time.deltaTime;
        _pushBackVel.y /= 1 + _drag.y * Time.deltaTime;
        _pushBackVel.z /= 1 + _drag.z * Time.deltaTime;

        Vector3 step = (_moveVel + _pushBackVel) * Time.deltaTime;

        // TODO: Check if the enemy can move first
        // Need to raycast based on the velocity to see if the enemy can move in that direction
        // Currently the push back (potentially) sends the enemy through walls

        _rigidBody.MovePosition(transform.position + step);
    }

    public void SetVelocity(Vector3 vel)
    {
        _moveVel = vel;
    }

    public void ApplyPushBack(Vector3 direction, float distance)
    {

        // Fancy math to apply the right amount of force
        Vector3 pushMagnitude = distance * new Vector3(
            Mathf.Log(1f / (Time.deltaTime * _drag.x + 1)) / -Time.deltaTime, 0,
            Mathf.Log(1f / (Time.deltaTime * _drag.z + 1)) / -Time.deltaTime);
        _pushBackVel += Vector3.Scale(direction, pushMagnitude);
    }
}
