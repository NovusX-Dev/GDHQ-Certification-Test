using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float _maxHealth = 5f;
    [SerializeField] protected Color _baseColor = Color.white;

    [Header("Movement")]
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected bool _delayEnterPlayableArea = false;
    [SerializeField] protected float _delayTime = 2f;

    [Header("Pickup Drop")]
    [SerializeField] protected bool _canDropPickup = false;
    [SerializeField] protected Pickup[] _pickups = null;

    [Header("Attack")]
    [SerializeField] protected float _fireRate = 2f;
    [SerializeField] protected float _minFireRate = 1.5f;
    [SerializeField] protected Transform _weaponPos = null;
    [SerializeField] protected Transform _projectileParent;
    [SerializeField] protected GameObject _laserPrefab = null;

    [Header("Score")]
    [SerializeField] protected int _comboScore = 1;
    [SerializeField] protected int _killScore = 25;

    [Header("References")]
    [SerializeField] protected ParticleSystem[] _deathVFXs;

    protected float _currentHealth;
    protected bool _inPlayableArea = false;
    protected float _nextFire = 0;
    protected WaitForSeconds _delayPlayableWaitForSeconds;

    protected MeshRenderer _meshRenderer;
    protected MeshRenderer[] _childrenRenderer;
    protected EnemyBulletBase _enemyBulletBase = null;

    public static Action<int> OnEnemyDeathCombo;
    public static Action<int> OnEnemyDeathKillScore;

    protected ColorBlinker _blinker;

    protected virtual void Awake()
    {
        if (GetComponent<MeshRenderer>() != null) _meshRenderer = GetComponent<MeshRenderer>();
        else
        {
            _childrenRenderer = GetComponentsInChildren<MeshRenderer>();
        }

        _blinker = GetComponent<ColorBlinker>();
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _projectileParent = GameObject.FindGameObjectWithTag("Projectile Parent").transform;
        _delayPlayableWaitForSeconds = new WaitForSeconds(_delayTime);
        if (_canDropPickup) _killScore += 25;
        if (_meshRenderer != null) _meshRenderer.material.color = _baseColor;
        else
        {
            foreach (var renderer in _childrenRenderer)
            {
                renderer.material.color = _baseColor;
            }
        }
        if (_laserPrefab != null)
        {
            _enemyBulletBase = _laserPrefab.GetComponent<EnemyBulletBase>();
        }
    }

    protected virtual void Update()
    {
        if (_inPlayableArea)
        {
            if (Time.time > _nextFire)
            {
                FireWeapon();
            }

            Move();
        }
    }

    protected virtual void FireWeapon()
    {
        if (_laserPrefab == null) return;
        var laserBullet = Instantiate(_laserPrefab, _weaponPos.position, _enemyBulletBase.Rotation, _projectileParent);
        laserBullet.GetComponent<EnemyBulletBase>().SetEnemyBulletDirection(Vector3.right);
        laserBullet.transform.position = new Vector3(laserBullet.transform.position.x, 0, laserBullet.transform.position.z);
        _nextFire = Time.time + UnityEngine.Random.Range(_minFireRate, _fireRate);
    }

    protected virtual void Move()
    {

    }

    public virtual void DamageEnemy(float amount)
    {
        _currentHealth -= amount;
        if(_blinker != null) _blinker.CanBlink = true;

        if (_currentHealth <= 0)
        {
            OnEnemyDeathCombo?.Invoke(_comboScore);
            OnEnemyDeathKillScore?.Invoke(_killScore);
            DestroyEnemy();
        }
    }

    protected virtual void DestroyEnemy()
    {
        if (_canDropPickup)
        {
            Instantiate(_pickups[UnityEngine.Random.Range(0, _pickups.Length)], transform.position, Quaternion.Euler(90f, 0f, 0f));
        }

        if (_deathVFXs != null)
        {
            Instantiate(_deathVFXs[UnityEngine.Random.Range(0, _deathVFXs.Length)], transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    #region Collisions and Triggers

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Projectile"))
        {
            DamageEnemy(other.GetComponent<BulletLaserBase>().GetBulletDamage());
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Playable Area"))
        {
            StartCoroutine(DelayEnteringPlayableArea());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Playable Area"))
        {
            _inPlayableArea = false;
        }
    }

    IEnumerator DelayEnteringPlayableArea()
    {
        if (_delayEnterPlayableArea)
        {
            yield return _delayPlayableWaitForSeconds;
            _inPlayableArea = true;
        }
        else if (!_delayEnterPlayableArea)
        {
            yield return null;
            _inPlayableArea = true;
        }
    }
    #endregion
}
