using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaExplosion : MonoBehaviour
{
    [SerializeField] float _damage = 2f;
    [SerializeField] AudioClip _clip = null;

    ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(-90f,0f,0f);
        if(_clip != null)
            AudioSource.PlayClipAtPoint(_clip, transform.position);

        Destroy(gameObject, 3f);
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.TryGetComponent(out PlayerHealth player))
            {
                player.DamagePlayer(_damage);
                var col = _particle.collision;
                col.enabled = false;
            }
        }
    }
}
