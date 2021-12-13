using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpointManager : MonoBehaviour
{
    [SerializeField] PlayerTimelineManager _playerTimeLine;
    [SerializeField] Transform _playerRig;

    private bool _canRespawn;
    private Vector3 _respawnPosition;

    public bool CanRespawn => _canRespawn;
    public Vector3 RespawnPos => _respawnPosition;


    private void Start()
    {
        _canRespawn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canRespawn = true;
            _playerTimeLine.ScreenShotTime();
            _respawnPosition = _playerRig.position;
        }
    }
}
