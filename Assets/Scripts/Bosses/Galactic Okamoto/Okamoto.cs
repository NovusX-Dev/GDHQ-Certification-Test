using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Okamoto : MonoBehaviour
{
    public static event Action OnOkamotoDeath;

    [SerializeField] int _killScore = 2500;
    [SerializeField] int _comboScore = 5;
    [SerializeField] int _bossID;

    [Header("Preferences")]
    [SerializeField] OkamotoBodyPart[] _bodyParts = null;
    [SerializeField] OkamotoTurret[] _turrets = null;
    [SerializeField] GameObject _explosionVFX = null;

    [Header("Pick Ups")]
    [SerializeField] Pickup[] _pickups;
    [SerializeField] Transform _pickupSpawnLocation;
    [SerializeField] int _pickupSpawnChance = 5;

    [Header("Attack")]
    [SerializeField] GameObject _bulletLaserPrefab;

    private bool _isDead = false;
    private bool _pickupSpawned = false;
    private float _currentTotalHealth;

    public int BossID => _bossID;

    private void OnEnable()
    {
        OkamotoBodyPart.OnPartDamaged += DamageOkamoto;
    }
    private void OnDisable()
    {
        OkamotoBodyPart.OnPartDamaged -= DamageOkamoto;
    }

    void Start()
    {
        for (int i = 0; i < _bodyParts.Length; i++)
        {
            CalculateOkamotoHealth(_bodyParts[i].MaxHealth);
        }
    }

    private void CalculateOkamotoHealth(float amount)
    {
        _currentTotalHealth += amount;
    }

    private void DamageOkamoto(float amount)
    {
        _currentTotalHealth -= amount;
        DestroyTurret();

        if (_currentTotalHealth > 50)
        {
            if (PickupProbability(_pickupSpawnChance))
            {
                SpawnPickup();
            }
        }

        if (_currentTotalHealth < 0)
        {
            OnOkamotoDeath?.Invoke();
            if(!_isDead)
                StartCoroutine(Death());
        }
    }

    private void SpawnPickup()
    {
        if (!_pickupSpawned)
        {
            var pickup = Instantiate(_pickups[UnityEngine.Random.Range(0, _pickups.Length)], _pickupSpawnLocation.position,
            Quaternion.Euler(90f, 0f, 0f));
            pickup.SetDirection(new Vector3(-4f, 0f, 0f));
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
        float randomProb = UnityEngine.Random.Range(1, 101);
        if (randomProb <= probability)
            return true;
        else
            return false;
    }

    private void DestroyTurret()
    {
        switch (_currentTotalHealth)
        {
            case 200:
                _turrets[0].DestroyTurret();
                break;
            case 100:
                _turrets[1].DestroyTurret();
                break;
            case 50:
                _turrets[2].DestroyTurret();
                break;
            case 20:
                _turrets[3].DestroyTurret();
                break;
        }
    }

    public GameObject GetBulletPrefab()
    {
        if (_bulletLaserPrefab != null)
        {
            return _bulletLaserPrefab;
        }
        else
            return null;
    }

    IEnumerator Death()
    {
        _isDead = true;
        _currentTotalHealth = 0;
        yield return new WaitForSeconds(3f);
        Instantiate(_explosionVFX, _pickupSpawnLocation.position, Quaternion.identity);

        ScoreManager.Instance.IncreaseKillScore(_killScore);
        ScoreManager.Instance.IncreaseCombo(_comboScore);

        GameObject.FindGameObjectWithTag("Player_Timeline").GetComponent<PlayerTimelineManager>().ResumeTimeline();
        Destroy(gameObject);
    }
}
