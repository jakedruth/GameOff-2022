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
    private CharacterController _characterController;


    private AI_State _currentState;
    [Header("Movement Values")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private RangedFloat _moveDistance;
    [SerializeField] private RangedFloat _waitTime;
    private float _timer;
    private Vector3 _facing;
    private Vector3 _targetPos;

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
        switch (_currentState)
        {
            case AI_State.WAIT:
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _currentState = AI_State.MOVE;
                    CalculateRandomTargetPoint();
                }
                break;
            case AI_State.MOVE:

                Vector3 pos = Vector3.MoveTowards(transform.position, _targetPos, _maxSpeed * Time.deltaTime);
                transform.position = pos;

                // Use the character controller here

                Vector3 delta = _targetPos - pos;
                if (delta.magnitude <= 0.0001f)
                {
                    transform.position = _targetPos;
                    _currentState = AI_State.WAIT;
                    _timer = _waitTime.GetRandomValue();
                }
                break;
            default:
                break;
        }
    }

    private static readonly Vector3[] facings = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    void CalculateRandomTargetPoint()
    {
        int i = Random.Range(0, 4);
        _facing = facings[i];
        int dist = Mathf.RoundToInt(_moveDistance.GetRandomValue());

        _targetPos = transform.position + _facing * dist;

        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);
    }
}
