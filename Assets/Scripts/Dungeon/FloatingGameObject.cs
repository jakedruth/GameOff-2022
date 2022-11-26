using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingGameObject : MonoBehaviour
{
    [Header("Controls")]
    public bool isOn;

    [Header("Displacement properties")]
    [SerializeField] private float _amplitude;
    [SerializeField] private float _frequency;
    private Vector3 _startLocalPos;

    [Header("Rotation properties")]
    [SerializeField] private float _rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _startLocalPos = transform.localPosition;
    }

    public void SetIsOn(bool value)
    {
        if (isOn == value)
            return;

        isOn = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn)
            return;

        float y = _amplitude * Mathf.Sin(Time.time * 2 * Mathf.PI / (float.Epsilon + _frequency));
        Vector3 pos = new(0, y, 0);

        transform.localPosition = _startLocalPos + pos;
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
