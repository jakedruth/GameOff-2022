using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Interactable
{
    [SerializeField] private int _damage;
    [SerializeField] private float _blastRadius;

    // Start is called before the first frame update
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {

    }

    public override bool TryInteract()
    {
        throw new System.NotImplementedException();
    }

    protected override void Interact()
    {
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _blastRadius);
    }
}
