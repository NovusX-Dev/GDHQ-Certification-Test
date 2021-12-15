using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public static event Action<bool, int> OnPlayerNearBoss;

    [SerializeField] bool _activation; //used if the receiver needs a bool to activate
    [SerializeField] int _id;

    Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPlayerNearBoss?.Invoke(_activation, _id);
            _collider.enabled = false;
        }
    }

    public void ResetTrigger()
    {
        _collider.enabled = true;
    }
}
