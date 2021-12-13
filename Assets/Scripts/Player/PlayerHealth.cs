using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<float> OnPlayerDamagedInt;
    public static event Action OnPlayerReceivedDamage;
    public static event Action<int> OnPlayerDeath;

    [SerializeField] float _maxHealth = 6;
    [SerializeField] int _maxRespawnLives = 1;

    [Header("References")]
    [SerializeField] GameObject _explosionVFX;

    private float _currentHealth;
    private int _currentRespawnLives;

    ColorBlinker _blinker;

    private void Awake()
    {
        _blinker = GetComponent<ColorBlinker>();
    }

    void Start()
    {
        switch (GameManager.Instance.CurrentDifficulty)
        {
            case 0:
                _maxHealth = 12;
                break;
            case 1:
                _maxHealth = 6;
                break;
        }

        _currentHealth = _maxHealth;
        _currentRespawnLives = _maxRespawnLives;
        OnPlayerDamagedInt?.Invoke(_currentHealth);
    }

    void Update()
    {
        //remove later
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E))
        {
            DamagePlayer(1.25f);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentHealth = _maxHealth;
        }
#endif
    }

    public void DamagePlayer(float amount)
    {
        _currentHealth -= amount;
        OnPlayerReceivedDamage?.Invoke();
        OnPlayerDamagedInt?.Invoke(_currentHealth);
        _blinker.CanBlink = true;

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    public void HealPlayer(float amount)
    {
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        OnPlayerDamagedInt?.Invoke(_currentHealth);
    }

    private void Death()
    {
        OnPlayerDamagedInt?.Invoke(_currentHealth);
        FindObjectOfType<PlayerTimelineManager>().PauseTimeline();
        _currentHealth = 0;
        Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        OnPlayerDeath?.Invoke(_currentRespawnLives);
        gameObject.SetActive(false);
    }

    public void RespawnPlayer()
    {
        if (_currentRespawnLives > 0) _currentRespawnLives--;

        _currentHealth = _maxHealth;
        OnPlayerDamagedInt?.Invoke(_currentHealth);
        gameObject.SetActive(true);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Projectile"))
        {
            DamagePlayer(other.GetComponent<EnemyBulletBase>().GetBulletDamage());
            Destroy(other.gameObject);
        }
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }


}
