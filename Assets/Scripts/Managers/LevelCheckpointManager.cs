using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpointManager : MonoBehaviour
{
    [SerializeField] PlayerTimelineManager _playerTimeLine;
    [SerializeField] Transform _playerRig;

    private bool _canRespawn;
    private Vector3 _respawnPosition;
    private LevelCheckpointManager[] _allCheckPoints;

    public bool CanRespawn => _canRespawn;
    public Vector3 RespawnPos => _respawnPosition;


    private void Start()
    {
        _canRespawn = false;
        _allCheckPoints = FindObjectsOfType<LevelCheckpointManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this._canRespawn = true;
            _playerTimeLine.ScreenShotTime();
            this._respawnPosition = _playerRig.position;

            for (int i = 0; i < _allCheckPoints.Length; i++)
            {
                if (_allCheckPoints[i] == this) continue;
                else
                {
                    if (_allCheckPoints[i]._canRespawn)
                    {
                        _allCheckPoints[i]._canRespawn = false;
                    }
                }
            }
        }
    }


}
