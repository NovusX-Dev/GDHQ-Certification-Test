using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomBadger : Enemy
{
    [Header ("Badger Specific")]
    [SerializeField] float _damage = 0.15f;
    [SerializeField] float _damageRate = 0.25f;
    [SerializeField] float _sinFrequency = 20f, _sinMagnitude = 0.5f;

    private Vector3 _direction;
    private float _nextDamage = -1;

    protected override void Start()
    {
        base.Start();
        _direction = transform.position;
    }

    protected override void Move()
    {
        var sinMove =  Mathf.Sin(Time.time * _sinFrequency) * _sinMagnitude;
        _direction += Vector3.left * _moveSpeed * Time.deltaTime;

        transform.position = _direction + transform.up * sinMove;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            var player = other.GetComponent<PlayerHealth>();
            if(Time.time > _nextDamage)
            {
                 player.DamagePlayer(_damage);
                _nextDamage = Time.time + _damageRate;
            }
        }

    }

}
