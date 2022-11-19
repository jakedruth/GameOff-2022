using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDR.Utils;

[System.Serializable]
public enum AI_State : int
{
    WAIT = 0,
    MOVE
}

[RequireComponent(typeof(Enemy))]
public class Slime : MonoBehaviour
{
    // Components
    public Enemy enemy { get; private set; }

    private AI_State _currentState;
    [Header("Movement Values")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private RangedFloat _moveDistance;
    [SerializeField] private RangedFloat _waitTime;

    private float _timer;
    private Vector3 _facing;

    void Awake()
    {
        // Get components
        enemy = GetComponent<Enemy>();

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
        _timer -= Time.deltaTime;
        switch (_currentState)
        {
            case AI_State.WAIT:
                if (_timer <= 0)
                {
                    SetState(1);
                }
                break;
            case AI_State.MOVE:
                if (_timer <= 0)
                {
                    _currentState = AI_State.WAIT;
                    _timer = _waitTime.GetRandomValue();
                    enemy.SetVelocity(Vector3.zero);
                }
                break;
            default:
                break;
        }
    }

    public void SetState(int state)
    {
        SetState((AI_State)state);
    }

    public void SetState(AI_State state)
    {
        _currentState = state;
        switch (_currentState)
        {
            default:
            case AI_State.WAIT:
                // Should make function
                enemy.SetVelocity(Vector3.zero);
                _timer = _waitTime.GetRandomValue();
                break;
            case AI_State.MOVE:
                HandleStateChangeToMove();
                break;
        }
    }

    private static readonly Vector3[] facings = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    private void HandleStateChangeToMove()
    {
        int i = Random.Range(0, 4);
        _facing = facings[i];
        enemy.SetVelocity(_facing * _maxSpeed);
        int dist = Mathf.RoundToInt(_moveDistance.GetRandomValue());
        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

        // TODO: Check if the new target position is in the bounds

        _timer = dist / _maxSpeed;
    }
}
