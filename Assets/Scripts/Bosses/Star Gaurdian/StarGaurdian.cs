using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGaurdian : MonoBehaviour
{
    [SerializeField] Vector3 _worldUp = Vector3.forward;
    [SerializeField] GameObject _bossToActivate;
    [SerializeField] int _bossID;

    [Header("Portal Spawn")]
    [SerializeField] GameObject _portal;
    [SerializeField] Transform _portalSpawnLocation;

    [Header("Boss To Spawn")]
    [SerializeField] GameObject _boss;
    [SerializeField] Vector3 _bossSpawnLocation;
    [SerializeField] Quaternion _bossSpawnRotation;
    [SerializeField] Transform _bossParent;
    [SerializeField] BossTrigger _bossTrigger = null;

    private bool _followPlayer = false;
    private bool _canSpawn = true;

    GameObject _player;

    private void OnEnable()
    {
        BossTrigger.OnPlayerNearBoss += SpawnBoss;
    }

    private void OnDisable()
    {
        BossTrigger.OnPlayerNearBoss -= SpawnBoss;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(_followPlayer)
            transform.LookAt(_player.transform, _worldUp);
    }

    private void SpawnBoss(bool follow, int id)
    {
        if(id == _bossID && _canSpawn == true)
        {
            _followPlayer = follow;
            StartCoroutine(SpawnBossRoutine());
        }
    }

    IEnumerator SpawnBossRoutine()
    {
        _canSpawn = false;
        yield return new WaitForSeconds(2f);
        var portal = Instantiate(_portal, _portalSpawnLocation.position, Quaternion.identity);
        //_bossToActivate.SetActive(true);
        var boss = Instantiate(_boss, _bossSpawnLocation, _bossSpawnRotation);
        boss.transform.parent = _bossParent;
        Destroy(portal, 3f);
    }

    public void ResetStar()
    {
        _canSpawn = true;
        if (_bossTrigger != null) _bossTrigger.ResetTrigger();
    }
}
