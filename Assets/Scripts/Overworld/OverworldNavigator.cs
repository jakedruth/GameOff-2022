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
        _currentNode = _overworldManager.routes[0].nodeA;
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
            Debug.Log($"Input: {input}\tDir: {inputDir}");
            HandleMoveInput(inputDir);
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {

    }

    public void Cancel(InputAction.CallbackContext context)
    {

    }

    private void HandleMoveInput(CompassDirection moveDir)
    {
        List<Route> routes = _overworldManager.GetRoutesWithNode(_currentNode);
        Debug.Log(routes.Count);
        for (int i = 0; i < routes.Count; i++)
        {
            Route route = routes[i];
            if (route.nodeA == _currentNode && route.directionA == moveDir)
            {
                StartCoroutine(HandleMovement(route, false));
                return;
            }

            if (route.nodeB == _currentNode && route.directionB == moveDir)
            {
                StartCoroutine(HandleMovement(route, true));
                return;
            }
        }

    }

    private IEnumerator HandleMovement(Route route, bool invertPath)
    {
        Debug.Log("Beginning to move");
        _isMoving = true;
        Vector3 targetPos = invertPath ? route.nodeA.transform.position : route.nodeB.transform.position;
        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        _isMoving = false;
        _currentNode = invertPath ? route.nodeA : route.nodeB;
    }
}
