using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Transform _playerRig;
    [SerializeField] PlayerTimelineManager _playerTimeline;
    [SerializeField] LevelCheckpointManager _levelCheckPoint;

    private int _currentDifficulty = 0;

    public int CurrentDifficulty => _currentDifficulty;

    PlayerHealth _player;

    protected override void Awake()
    {
        base.Awake();
        _player = FindObjectOfType<PlayerHealth>();
    }

    private void Start()
    {
        _currentDifficulty = LoadingData.difficultyIndex;
        Debug.Log("Current Difficulty = " + _currentDifficulty);
        Debug.Log("Loading Difficulty = " + LoadingData.difficultyIndex);
    }

    public void RespawnPlayer()
    {
        UIManager.Instance.UIFadeIN(3f);
        _playerTimeline.ResumeOnScreenShotTime(1);
        Time.timeScale = 1;
        UIManager.Instance.DeactivateDeathPanel();
        if (_player != null)
        {
            _player.RespawnPlayer();
            _playerRig.position = _levelCheckPoint.RespawnPos;
        }

    }
}
