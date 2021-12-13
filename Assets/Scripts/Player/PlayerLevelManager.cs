using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public static event Action<int> OnPlayerLevelUp;

    [Header("Levels")]
    [SerializeField] int _minLevel = 0;
    [SerializeField] int _maxLevel = 10;
    [SerializeField] float _fireRateMultiplier = 0.85f;

    private int _currentLevel;
    private bool _upgraded = false;

    PlayerController _playerController;
    PlayerAttack _playerAttack;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerReceivedDamage += DegradeLevel;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerReceivedDamage -= DegradeLevel;
    }

    void Start()
    {
        _currentLevel = _minLevel;
        CheckLevel();
    }

    void Update()
    {
        //remove later
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Q))
        {
            UpgradeLevel(1);
        }
        #endif
    }

    private void CheckLevel()
    {
        //var minFireRate = _playerAttack.GetMinimumFireRate();
        switch (_currentLevel)
        {
            case 1:
                break;
            case 2:
                MultiplyFireRate();
                _upgraded = true;
                break;
            case 3:
                MultiplyFireRate();
                _upgraded = true;
                break;
            case 4:
                MultiplyFireRate();
                _upgraded = true;
                break;
            case 5:
                MultiplyFireRate();
                _upgraded = true;
                break;
            case 6:
                MultiplyFireRate();
                _upgraded = true;
                break;
            case 7:
                MultiplyFireRate();
                _playerController.IncreaseFlySpeed();
                _upgraded = true;
                break;
            case 8:
                MultiplyFireRate();
                _upgraded = true;
                break;
            case 9:
                MultiplyFireRate();
                _upgraded = true;
                break;
        }

        if (_currentLevel < 4)
        {
            _playerAttack.ActivateWeapons(true, false, false);
        }
        else if (_currentLevel < 7)
        {
            _playerController.ResestFlySpeed();
            _playerAttack.ActivateWeapons(true, true, false);
        }
        else 
        {
            _playerAttack.ActivateWeapons(true, true, true);
        }

        OnPlayerLevelUp?.Invoke(_currentLevel);
    }

    private void MultiplyFireRate()
    {
        var currentFireRate = _playerAttack.CurrentFireRate;
        var minFireRate = _playerAttack.GetMinimumFireRate();

        if(_upgraded) return;

        currentFireRate *= _fireRateMultiplier;
        if(currentFireRate <= minFireRate)
        {
            currentFireRate = minFireRate;
        }

        _playerAttack.CurrentFireRate = currentFireRate;
    }

    public void UpgradeLevel(int amount)
    {
        _currentLevel += amount;
        if (_currentLevel > _maxLevel)
        {
            _currentLevel = _maxLevel;
        }

        _upgraded = false;
        CheckLevel();
    }

    private void DegradeLevel()
    {
        _currentLevel -= 3;
        if (_currentLevel <= 1)
        {
            _currentLevel = 1;
        }

        _playerAttack.CurrentFireRate *= 1.7f;
        if (_playerAttack.CurrentFireRate >= _playerAttack.GetMaxFireRate())
        {
            _playerAttack.CurrentFireRate = _playerAttack.GetMaxFireRate();
        }
        _upgraded = false;

        CheckLevel();
    }

    #region Getters
    public int GetCurrentLevel()
    {
        return _currentLevel;
    }
    #endregion
}
