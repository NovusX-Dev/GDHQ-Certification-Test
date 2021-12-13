using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{

    [SerializeField] bool _isLeftSide = false;

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject, 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground_Turret") && _isLeftSide)
        {
            Destroy(other.gameObject, 0.25f);
        }
    }
}
