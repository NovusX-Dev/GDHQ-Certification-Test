using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkamotoBodyPart : MonoBehaviour
{
    public static event Action<float> OnPartDamaged;

    [SerializeField] float _maxHealth;
    [SerializeField] bool _isInvincible = false;
    [SerializeField] GameObject _fireVFX;
    [SerializeField] Transform[] _firePositions;

    private float _currentHealth;
    private bool _isDestroyed = false;
    private bool _isDamaged = false;
    private WaitForSeconds _damagedTimer;

    public bool IsDestroyed => _isDestroyed;
    public float MaxHealth => _maxHealth;

    ColorBlinker _blinker;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _damagedTimer = new WaitForSeconds(0.35f);
        _blinker = GetComponent<ColorBlinker>();
    }

    private void DamagePart(float amount)
    {
        if (_isInvincible || _isDestroyed) return;
        OnPartDamaged?.Invoke(amount);
        _blinker.CanBlink = true;

        _currentHealth -= amount;
        _isDamaged = true;

        if (_currentHealth < 0)
        {
            _isDestroyed = true;
            foreach (var firePos in _firePositions)
            {
                var explosion = Instantiate(_fireVFX, firePos.position, Quaternion.identity);
                explosion.transform.parent = this.transform;
            }
        }
    }
    public bool IsPartInvincible(bool invincible)
    {
        return _isInvincible = invincible;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
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
