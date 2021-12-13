using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLookAtPlayer : MonoBehaviour
{
    GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.LookAt(_player.transform, Vector3.forward);
    }
}
