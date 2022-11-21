using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour
{
    // Components
    public Actor actor { get; private set; }
    private Rigidbody _rigidBody;
    private CapsuleCollider _capsuleCollider;

    [Header("Physics properties")]
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
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        // Apply Drag
        _pushBackVel.x /= 1 + _drag.x * Time.deltaTime;
        _pushBackVel.y /= 1 + _drag.y * Time.deltaTime;
        _pushBackVel.z /= 1 + _drag.z * Time.deltaTime;

        Vector3 velocity = _moveVel + _pushBackVel;
        Vector3 step = (velocity) * Time.deltaTime;
        Vector3 dir = velocity.normalized;

        // TODO: Check if the enemy can move first
        // Need to raycast based on the velocity to see if the enemy can move in that direction
        // Currently the push back (potentially) sends the enemy through walls

        const float skin = 0.05f;
        //Ray ray = new(_capsuleCollider.bounds.center + dir * (_capsuleCollider.radius), dir);

        Vector3 center = _capsuleCollider.bounds.center;
        float distToCenter = (_capsuleCollider.height - _capsuleCollider.radius) * 0.5f;
        if (Physics.CapsuleCast(center + Vector3.up * distToCenter, center + Vector3.down * distToCenter, _capsuleCollider.radius - skin, dir, out RaycastHit hit))
        //if (Physics.Raycast(ray, out RaycastHit hit))
        {
            float dist = hit.distance - skin;
            if (dist * dist <= step.sqrMagnitude)
            {
                step.Normalize();
                step *= (hit.distance + skin);
            }
        }

        _rigidBody.MovePosition(transform.position + step);

        //_rigidBody.AddForce((_moveVel + _pushBackVel) * Time.deltaTime, ForceMode.VelocityChange);
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
