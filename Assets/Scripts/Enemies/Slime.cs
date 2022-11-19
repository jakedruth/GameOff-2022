using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDR.Utils;

[RequireComponent(typeof(Actor))]
public class Slime : MonoBehaviour
{
    public enum AI_State
    {
        WAIT,
        MOVE
    }

    // Components
    public Actor actor { get; private set; }

    // TODO: Switch to rigidbody that is kinematic
    private CharacterController _characterController;

    private AI_State _currentState;
    [Header("Movement Values")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private RangedFloat _moveDistance;
    [SerializeField] private RangedFloat _waitTime;
    private Vector3 _hVelocity;
    private Vector3 _vVelocity;
    private bool _isGrounded;

    private float _timer;
    private Vector3 _facing;

    void Awake()
    {
        // Get components
        actor = GetComponent<Actor>();
        _characterController = GetComponent<CharacterController>();

        Init();
    }

    private void Init()
    {
        _facing = Vector3.forward;
        _timer = _waitTime.GetRandomValue();
        _currentState = AI_State.WAIT;
    }

    void Update()
    {
        // TODO: Enemy needs to deal damage to the player on contact

        // Update vertical velocity
        _isGrounded = (_characterController.collisionFlags & CollisionFlags.Below) != 0;
        if (_isGrounded)
            _vVelocity.y = 0;

        _vVelocity += Physics.gravity * Time.deltaTime;

        _timer -= Time.deltaTime;
        switch (_currentState)
        {
            case AI_State.WAIT:
                if (_timer <= 0)
                {
                    _currentState = AI_State.MOVE;
                    CalculateNewTargetPoint();
                }
                break;
            case AI_State.MOVE:
                if (_timer <= 0)
                {
                    _currentState = AI_State.WAIT;
                    _timer = _waitTime.GetRandomValue();
                    _hVelocity = Vector3.zero;
                }
                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {
        if (_currentState == AI_State.MOVE)
        {
            _characterController.Move((_vVelocity + _hVelocity) * Time.deltaTime);
        }
    }

    private static readonly Vector3[] facings = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    void CalculateNewTargetPoint()
    {
        int i = Random.Range(0, 4);
        _facing = facings[i];
        _hVelocity = _facing * _maxSpeed;
        int dist = Mathf.RoundToInt(_moveDistance.GetRandomValue());
        //transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

        // TODO: Check if the new target position is in the bounds

        _timer = dist / _maxSpeed;
    }
}
