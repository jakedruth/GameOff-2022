using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JDR.ExtensionMethods;

public class OverworldNavigator : MonoBehaviour
{
    private OverworldManager _overworldManager;

    [SerializeField] private float _moveSpeed;
    private LevelNode _currentNode;
    private bool _isMoving;

    protected void Awake()
    {
        _overworldManager = FindObjectOfType<OverworldManager>();
        _currentNode = _overworldManager.startNode;
        transform.position = _currentNode.transform.position;
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (_isMoving)
                return;

            Vector3 input = context.ReadValue<Vector2>().ToVector3_XZ();
            if (input.sqrMagnitude < 0.5f)
                return;

            CompassDirection inputDir = input.ToClosestCompassDirection();
            HandleMoveInput(inputDir);
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !_isMoving)
            _overworldManager.LoadLevel(_currentNode);
    }

    public void Cancel(InputAction.CallbackContext context)
    {

    }

    private void HandleMoveInput(CompassDirection moveDir)
    {
        Trail trail = _currentNode.GetTrail(moveDir);
        StartCoroutine(HandleMovement(trail));
    }

    private IEnumerator HandleMovement(Trail trail)
    {
        Path path = _overworldManager.GetPath(trail);
        if (path == null)
            yield break;

        _isMoving = true;
        for (int i = 0; i < path.Length; i++)
        {
            int index = trail.invertPath ? path.Length - 1 - i : i;
            Vector3 start = transform.position;
            Vector3 end = path[index];
            float distance = Vector3.Distance(start, end);
            float time = (distance + 0.01f) / (_moveSpeed + 0.01f);
            float t = 0;
            while (t <= time)
            {
                t += Time.deltaTime;
                float k = t / time;
                transform.position = Vector3.Lerp(start, end, k);

                yield return null;
            }
            transform.position = end;
        }

        _isMoving = false;
        _currentNode = trail.targetNode;
    }
}
