using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private Animator _animator;
    private MeshRenderer _meshRenderer;

    [SerializeField] private Color[] _colors;
    [SerializeField] private bool _isActive;
    [SerializeField] private Teleporter _destination;
    protected bool ignoreOnEnter;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Start()
    {
        SetIsActive(_isActive);
    }

    public void SetIsActive(bool value)
    {
        _animator.SetBool("SetState", value);
        _isActive = value;
        SetMaterialTint(_isActive ? 1 : 0);
    }

    private void SetMaterialTint(int index)
    {
        if (_colors == null || _colors.Length == 0 || index >= _colors.Length)
            return;

        _meshRenderer.material.color = _colors[index];
    }

    public void SetDestinationTeleporter(Teleporter destination)
    {
        _destination = destination;
    }

    void TeleportPlayer(PlayerController player)
    {
        if (_destination == null)
            return;

        _destination.ignoreOnEnter = true;
        player.TemporaryDisableInput(0.25f);
        player.WarpToPoint(_destination.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (ignoreOnEnter)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
            return;

        TeleportPlayer(player);
    }

    void OnTriggerExit(Collider other)
    {
        ignoreOnEnter = false;
    }
}
