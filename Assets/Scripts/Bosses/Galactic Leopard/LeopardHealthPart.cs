using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeopardHealthPart : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float _maxHealth = 500;
    [SerializeField] bool _isInvicible = false;

    [Header("Pick Ups")]
    [SerializeField] Pickup[] _pickups;
    [SerializeField] int _pickupSpawnChance = 5;
    [SerializeField] Vector3 _pickupSpawnDirection = new Vector3(-1f, 0f, 5f);
    
    [Header("Attack")]
    [SerializeField] bool _weaponsActiveAtStart = false;
    [SerializeField] LeopardMisslleLauncher[] _missileLaunchers;
    [SerializeField] LeopardTurret[] _turrets;

    [Header("Next Health Parts")]
    [SerializeField] LeopardHealthPart[] _nextHealthParts;

    private float _currentHealth;
    private bool _pickupSpawned;
    private bool _isDamaged = false;
    private WaitForSeconds _damagedTimer;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    Leopard _leopard;
    ColorBlinker _blinker;

    private void Awake()
    {
        _leopard = GetComponentInParent<Leopard>();
        _blinker = GetComponentInParent<ColorBlinker>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;

        if(!_weaponsActiveAtStart)
        {
            ActivateLaunchers(false);
        }
        _damagedTimer = new WaitForSeconds(0.35f);
    }

    private void DamagePart(float amount)
    {
        if(_isInvicible) return;
        _blinker.CanBlink = true;
        _currentHealth -= amount;
        _leopard.CalculatLeopardTotalHealth(amount);

        if (_currentHealth > 50)
        {
            if (PickupProbability(_pickupSpawnChance))
            {
                SpawnPickup();
            }
        }

        if (_currentHealth < 0)
        {
            ActivateLaunchers(false);
            _isInvicible = true;
            if(_turrets != null)
            {
                foreach(var turret in _turrets)
                {
                    turret.DeactivateTurrets();
                }
            }
        }
    }

    private void SpawnPickup()
    {
        if (!_pickupSpawned)
        {
            var pickup = Instantiate(_pickups[Random.Range(0, _pickups.Length)], transform.position,
            Quaternion.Euler(90f, 0f, 0f));
            pickup.SetDirection(_pickupSpawnDirection);
        }
        StartCoroutine(PickupRoutine());
    }

    IEnumerator PickupRoutine()
    {
        _pickupSpawned = true;
        yield return new WaitForSeconds(2.5f);
        _pickupSpawned = false;
    }

    private bool PickupProbability(int probability)
    {
        float randomProb = Random.Range(1, 101);
        if (randomProb <= probability)
            return true;
        else
            return false;
    }

    public void ActivateLaunchers(bool active)
    {
        if(_missileLaunchers == null) return;
        foreach (var launcher in _missileLaunchers)
        {
            launcher.EnableMissileLaunchers(active);
        }
    }

    public void DisableInvincibility()
    {
        _isInvicible = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Projectile"))
        {
            DamagePart(other.GetComponent<BulletLaserBase>().GetBulletDamage());
            Destroy(other.gameObject);
        }
    }
}
