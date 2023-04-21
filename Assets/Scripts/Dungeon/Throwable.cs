using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Interactable
{
    // Components
    private Rigidbody _body;

    public bool HasBeenThrown { get; set; }
    [Header("Throwable Values")][SerializeField] private bool _breakOnImpact;
    [SerializeField] private float _torqueOnThrow;
    [SerializeField] private int _damageOnThrow;

    protected virtual void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    protected override bool CanInteract(InteractController controller)
    {
        return true;
    }

    protected override void Interact(InteractController controller)
    {
        controller.Carry(this);
        _body.isKinematic = true;
    }

    public void Throw(Vector3 velocity)
    {
        transform.SetParent(null);
        HasBeenThrown = true;

        _body.isKinematic = false;
        _body.constraints = RigidbodyConstraints.None;
        _body.AddForce(velocity, ForceMode.VelocityChange);
        _body.AddRelativeTorque(Vector3.right * _torqueOnThrow, ForceMode.Impulse);
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (!HasBeenThrown)
            return;

        // Check if you hit a switch
        Switch otherSwitch = other.gameObject.GetComponent<Switch>();
        otherSwitch?.ActivateSwitch();

        // Check if you hit an actor
        Actor otherActor = other.gameObject.GetComponent<Actor>();
        otherActor?.ApplyDamage(_damageOnThrow);

        // Should this break when thrown?
        if (_breakOnImpact)
        {
            Actor actor = GetComponent<Actor>();
            actor?.InstantKill();
        }
    }
}
