using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGaurdian : MonoBehaviour
{
    [SerializeField] Vector3 _worldUp = Vector3.forward;
    [SerializeField] GameObject _bossToActivate;
    [SerializeField] int _bossID;
    [SerializeField] GameObject _portal;
    [SerializeField] Transform _spawnLocation;

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
        var portal = Instantiate(_portal, _spawnLocation.position, Quaternion.identity);
        _bossToActivate.SetActive(true);
        Destroy(portal, 3f);
    }
}
