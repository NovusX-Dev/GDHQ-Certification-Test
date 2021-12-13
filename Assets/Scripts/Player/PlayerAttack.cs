using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] InputAction _shootWeaponIA;

    [Header("Weapon Locations")]
    [SerializeField] Transform _projectileParent;
    [SerializeField] Transform[] _weaponLevels = new Transform[3];
    
    [Header("Fire Speed")]
    [SerializeField] float _fireRate = 0.75f;
    [SerializeField] float _minFireRate = 0.25f;

    [Header("Bullet Lasers")]
    [SerializeField] GameObject _simpleLaserBullet;
    [SerializeField] GameObject _electricLaserBullet;
    [SerializeField] GameObject _ringBullet;


    private float _currentFireRate;
    private float _nextFire = 0f;
    private GameObject _currentLaserPrefab;

    public float CurrentFireRate
    {
        get => _currentFireRate;
        set => _currentFireRate = value;
    }

    PlayerLevelManager _playerLevelManager;

    private void OnEnable()
    {
        _shootWeaponIA.Enable();
    }

    private void OnDisable()
    {
        _shootWeaponIA.Disable();
    }

    private void Awake()
    {
        _playerLevelManager = GetComponent<PlayerLevelManager>();
    }

    void Start()
    {
        _currentFireRate = _fireRate;
    }

    void Update()
    {
        SetCurrentProjectile();

        if (_shootWeaponIA.ReadValue<float>() > 0.5)
        {
            if(Time.time > _nextFire)
            {
               FireWeapon(_currentLaserPrefab);
            }
        }
    }

    private void SetCurrentProjectile()
    {
        if (_playerLevelManager.GetCurrentLevel() < 4 )
        {
            _currentLaserPrefab = _simpleLaserBullet;
        }
        else if (_playerLevelManager.GetCurrentLevel() >= 4 && _playerLevelManager.GetCurrentLevel() < 7)
        {
            _currentLaserPrefab = _electricLaserBullet;
        }
        else if (_playerLevelManager.GetCurrentLevel() >= 7 )
        {
            _currentLaserPrefab = _ringBullet;
        }
    }

    private void FireWeapon(GameObject laserPrefab)
    {
        foreach(var weapon in _weaponLevels)
        { 
            if(!weapon.gameObject.activeInHierarchy) return;

            var laserBullet = Instantiate(laserPrefab, weapon.position, laserPrefab.GetComponent<BulletLaserBase>().Rotation, 
                _projectileParent);

            if(weapon == _weaponLevels[0])
            {
                laserBullet.GetComponent<BulletLaserBase>().GetForwardDirection(Vector3.right);
            }
            else
            {
                laserBullet.GetComponent<BulletLaserBase>().GetForwardDirection(weapon.forward);
            }

            laserBullet.transform.position = new Vector3(laserBullet.transform.position.x, 0, laserBullet.transform.position.z);
            _nextFire = Time.time + _currentFireRate;
        }
    }

    public void ActivateWeapons(bool level1, bool level2, bool level3)
    {
        for(int i =0; i < _weaponLevels.Length; i++)
        {
            _weaponLevels[0].gameObject.SetActive(level1);
            _weaponLevels[1].gameObject.SetActive(level2);
            _weaponLevels[2].gameObject.SetActive(level3);
        }
    }

    #region Getters
    public float GetMinimumFireRate()
    {
        return _minFireRate;
    }

    public float GetMaxFireRate()
    {
        return _fireRate;
    }
    #endregion
}
