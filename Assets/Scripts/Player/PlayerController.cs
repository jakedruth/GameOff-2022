using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JDR.ExtensionMethods;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Actor Actor { get; private set; }

    // Components
    private PlayerInput _playerInput;
    private ItemController _itemController;
    private CharacterController _characterController;
    private Animator _animator;

    // Inputs
    private Vector3 _facing;
    private Vector3 _inputMove;
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _action1Action;
    private InputAction _action2Action;
    private InputAction _action3Action;
    private InputAction _pauseAction;

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
    protected void Awake()
    {
        instance = this;
        Actor = GetComponent<Actor>();

        _playerInput = GetComponent<PlayerInput>();
        _itemController = GetComponent<ItemController>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        Init();
    }

    private void Init()
    {
        _itemController.InitReferences(this, _animator);
        _itemController.SetItem<SwordHandler>(0);
        _itemController.SetItem<BoomerangHandler>(1, 1);

        _facing = Vector3.forward;
        inventory = new Inventory();
    }

    #region Setup Input events
    protected void OnEnable()
    {
        // Move
        _moveAction = _playerInput.currentActionMap.FindAction("Move");
        _moveAction.started += OnMove;
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;

        // Interact
        _interactAction = _playerInput.currentActionMap.FindAction("Interact");
        _interactAction.started += OnInteract;
        _interactAction.performed += OnInteract;
        _interactAction.canceled += OnInteract;

        // Action 1
        _action1Action = _playerInput.currentActionMap.FindAction("Action1");
        _action1Action.started += OnAction;
        _action1Action.performed += OnAction;
        _action1Action.canceled += OnAction;

        // Action 2
        _action2Action = _playerInput.currentActionMap.FindAction("Action2");
        _action2Action.started += OnAction;
        _action2Action.performed += OnAction;
        _action2Action.canceled += OnAction;

        // Action 3
        _action3Action = _playerInput.currentActionMap.FindAction("Action3");
        _action3Action.started += OnAction;
        _action3Action.performed += OnAction;
        _action3Action.canceled += OnAction;

        // Pause
        _pauseAction = _playerInput.currentActionMap.FindAction("Pause");
        _pauseAction.started += OnPause;
    }

    protected void OnDisable()
    {
        // Move
        _moveAction.started -= OnMove;
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;

        // Interact
        _interactAction.started -= OnInteract;
        _interactAction.performed -= OnInteract;
        _interactAction.canceled -= OnInteract;

        // Action 1
        _action1Action.started -= OnAction;
        _action1Action.performed -= OnAction;
        _action1Action.canceled -= OnAction;

        // Action 2
        _action2Action.started -= OnAction;
        _action2Action.performed -= OnAction;
        _action2Action.canceled -= OnAction;

        // Action 3
        _action3Action.started -= OnAction;
        _action3Action.performed -= OnAction;
        _action3Action.canceled -= OnAction;

        // Pause
        _pauseAction.started -= OnPause;
    }
    #endregion 

    protected void Start()
    {
        HUD.instance.HealthBar.SetMaxHeatCount(Mathf.RoundToInt(Actor.maxHP));
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
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BeginInteract();
        }
    }

    // private void OnAction1(InputAction.CallbackContext context)
    // {
    //     switch (context.phase)
    //     {
    //         case InputActionPhase.Disabled:
    //             break;
    //         case InputActionPhase.Waiting:
    //             break;
    //         case InputActionPhase.Started:
    //             _itemController.ItemActionStarted(0);
    //             break;
    //         case InputActionPhase.Performed:
    //             break;
    //         case InputActionPhase.Canceled:
    //             _itemController.ItemActionEnded(0);
    //             break;
    //     }
    // }

    // private void OnAction2(InputAction.CallbackContext context)
    // {
    //     switch (context.phase)
    //     {
    //         case InputActionPhase.Disabled:
    //             break;
    //         case InputActionPhase.Waiting:
    //             break;
    //         case InputActionPhase.Started:
    //             _itemController.ItemActionStarted(1);
    //             break;
    //         case InputActionPhase.Performed:
    //             break;
    //         case InputActionPhase.Canceled:
    //             _itemController.ItemActionEnded(1);
    //             break;
    //     }
    // }

    // private void OnAction3(InputAction.CallbackContext context)
    // {
    //     switch (context.phase)
    //     {
    //         case InputActionPhase.Disabled:
    //             break;
    //         case InputActionPhase.Waiting:
    //             break;
    //         case InputActionPhase.Started:
    //             _itemController.ItemActionStarted(2);
    //             break;
    //         case InputActionPhase.Performed:
    //             break;
    //         case InputActionPhase.Canceled:
    //             _itemController.ItemActionEnded(2);
    //             break;
    //     }
    // }

    private void OnAction(InputAction.CallbackContext context)
    {
        // Gets the last char and uses it as an int. 
        // Subtracts the char '1' to make the range 0 to 2
        int index = context.action.name.Last() - '1';

        _itemController.HandleItemAction(index, context);

        // int index = -1;
        // int last = context.action.name.Last();
        // Debug.Log(last);
        // if (context.action.name == "Action1")
        //     index = 0;
        // else if (context.action.name == "Action2")
        //     index = 1;
        // else if (context.action.name == "Action3")
        //     index = 2;

        // switch (context.phase)
        // {
        //     case InputActionPhase.Disabled:
        //         break;
        //     case InputActionPhase.Waiting:
        //         break;
        //     case InputActionPhase.Started:
        //         _itemController.ItemActionStarted(index);
        //         break;
        //     case InputActionPhase.Performed:
        //         break;
        //     case InputActionPhase.Canceled:
        //         _itemController.ItemActionEnded(index);
        //         break;
        // }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        GameManager.Instance.ToggleIsPaused();
    }
    #endregion

    // Update is called once per frame
    protected void Update()
    {
        // Update rotation
        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

        // Update Horizontal velocity
        Vector3 input = _inputMove;
        float targetSpeed = _maxSpeed;

        _itemController.HandleMovement(ref input, ref targetSpeed);

        Vector3 targetVel = input * targetSpeed;
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

    protected bool BeginInteract()
    {
        // OnInteractEvent?.Invoke();
        Vector3 checkPoint = GetCenter() + transform.forward;
        const float radius = 0.5f;
        int layerMask = LayerMask.GetMask(Interactable.InteractLayerName);

        /* Debug Drawing */
        Debug.DrawLine(checkPoint + Vector3.back * radius, checkPoint + Vector3.forward * radius, Color.green, 0.5f, false);
        Debug.DrawLine(checkPoint + Vector3.down * radius, checkPoint + Vector3.up * radius, Color.green, 0.5f, false);
        Debug.DrawLine(checkPoint + Vector3.left * radius, checkPoint + Vector3.right * radius, Color.green, 0.5f, false);
        /* End Debug Drawing */

        Collider[] colliders = Physics.OverlapSphere(checkPoint, radius, layerMask, QueryTriggerInteraction.Ignore);

        // Check if there is anything to interact with
        Debug.Log($"Num of colliders: {colliders.Length}");
        if (colliders.Length == 0)
            return false;

        // Find closest
        Collider closet = null;
        float best = Mathf.Infinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            float sqrDist = (colliders[i].transform.position - checkPoint).sqrMagnitude;
            if (sqrDist < best)
            {
                best = sqrDist;
                closet = colliders[i];
            }

            /* Debug Drawing */
            Debug.DrawRay(colliders[i].bounds.center, Vector3.up, Color.blue, 0.5f, false);
            /* End Debug Drawing */
        }

        if (closet == null)
            return false;

        // Get Interactable
        Interactable interactable = closet.GetComponent<Interactable>();
        if (interactable == null)
        {
            // Check parent
            interactable = closet.GetComponentInParent<Interactable>();
            if (interactable == null)
                return false;
        }

        bool success = interactable.TryInteract();

        return success;
    }

    void HandleAnimator()
    {
        _animator.SetFloat("Walk_Speed", _hVelocity.sqrMagnitude);
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
