using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : Actor
{
    // Components
    private Rigidbody _rigidBody;
    private CapsuleCollider _capsuleCollider;

    [Header("Attack values")]
    [SerializeField] private int _damage;
    [SerializeField] private float _pushBackDist;
    [SerializeField] private float _disablePlayerInputTime;


    [Header("Physics properties")]
    [SerializeField] private Vector3 _drag;
    private Vector3 _moveVel;
    private Vector3 _pushBackVel;

    void Reset()
    {
        tag = "Enemy";
    }

    public override void Awake()
    {
        base.Awake();
        _rigidBody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        // Apply Drag
        _pushBackVel.x /= 1 + _drag.x * Time.deltaTime;
        _pushBackVel.y /= 1 + _drag.y * Time.deltaTime;
        _pushBackVel.z /= 1 + _drag.z * Time.deltaTime;

        // Calculate the velocity and step this frame
        Vector3 velocity = _moveVel + _pushBackVel;
        Vector3 step = velocity * Time.deltaTime;
        Vector3 dir = velocity.normalized;

        // bound the step to the world
        const float skin = 0.05f;
        Vector3 center = _capsuleCollider.bounds.center;
        float distToCenter = (_capsuleCollider.height - _capsuleCollider.radius) * 0.5f;
        if (Physics.CapsuleCast(center + Vector3.up * distToCenter, center + Vector3.down * distToCenter, _capsuleCollider.radius - skin, dir, out RaycastHit hit))
        {
            float dist = hit.distance - skin;
            if (dist * dist <= step.sqrMagnitude)
            {
                step.Normalize();
                step *= hit.distance + skin;
            }
        }

        // Move the enemy
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

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController playerController = other.GetComponent<PlayerController>();

        Vector3 dir = playerController.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();

        if (playerController.actor.ApplyDamage(_damage))
        {
            playerController.ApplyPushBack(dir, _pushBackDist);
            playerController.TemporaryDisableInput(_disablePlayerInputTime);
        }
    }
}
