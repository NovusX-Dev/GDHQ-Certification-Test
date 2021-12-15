using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkamotoTurret : MonoBehaviour
{
    [SerializeField] float _mainTurrentRotationSpeed = 5f;
    [SerializeField] Transform[] _cannonPositions;
    [SerializeField] Vector2 _fireRate = new Vector2(1f, 5f);
    [SerializeField] GameObject _explosionVFX;

    private bool _isActive = false;
    private bool _isDestroyed = false;
    private bool _canRotate = true;
    private Vector3 _targetDir;
    private Vector3 _currentDir;
    private Quaternion _quaternionDir;
    private float _nextFire;
    private int _bossId;
    private GameObject _bulletPrefab;
    private Transform _projectileParent;
    private PlayerHealth _player;

    Okamoto _okmato;

    private void Awake()
    {
        _okmato = GetComponentInParent<Okamoto>();
        _bossId = _okmato.BossID;
    }

    private void OnEnable()
    {
        BossTrigger.OnPlayerNearBoss += ActivateTurret;
        PlayerHealth.OnPlayerDeath += PlayerisDead;
    }

    private void OnDisable()
    {
        BossTrigger.OnPlayerNearBoss -= ActivateTurret;
        PlayerHealth.OnPlayerDeath -= PlayerisDead;
    }

    private void Start()
    {
        _bulletPrefab = GetComponentInParent<Okamoto>().GetBulletPrefab();
        _player = GameObject.FindObjectOfType<PlayerHealth>();
        _projectileParent = GameObject.FindGameObjectWithTag("Projectile Parent").transform;
    }

    private void Update()
    {
        if(_isDestroyed || !_isActive || _player == null) return;

        if (_canRotate)
        {
            RotateTowardsPlayer();
        }

        if(Time.time > _nextFire)
        {
            FireCannons();
        }

    }

    private void FireCannons()
    {
        foreach (var cannon in _cannonPositions)
        {
            var laserBullet = Instantiate(_bulletPrefab, cannon.position, Quaternion.identity);
            laserBullet.GetComponent<EnemyBulletBase>().SetEnemyBulletDirection(cannon.forward * -1);
            _nextFire = Time.time + Random.Range(_fireRate.x, _fireRate.y);
        }
    }

    private void RotateTowardsPlayer()
    {
        _targetDir = _player.transform.position - transform.position;
        _targetDir = _targetDir.normalized;

        _currentDir = transform.forward;

        _currentDir = Vector3.RotateTowards(_currentDir, _targetDir, _mainTurrentRotationSpeed * Time.deltaTime, 1f);
        _quaternionDir = new Quaternion();
        _quaternionDir.SetLookRotation(_currentDir, Vector3.up);
        transform.rotation = _quaternionDir;
    }

    public void DestroyTurret()
    {
        _isDestroyed = true;
        Instantiate(_explosionVFX, transform.position, Quaternion.identity);;
        gameObject.SetActive(true);
    }

    private void PlayerisDead()
    {
        _player = null;
    }

    private void ActivateTurret(bool active, int id)
    {
        if(_bossId == id)
            StartCoroutine(ActivateTurretsRoutine(active));
    }

    IEnumerator ActivateTurretsRoutine(bool active)
    {
        yield return new WaitForSeconds(3f);
        _isActive = active;
    }
}
