using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _camHolder;
    public Transform target;
    public float maxSpeed;

    void Awake()
    {
        _camHolder = transform.GetChild(0);
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        // Move the Camera
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, maxSpeed * Time.deltaTime);
        transform.position = pos;

        // Rotate the camera
        _camHolder.rotation = Quaternion.LookRotation(target.position - _camHolder.position, Vector3.up);
    }
}
