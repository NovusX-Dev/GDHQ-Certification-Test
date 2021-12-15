using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leopard : MonoBehaviour
{
    public static event Action OnLeopardDeath;

    [SerializeField] int _bossID;

    [Header("Fire VFX")]
    [SerializeField] GameObject[] _firstSet;
    [SerializeField] GameObject[] _secondSet;
    [SerializeField] GameObject[] _thirdSet;
    [SerializeField] GameObject[] _fourthSet;

    [Header("Death")]
    [SerializeField] GameObject _smallExplosionVFX;
    [SerializeField] GameObject _bigExplosionVFX;
    [SerializeField] AudioClip _winSFX;
    [SerializeField] Transform _explosionPosition;
    [SerializeField] int _killScore = 25000;
    [SerializeField] int _comboScore = 5;

    [Header("References")]
    [SerializeField] GameObject _bulletLaserPrefab;
    [SerializeField] LeopardHealthPart[] _healthParts;
    [SerializeField] GameObject _stage1Parent;
    [SerializeField] GameObject[] _stage1Parts;
    [SerializeField] LeopardHealthPart[] _stage2Parts;

    private float _currentTotalHealth;
    private float _startingTotalHealth;
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += PlayerIsDead;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayerIsDead;
    }

    private void Start()
    {
        for (int i = 0; i < _healthParts.Length; i++)
        {
            _currentTotalHealth += _healthParts[i].MaxHealth;
        }
        _startingTotalHealth = _currentTotalHealth;
    }

    public void CalculatLeopardTotalHealth(float amount)
    {
        _currentTotalHealth -= amount;

        if (_currentTotalHealth < 501)
        {
            _animator.SetTrigger("stage_1_explosion");
        }

        StartFireVFX(800, _firstSet);
        StartFireVFX(650, _secondSet);
        StartFireVFX(350, _thirdSet);
        StartFireVFX(100, _fourthSet);

        if (_currentTotalHealth < 1)
        {
            OnLeopardDeath?.Invoke();
            _animator.SetTrigger("death");
        }
    }

    public void EnableStage1Explosions()
    {
        foreach (var part in _stage1Parts)
        {
            //part.SetActive(false);
            Instantiate(_smallExplosionVFX, part.transform.position, Quaternion.identity);
        }

        _stage1Parent.SetActive(false);
    }

    public void StartStage2()
    {
        foreach (var part in _stage2Parts)
        {
            part.DisableInvincibility();
            part.ActivateLaunchers(true);
        }
    }

    private void StartFireVFX(float health, GameObject[] set)
    {
        if (_currentTotalHealth <= health)
        {
            foreach (var vfx in set)
            {
                if (!vfx.activeInHierarchy)
                {
                    vfx.SetActive(true);
                }
            }
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

    private void ResetBoss()
    {
        _animator.SetTrigger("reset");
        _stage1Parent.SetActive(false);
        _currentTotalHealth = _startingTotalHealth;
        foreach (var part in _stage1Parts)
        {
            if(part.GetComponent<LeopardHealthPart>() != null)
            {
                part.GetComponent<LeopardHealthPart>().ResetHealthPart();
            }
        }
    }

    private void PlayerIsDead()
    {
        FindObjectOfType<StarGaurdian>().ResetStar();
        Destroy(gameObject, 3f);
        //gameObject.SetActive(false);
    }

    public IEnumerator Death()
    {
        AudioManager.Instance.PlayMusic(_winSFX);
        yield return new WaitForSeconds(3f);
        _smallExplosionVFX.SetActive(true);

        yield return new WaitForSeconds(3f);
        Instantiate(_bigExplosionVFX, _explosionPosition.position, Quaternion.identity);
        ScoreManager.Instance.IncreaseKillScore(_killScore);
        ScoreManager.Instance.IncreaseCombo(_comboScore);
        GameObject.FindGameObjectWithTag("Player_Timeline").GetComponent<PlayerTimelineManager>().ResumeTimeline(3);
        Destroy(gameObject);
    }
}
