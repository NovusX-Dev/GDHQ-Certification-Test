using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomLegacy : Enemy
{
    [Header("Legacy Specific")]
    [SerializeField] Vector2 _yMoveRange = new Vector2(-12, 17);
    [SerializeField] float _roationSpeed = 5f;
    [SerializeField] GameObject _energySource = null;

    private bool _canShoot = false;
    private bool _invincible = true;
    private Vector3 _moveDirection;
    private WaitForSeconds _shootWaitforSeconds;
    private Quaternion _startRotation;

    GameObject _player;

    protected override void Start()
    {
        base.Start();
        _shootWaitforSeconds = new WaitForSeconds(4f);
        _player = GameObject.FindGameObjectWithTag("Player");
        _startRotation = transform.rotation;
    }

    protected override void Update()
    {
        base.Update();

        if (_inPlayableArea)
        {
            transform.parent = GameObject.FindGameObjectWithTag("Projectile Parent").transform;
        }

        RaycastHit hit;

        if(Physics.Raycast(transform.position + new Vector3(1f, 0f,0f), Vector3.right, out hit, 20f))
        {
            if (hit.collider.GetComponent<PhantomLegacy>() != null && hit.collider != this)
            {
                DestroyEnemy();
            }
        }
    }

    protected override void Move()
    {
        if (transform.localPosition.z == _yMoveRange.x)
        {
            StartCoroutine(MoveRoutine(_yMoveRange.y));
            RotateTowardsPlayer();
        }
        else if (transform.localPosition.z == _yMoveRange.y)
        {
            StartCoroutine(MoveRoutine(_yMoveRange.x));
            RotateTowardsPlayer();
        }
        else
        {
            _canShoot = false;
            _invincible = false;
        }
    }

    public override void DamageEnemy(float amount)
    {
        if (_invincible) return;
        base.DamageEnemy(amount);
    }

    protected override void FireWeapon()
    {
        if (!_canShoot) return;
        if (_laserPrefab == null) return;
        var laserBullet = Instantiate(_laserPrefab, _weaponPos.position, _enemyBulletBase.Rotation, _projectileParent);
        laserBullet.GetComponent<EnemyBulletBase>().SetEnemyBulletDirection(transform.forward * -1);
        laserBullet.transform.position = new Vector3(laserBullet.transform.position.x, 0, laserBullet.transform.position.z);
        _nextFire = Time.time + UnityEngine.Random.Range(_minFireRate, _fireRate);
    }

    private void RotateTowardsPlayer()
    {
        var targetDir = _player.transform.position - transform.position;
        targetDir = targetDir.normalized;
        var currentDir = transform.forward;
        currentDir = Vector3.RotateTowards(currentDir, targetDir, _roationSpeed * Time.deltaTime, 1f);
        var quaternionDir = new Quaternion();
        quaternionDir.SetLookRotation(currentDir, Vector3.forward);
        transform.localRotation = quaternionDir;
    }

    IEnumerator MoveRoutine(float zPos)
    {
        _invincible = true;
        _canShoot = true;
        _energySource.SetActive(true);

        yield return _shootWaitforSeconds;

        _energySource.SetActive(false);
        _moveDirection = transform.localPosition;
        _moveDirection.z = zPos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _moveDirection, _moveSpeed * Time.deltaTime);
    }
}
