using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeopardTurret : MonoBehaviour
{
    [SerializeField] Transform _firePosition;
    [SerializeField] Vector2 _fireRate = new Vector2(1f, 3f);
    [SerializeField] float _shootingTime = 10f;
    [SerializeField] float _cooldownTime = 5f;
    [SerializeField] GameObject _smokeVFX;

    private bool _isActive;
    private GameObject _bulletPrefab = null;
    private Transform _projectileParent;
    private float _nextFire;
    private bool _canFire = true;
    private bool _isCoolingDown = false;
    private float _currentShootingTime;
    private float _currentCooldownTime;
    private Quaternion _originalRot;

    PlayerHealth _player;

    private void OnEnable()
    {
        _originalRot = transform.rotation;
        StartCoroutine(ActivateTurretsRoutine(true));
    }

    private void Start()
    {
        _bulletPrefab = GetComponentInParent<Leopard>().GetBulletPrefab();
        _player = GameObject.FindObjectOfType<PlayerHealth>();
        _projectileParent = GameObject.FindGameObjectWithTag("Projectile Parent").transform;
        _currentShootingTime = _shootingTime;
        _currentCooldownTime = _cooldownTime;
    }

    private void Update()
    {
        if (!_isActive || _player == null) return;

        if (!_isCoolingDown)
        {
            CoolDownTime();
        }
        else
        {
            ShootingTime();
        }

        if (Time.time > _nextFire && _canFire)
        {
            FireTurret();
        }
    }

    private void CoolDownTime()
    {
        transform.LookAt(_player.transform.position, Vector3.forward);

        _smokeVFX.SetActive(false);
        _canFire = true;

        _currentShootingTime -= Time.deltaTime;
        if (_currentShootingTime < 0)
        {
            _isCoolingDown = true;
            _currentShootingTime = _shootingTime;
        }
    }

    private void ShootingTime()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _originalRot, 360 * Time.deltaTime);

        _canFire = false;
        _smokeVFX.SetActive(true);

        _currentCooldownTime -= Time.deltaTime;
        if (_currentCooldownTime < 0)
        {
            _isCoolingDown = false;
            _currentCooldownTime = _cooldownTime;
            _nextFire = Time.time + 0.25f;
        }
    }

    private void FireTurret()
    {
        var laserBullet = Instantiate(_bulletPrefab, _firePosition.position, Quaternion.identity);
        laserBullet.GetComponent<EnemyBulletBase>().SetEnemyBulletDirection(transform.forward * -1);
        _nextFire = Time.time + Random.Range(_fireRate.x, _fireRate.y);
    }

    IEnumerator ActivateTurretsRoutine(bool active)
    {
        yield return new WaitForSeconds(3f);
        _isActive = active;
        _nextFire = Time.time + 1f;
    }

    public void DeactivateTurrets()
    {
        _isActive = false;
    }
}
