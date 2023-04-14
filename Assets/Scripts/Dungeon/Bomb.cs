using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Throwable
{
    [SerializeField] private int _damage;
    [SerializeField] private float _blastRadius;
    [SerializeField] private float _blastTime;
    private float _timer;

    protected void Start()
    {
        _timer = _blastTime;
    }

    // Update is called once per frame
    protected void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Explode();
        }
    }

    public void Explode()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _blastRadius);
    }
}
