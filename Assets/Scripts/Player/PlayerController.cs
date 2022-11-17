using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDR.ExtensionMethods;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Actor actor { get; private set; }

    // Components
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    private CharacterController _characterController;
    private SwordController _swordController;
    private BoomerangController _boomerangController;
    private Animator _Animator;

    // Inputs
    private Vector3 _facing;
    private Vector3 _inputMove;

    [Header("Movement Values")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    private Vector3 _hVelocity;
    private Vector3 _vVelocity;

    [Header("Physics Properties")]
    [SerializeField] private float _gravity;
    [SerializeField] private LayerMask _groundLayerMask;
    private bool _isGrounded;

    // Inventory (not a good name and should probably move anyways)
    public int KeyCount { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        actor = GetComponent<Actor>();

        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        _swordController = GetComponent<SwordController>();
        _boomerangController = GetComponent<BoomerangController>();
        _Animator = GetComponentInChildren<Animator>();

        Init();
    }

    private void Init()
    {
        _swordController.SetOwner(actor);
        _boomerangController.SetOwner(actor);

        _facing = Vector3.forward;
    }

    public Vector3 GetCenter()
    {
        return _characterController.bounds.center;
    }

    #region Input System Events
    public void TemporaryDisableInput(float timer)
    {
        StartCoroutine(DisableInputTimer(timer));
    }

    private IEnumerator DisableInputTimer(float timer)
    {
        DisableInput();
        yield return new WaitForSeconds(timer);
        ActivateInput();
    }

    private void DisableInput()
    {
        _playerInput.DeactivateInput();
    }

    private void ActivateInput()
    {
        _playerInput.ActivateInput();
    }


    public void SetPlayerInputMap(string map)
    {
        _playerInput.SwitchCurrentActionMap(map);
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>().Vector2ToVector3_XZ();
        input = SnapToAngle(input, 45f, Vector3.forward);

        if (context.performed)
            _facing = input;

        _inputMove = input;
    }

    public UnityEngine.Events.UnityEvent OnInteractEvent { get; set; } = new UnityEngine.Events.UnityEvent();
    public void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteractEvent?.Invoke();
        }
    }

    public void OnSword(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case UnityEngine.InputSystem.InputActionPhase.Disabled:
                _swordController.EndAttack();
                break;
            case UnityEngine.InputSystem.InputActionPhase.Waiting:
                break;
            case UnityEngine.InputSystem.InputActionPhase.Started:
                _swordController.StartAttack();
                break;
            case UnityEngine.InputSystem.InputActionPhase.Performed:
                break;
            case UnityEngine.InputSystem.InputActionPhase.Canceled:
                _swordController.EndAttack();
                break;
        }
    }

    public void OnBoomerang(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case UnityEngine.InputSystem.InputActionPhase.Disabled:
                _boomerangController.EndAttack();
                break;
            case UnityEngine.InputSystem.InputActionPhase.Waiting:
                break;
            case UnityEngine.InputSystem.InputActionPhase.Started:
                if (_boomerangController.CanThrow)
                {
                    _boomerangController.StartAttack();
                    TemporaryDisableInput(0.25f);
                    _Animator.SetTrigger("Attack2");
                }
                break;
            case UnityEngine.InputSystem.InputActionPhase.Performed:
                break;
            case UnityEngine.InputSystem.InputActionPhase.Canceled:
                _boomerangController.EndAttack();
                break;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        // Update rotation
        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

        // Update Horizontal velocity
        float targetSpeed = _swordController.inputKey ? 0 : _maxSpeed;
        Vector3 targetVel = _inputMove * targetSpeed;
        _hVelocity = Vector3.MoveTowards(_hVelocity, targetVel, _acceleration * Time.deltaTime);

        // Update vertical velocity
        _vVelocity.y -= _gravity * Time.deltaTime;
        //_isGrounded = Physics.CheckSphere(transform.position, 0.2f, _groundLayerMask);
        _isGrounded = (_characterController.collisionFlags & CollisionFlags.Below) != 0;
        if (_isGrounded)
            _vVelocity.y = 0;

        // Update animator
        HandleAnimator();
    }

    void FixedUpdate()
    {
        _characterController.Move((_hVelocity + _vVelocity) * Time.deltaTime);
    }

    void HandleAnimator()
    {
        _Animator.SetFloat("Walk_Speed", _hVelocity.sqrMagnitude);
        _Animator.SetBool("Attack1", _swordController.inputKey);
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
