using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    public Pawn pawn { get; private set; }

    // Inputs
    private Vector3 _facing;
    private Vector3 _inputMove;

    [Header("Movement Values")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    private Vector3 _velocity;

    // Start is called before the first frame update
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        pawn = GetComponent<Pawn>();
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);

        Vector2 input = context.ReadValue<Vector2>();
        input = SnapTo(input, 45f);

        _inputMove.x = input.x;
        _inputMove.y = 0;
        _inputMove.z = input.y;

        if (context.performed)
        {
            _facing = _inputMove;
        }
    }

    public void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Update rotation
        transform.rotation = Quaternion.LookRotation(_facing, Vector3.up);

        // Update Velocity
        Vector3 targetVel = _inputMove * _maxSpeed;
        _velocity = Vector3.MoveTowards(_velocity, targetVel, _acceleration * Time.deltaTime);
    }

    void FixedUpdate()
    {
        _characterController.Move(_velocity * Time.deltaTime);
    }

    Vector3 SnapTo(Vector3 v3, float snapAngle)
    {
        float angle = Vector3.Angle(v3, Vector3.up);
        if (angle < snapAngle / 2.0f)          // Cannot do cross product 
            return Vector3.up * v3.magnitude;  //   with angles 0 & 180
        if (angle > 180.0f - snapAngle / 2.0f)
            return Vector3.down * v3.magnitude;

        float t = Mathf.Round(angle / snapAngle);
        float deltaAngle = (t * snapAngle) - angle;

        Vector3 axis = Vector3.Cross(Vector3.up, v3);
        Quaternion q = Quaternion.AngleAxis(deltaAngle, axis);
        return q * v3;
    }
}
