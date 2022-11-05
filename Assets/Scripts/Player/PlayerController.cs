using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDR.ExtensionMethods;

public class PlayerController : MonoBehaviour
{
    // Pawn
    public Actor actor { get; private set; }

    // Components
    private CharacterController _characterController;
    private SwordController _swordController;
    private BoomerangController _boomerangController;

    // Inputs
    private Vector3 _facing;
    private Vector3 _inputMove;
    private bool _inputAttack1; // I should move these booleans to their controllers
    private bool _inputAttack2;

    [Header("Movement Values")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    private Vector3 _velocity;

    // Start is called before the first frame update
    void Awake()
    {
        actor = GetComponent<Actor>();

        _characterController = GetComponent<CharacterController>();
        _swordController = GetComponent<SwordController>();
        _boomerangController = GetComponent<BoomerangController>();

        Init();
    }

    private void Init()
    {
        _swordController.SetOwner(actor);
        _boomerangController.SetOwner(actor);

        _facing = Vector3.forward;
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>().Vector2ToVector3_XZ();
        input = SnapToAngle(input, 45f, Vector3.forward);

        if (context.performed)
            _facing = input;

        _inputMove = input;
    }

    public void OnSword(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case UnityEngine.InputSystem.InputActionPhase.Disabled:
                _swordController.EndAttack();
                _inputAttack1 = false;
                break;
            case UnityEngine.InputSystem.InputActionPhase.Waiting:
                break;
            case UnityEngine.InputSystem.InputActionPhase.Started:
                _swordController.StartAttack();
                break;
            case UnityEngine.InputSystem.InputActionPhase.Performed:
                _inputAttack1 = true;
                break;
            case UnityEngine.InputSystem.InputActionPhase.Canceled:
                _swordController.EndAttack();
                _inputAttack1 = false;
                break;
        }
    }

    public void OnBoomerang(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case UnityEngine.InputSystem.InputActionPhase.Disabled:
                _boomerangController.EndAttack();
                _inputAttack2 = false;
                break;
            case UnityEngine.InputSystem.InputActionPhase.Waiting:
                break;
            case UnityEngine.InputSystem.InputActionPhase.Started:
                _boomerangController.StartAttack();
                break;
            case UnityEngine.InputSystem.InputActionPhase.Performed:
                _inputAttack2 = true;
                break;
            case UnityEngine.InputSystem.InputActionPhase.Canceled:
                _boomerangController.EndAttack();
                _inputAttack2 = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update rotation
        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

        // Update Velocity
        float targetSpeed = _inputAttack1 ? 0 : _maxSpeed;
        Vector3 targetVel = _inputMove * targetSpeed;
        _velocity = Vector3.MoveTowards(_velocity, targetVel, _acceleration * Time.deltaTime);
    }

    void FixedUpdate()
    {
        _characterController.Move(_velocity * Time.deltaTime);
    }

    Vector3 SnapToAngle(Vector3 input, float snapAngle, Vector3 forward)
    {
        float angle = Vector3.Angle(input, forward);
        if (angle < snapAngle / 2.0f)          // Cannot do cross product 
            return forward * input.magnitude;  //   with angles 0 & 180
        if (angle > 180.0f - snapAngle / 2.0f)
            return -forward * input.magnitude;

        float t = Mathf.Round(angle / snapAngle);
        float deltaAngle = (t * snapAngle) - angle;

        Vector3 axis = Vector3.Cross(forward, input);
        Quaternion q = Quaternion.AngleAxis(deltaAngle, axis);
        return q * input;
    }
}
