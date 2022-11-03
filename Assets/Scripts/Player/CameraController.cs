using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float maxSpeed;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 pos = Vector3.Lerp(transform.position, target.position, 1 - Mathf.Exp(-maxSpeed * Time.deltaTime));
        transform.position = pos;
    }
}
