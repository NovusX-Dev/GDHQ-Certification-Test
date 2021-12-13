using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] float _timer = 3f;
    [SerializeField] GameObject _explosionVFX;
    [SerializeField] AudioClip _explosionSFX;

    WaitForSeconds _waitForSeconds;

    void Start()
    {
        _waitForSeconds = new WaitForSeconds(_timer);
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return _waitForSeconds;

        if(_explosionVFX != null) Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        if(_explosionSFX != null) AudioSource.PlayClipAtPoint(_explosionSFX, transform.position);

        Destroy(gameObject);
    }
}
