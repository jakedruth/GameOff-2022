using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JDR.ExtensionMethods;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Actor actor { get; private set; }

    // Components
    private PlayerInput _playerInput;
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
    [SerializeField] private Vector3 _drag;
    private Vector3 _hVelocity;
    private Vector3 _vVelocity;
    private Vector3 _pushBackVelocity;
    private Vector3 _floorPusherVelocity;

    [Header("Physics Properties")]
    [SerializeField] private LayerMask _groundLayerMask;
    private bool _isGrounded;

    // Events
    public PlayerInput.ControlsChangedEvent ControlsChangedEvent { get { return _playerInput.controlsChangedEvent; } }
    public PlayerInput CurrentPlayerInput { get { return _playerInput; } }

    public Inventory inventory;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        actor = GetComponent<Actor>();

        _playerInput = GetComponent<PlayerInput>();
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
        inventory = new Inventory();
    }

    void Start()
    {
        HUD.instance.HealthBar.SetMaxHeatCount(Mathf.RoundToInt(actor.maxHP));
    }

    public Vector3 GetCenter()
    {
        return _characterController.bounds.center;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _hVelocity = velocity;
    }

    public void WarpToPoint(Vector3 point)
    {
        _characterController.enabled = false;
        transform.position = point;
        _characterController.enabled = true;
    }

    public void ApplyPushBack(Vector3 direction, float distance)
    {
        // Fancy math to apply the right amount of force
        Vector3 pushMagnitude = distance * new Vector3(
            Mathf.Log(1f / (Time.deltaTime * _drag.x + 1)) / -Time.deltaTime, 0,
            Mathf.Log(1f / (Time.deltaTime * _drag.z + 1)) / -Time.deltaTime);
        _pushBackVelocity += Vector3.Scale(direction, pushMagnitude);
    }

    public void SetFloorPusherVelocity(Vector3 velocity)
    {
        _floorPusherVelocity = velocity;
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

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>().ToVector3_XZ();
        input = SnapToAngle(input, 45f, Vector3.forward);

        if (context.performed)
            _facing = input;

        _inputMove = input;
    }

    public UnityEngine.Events.UnityEvent OnInteractEvent { get; set; } = new UnityEngine.Events.UnityEvent();
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteractEvent?.Invoke();
        }
    }

    public void OnSword(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                _swordController.EndAttack();
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                _swordController.StartAttack();
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                _swordController.EndAttack();
                break;
        }
    }

    public void OnBoomerang(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                _boomerangController.EndAttack();
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                if (_boomerangController.CanThrow)
                {
                    _boomerangController.StartAttack();
                    TemporaryDisableInput(0.25f);
                    _Animator.SetTrigger("Attack2");
                }
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                _boomerangController.EndAttack();
                break;
        }
    }
    #endregion


    // Update is called once per frame
    protected void Update()
    {
        // Update rotation
        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

        // Update Horizontal velocity
        float targetSpeed = _swordController.inputKey ? 0 : _maxSpeed;
        Vector3 targetVel = _inputMove * targetSpeed;
        _hVelocity = Vector3.MoveTowards(_hVelocity, targetVel, _acceleration * Time.deltaTime);

        // Update vertical velocity
        //_isGrounded = Physics.CheckSphere(transform.position, 0.2f, _groundLayerMask);
        _isGrounded = (_characterController.collisionFlags & CollisionFlags.Below) != 0;
        if (_isGrounded)
            _vVelocity.y = 0;

        _vVelocity += Physics.gravity * Time.deltaTime;

        // Update animator
        HandleAnimator();
    }

    protected void FixedUpdate()
    {
        // Apply Drag
        _pushBackVelocity.x /= 1 + _drag.x * Time.deltaTime;
        _pushBackVelocity.y /= 1 + _drag.y * Time.deltaTime;
        _pushBackVelocity.z /= 1 + _drag.z * Time.deltaTime;

        _characterController.Move((_hVelocity + _vVelocity + _pushBackVelocity + _floorPusherVelocity) * Time.deltaTime);
        _floorPusherVelocity = Vector3.zero;
    }

    void HandleAnimator()
    {
        _Animator.SetFloat("Walk_Speed", _hVelocity.sqrMagnitude);
        _Animator.SetBool("Attack1", _swordController.inputKey);
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Handle taking a hit?
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit by Enemy");
        }
    }

    private Vector3 SnapToAngle(Vector3 input, float snapAngle, Vector3 forward)
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

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class InventorySlot<T>
    {
        private T slot;
        public UnityEngine.Events.UnityEvent<T> OnSlotUpdated = new();
        public InventorySlot() { }

        public T Get()
        {
            return slot;
        }

        public void Set(T value)
        {
            if (slot.Equals(value))
                return;

            slot = value;
            OnSlotUpdated.Invoke(slot);
        }

        public override string ToString()
        {
            return slot.ToString();
        }
    }

    public InventorySlot<int> key = new();
}
