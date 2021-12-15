using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeopardMisslleLauncher : MonoBehaviour
{
    [SerializeField] GameObject _spaceTorpedo;
    [SerializeField] Vector2 _firRate = new Vector2(8f, 12f);

    private bool _isActive = true;
    private float _nextFire;

    PlayerHealth _player;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += PlayerIsDead;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayerIsDead;
    }

    private void Start()
    {
        _nextFire = Time.time + 10f;
        _player = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        if(!_isActive || _player == null) return;

        if(Time.time > _nextFire)
        {
            Instantiate(_spaceTorpedo, transform.position, Quaternion.identity);
            _nextFire = Time.time + Random.Range(_firRate.x, _firRate.y);
        }
    }

    public bool EnableMissileLaunchers(bool active)
    {
        return _isActive = active;
    }

    private void PlayerIsDead()
    {
        _player = null;
    }
}
