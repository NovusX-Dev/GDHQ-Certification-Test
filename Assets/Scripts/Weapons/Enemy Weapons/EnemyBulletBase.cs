using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : MonoBehaviour
{
    [SerializeField] LaserScriptableObject _laserSO;

    private float _currentPower;
    private Vector3 _enemyDirection;
    private Vector3 _direction;
    private float _currentProjectileSpeed;

    public Quaternion Rotation => _laserSO._rotation;

    private void Start()
    {
        _currentPower = _laserSO._bulletDamage;
        _currentProjectileSpeed = _laserSO._bulletSpeed; ;
        transform.parent = GameObject.FindGameObjectWithTag("Projectile Parent").transform;
    }

    private void Update()
    {
        ProcessProjectileSpeed();
    }

    private void ProcessProjectileSpeed()
    {
        _direction = _enemyDirection * _currentProjectileSpeed * Time.deltaTime;
        transform.position += _direction;
    }

    public float GetBulletDamage()
    {
        return _currentPower;
    }

    public void MultiplyProjectileSpeed(float multiplier)
    {
        _currentProjectileSpeed *= multiplier;
    }

    public Vector3 SetEnemyBulletDirection(Vector3 direction)
    {
        return _enemyDirection = direction;
    }
}
