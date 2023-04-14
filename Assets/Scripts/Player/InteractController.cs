using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour
{
    [SerializeField] private Transform _carryPoint;
    private Throwable _carryObject;

    [SerializeField] private float _throwAngle;
    [SerializeField] private float _throwStrength;

    protected void Awake()
    {
    }

    public void HandleInteractAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_carryObject != null)
            {
                ThrowCarryObject();
                return;
            }

            BeginInteract();
        }
    }

    public void Carry(Throwable objectToCarry)
    {
        objectToCarry.transform.SetParent(_carryPoint);
        objectToCarry.transform.localPosition = Vector3.zero;
        objectToCarry.transform.localRotation = Quaternion.identity;
        _carryObject = objectToCarry;
    }

    private void ThrowCarryObject()
    {
        Vector3 dir = transform.TransformDirection(Quaternion.AngleAxis(-_throwAngle, Vector3.right) * Vector3.forward);
        Debug.DrawRay(_carryPoint.position, dir * 2, Color.green, 0.5f, false);
        _carryObject.Throw(dir, _throwStrength);
        _carryObject = null;
    }

    protected bool BeginInteract()
    {
        Vector3 checkPoint = PlayerController.instance.GetCenter() + transform.forward;
        const float radius = 0.5f;
        int layerMask = LayerMask.GetMask(Interactable.InteractLayerName);

        // /* Debug Drawing
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

            // /* Debug Drawing
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

        bool success = interactable.TryInteract(this);

        return success;
    }
}
