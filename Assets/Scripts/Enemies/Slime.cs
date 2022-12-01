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
    private Vector3[] Facings { get; } = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

    // Components
    public Enemy enemy { get; private set; }
    private Animator _animator;

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
        _animator = GetComponentInChildren<Animator>();

        Init();
    }

    private void SetFacing(Vector3 facing)
    {
        _facing = facing;
        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);
    }

    private void Init()
    {
        // Randomly rotate the slime
        int i = Random.Range(0, 4);
        SetFacing(Facings[i]);

        // Set the state to wait
        SetState(AI_State.WAIT);
    }

    void Update()
    {
        // Handle timer
        _timer -= Time.deltaTime;
        switch (_currentState)
        {
            case AI_State.WAIT:
                if (_timer <= 0)
                    SetState(AI_State.MOVE);
                break;
            case AI_State.MOVE:
                if (_timer <= 0)
                    SetState(AI_State.WAIT);
                break;
            default:
                break;
        }
    }

    public void SetState(int state)
    {
        state = Mathf.Clamp(state, 0, (int)AI_State.MOVE);
        SetState((AI_State)state);
    }

    public void SetState(AI_State state)
    {
        _currentState = state;
        switch (_currentState)
        {
            default:
            case AI_State.WAIT:
                HandleStateChangeToWait();
                break;
            case AI_State.MOVE:
                HandleStateChangeToMove();
                break;
        }
    }

    private void HandleStateChangeToWait()
    {
        enemy.SetVelocity(Vector3.zero);
        _animator.SetFloat("Walk_Speed", 0f);
        _timer = _waitTime.GetRandomValue();
    }

    private void HandleStateChangeToMove()
    {
        int i = Random.Range(0, 4);
        Vector3 dir = Facings[i];
        int dist = Mathf.RoundToInt(_moveDistance.GetRandomValue());

        // TODO: Check if the new target position is in the bounds

        _timer = dist / _maxSpeed;
        SetFacing(dir);
        enemy.SetVelocity(_facing * _maxSpeed);

        _animator.SetFloat("Walk_Speed", _maxSpeed);
    }


}
