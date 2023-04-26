using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Throwable
{
    [Header("Bomb Values")]
    [SerializeField] private GameObject _particleSystem;
    private Animator _animator;

    [SerializeField] private float _centerOffset;
    [SerializeField] private int _damage;
    [SerializeField] private float _blastRadius;
    [SerializeField] private float _blastTime;
    [SerializeField][Range(0, 1)] private float _animSpeedMultiplier;
    private float _timer;
    private float _animSpeed = 1;

    public UnityEngine.Events.UnityEvent OnExplode;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
        OnExplode = new UnityEngine.Events.UnityEvent();
    }

    protected void Start()
    {
        _timer = 0;
        _animSpeed = 1;
        _animator.speed = _animSpeed;
    }

    // Update is called once per frame
    protected void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _blastTime)
        {
            Explode();
        }

        // Exponentially increase the animation speed every second
        int r = 1 + Mathf.FloorToInt(_timer * _animSpeedMultiplier);
        if (_animSpeed < r * r)
        {
            _animator.speed = _animSpeed = r * r; ;
        }
    }

    public void Explode()
    {
        _particleSystem.SetActive(true);
        _particleSystem.transform.SetParent(null);

        // Get every object in range
        Collider[] colliders = Physics.OverlapSphere(CenterPointToWorld(), _blastRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            // Apply damage to each actor in range
            Actor actor = colliders[i].gameObject.GetComponent<Actor>();
            if (actor != null)
            {
                actor.ApplyDamage(_damage);
                continue;
            }

            // Activate each switch in range
            Switch s = colliders[i].gameObject.GetComponent<Switch>();
            if (s != null)
            {
                s.ActivateSwitch();
                continue;
            }
        }

        OnExplode?.Invoke();

        Destroy(gameObject);
    }

    public Vector3 CenterPointToWorld()
    {
        return transform.TransformPoint(Vector3.up * _centerOffset);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(CenterPointToWorld(), _blastRadius);
    }
}
