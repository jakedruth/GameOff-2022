using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _camHolder;
    private Quaternion _startRotation;
    public Transform target;
    public float maxSpeed;
    public float maxTilt;

    void Awake()
    {
        _camHolder = transform.GetChild(0);
        _startRotation = _camHolder.rotation;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        // Move the Camera
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, maxSpeed * Time.deltaTime);
        transform.position = pos;

        // Rotate the camera
        Quaternion targetLook = Quaternion.LookRotation(target.position - _camHolder.position, Vector3.up);
        _camHolder.rotation = Quaternion.RotateTowards(_startRotation, targetLook, maxTilt);
    }
}
