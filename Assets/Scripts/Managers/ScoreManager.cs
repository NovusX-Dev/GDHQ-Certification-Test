using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private int _killScore;
    private int _currentCombo;
    private int _highestCombo = 0;
    private int _survivabilityScore;
    private int _totalScore;

    private void OnEnable()
    {
        Enemy.OnEnemyDeathCombo += IncreaseCombo;
        Enemy.OnEnemyDeathKillScore += IncreaseKillScore;
        PlayerHealth.OnPlayerReceivedDamage += ResetCombo;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeathCombo -= IncreaseCombo;
        Enemy.OnEnemyDeathKillScore -= IncreaseKillScore;
        PlayerHealth.OnPlayerReceivedDamage -= ResetCombo;
    }

    void Start()
    {
        _killScore = 0;
        UIManager.Instance.UpdateKillScoreUI(_killScore);
    }

    public void IncreaseKillScore(int score)
    {
        _killScore += score;
        UIManager.Instance.UpdateKillScoreUI(_killScore);
    }

    public void IncreaseCombo(int combo)
    {
        _currentCombo += combo;
        UIManager.Instance.UpdateComboUI(_currentCombo, true);
    }

    private void ResetCombo()
    {
        if (_highestCombo < _currentCombo)
        {
            _highestCombo = _currentCombo;
        }
        _currentCombo = 0;

        UIManager.Instance.UpdateComboUI(_currentCombo, false);

        CalculateTotalScore();
    }

    public void CalculateTotalScore()
    {
        if (_highestCombo < _currentCombo)
        {
            _highestCombo = _currentCombo;
        }

        int multiplier;
        if(_highestCombo < 11)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = _highestCombo / 10;
        }

        _totalScore = _killScore * multiplier;
    }

    #region Getters

    public int GetCurrentCombo()
    {
        return _currentCombo;
    }

    public int GetTotalScore()
    {
        CalculateTotalScore();
        return _totalScore;
    }

    public int GetKillScore()
    {
        return _killScore;
    }

    public int GetHighestCombo()
    {
        if (_highestCombo < _currentCombo)
        {
            _highestCombo = _currentCombo;
        }
        return _highestCombo;
    }

    #endregion

}
