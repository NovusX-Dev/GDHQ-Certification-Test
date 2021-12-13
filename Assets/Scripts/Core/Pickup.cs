using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum PickupType {Upgrade, Health};

    [SerializeField] PickupType _type;
    [SerializeField] AudioClip[] _audioClips;

    private Vector3 _direction = Vector3.left;

    private void Update()
    {
        transform.Translate(_direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_type == PickupType.Upgrade)
            {
                other.GetComponent<PlayerLevelManager>().UpgradeLevel(1);
            }
            else if (_type == PickupType.Health)
            {
                other.GetComponent<PlayerHealth>().HealPlayer(1);
            }

            AudioManager.Instance.PlayMultiSFX(_audioClips);
            Destroy(gameObject);
        }

    }

    public void SetDirection(Vector3 dir)
    {
        _direction = dir;
    }
}
