using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BulletLaserBase : MonoBehaviour
{
    [SerializeField] private LaserScriptableObject _laserSO;

    private float _currentPower;
    private float _currentProjectileSpeed;
    private Vector3 _direction;
    private Vector3 _forwardDirection;
    private float _damageDificultyMultiplier;

    public Quaternion Rotation => _laserSO._rotation;

    private void Start()
    {
        _currentPower = _laserSO._bulletDamage;
        _currentProjectileSpeed = _laserSO._bulletSpeed;

        switch (GameManager.Instance.CurrentDifficulty)
        {
            case 0:
                _damageDificultyMultiplier = 2f;
                break;
            case 1:
                _damageDificultyMultiplier = 1;
                break;
        }

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        ProcessProjectileSpeed();
    }

    private void ProcessProjectileSpeed()
    {
        _direction = _forwardDirection * _currentProjectileSpeed * Time.deltaTime;
        transform.position += _direction;
    }

    public float GetBulletDamage()
    {
        return _currentPower * _damageDificultyMultiplier;
    }

    public void MultiplyProjectileSpeed(float multiplier)
    {
        _currentProjectileSpeed *= multiplier;
    }

    public void GetForwardDirection(Vector3 direction)
    {
        _forwardDirection = direction;
    }

}
