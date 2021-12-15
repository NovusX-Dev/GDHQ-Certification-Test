using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTorpedoMissile : MonoBehaviour
{
    [SerializeField] float _launchSpeed = 5f;
    [SerializeField] float _attackSpeed = 15f;
    [SerializeField] Vector3 _launchPosition;
    [SerializeField] Vector2 _targetRange = new Vector2(-5f, 5f);
    [SerializeField] GameObject _explosionVFX;

    private Vector3 _offsetLuanchPosition;
    private Vector3 _playerTransformOffset;
    private bool _isLuanching = true;
    private bool _isTargeting = false;
    private WaitForSeconds _targetTime;

    PlayerHealth _player;

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
        _offsetLuanchPosition = transform.position + _launchPosition;
        _player = FindObjectOfType<PlayerHealth>();
        _targetTime = new WaitForSeconds(Random.Range(1f, 3f));
    }

    private void Update()
    {
        LaunchSequence();

        TargetPlayer();
    }

    private void LaunchSequence()
    {
        if (_isLuanching)
        {
            transform.position = Vector3.MoveTowards(transform.position, _offsetLuanchPosition, _launchSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _offsetLuanchPosition) < 0.1f)
            {
                StartCoroutine(TargetPlayerRoutine());
                _isLuanching = false;
            }
        }
    }

    private void TargetPlayer()
    {
        if (_isTargeting)
        {
            transform.LookAt(_playerTransformOffset, Vector3.forward);

            transform.position = Vector3.MoveTowards(transform.position, _playerTransformOffset, _attackSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _playerTransformOffset) < 0.1f)
                DestroyMissile();
        }
    }

    private void DestroyMissile()
    {
        Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator TargetPlayerRoutine()
    {
        yield return _targetTime;

        _playerTransformOffset = new Vector3(_player.transform.position.x + Random.Range(_targetRange.x, _targetRange.y),
                   0f, _player.transform.position.z + Random.Range(_targetRange.x, _targetRange.y));
        _isTargeting = true;
    }

    private void PlayerIsDead()
    {
        DestroyMissile();
    }

}
