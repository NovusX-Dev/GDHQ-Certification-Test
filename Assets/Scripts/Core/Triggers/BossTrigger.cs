using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public static event Action<bool, int> OnPlayerNearBoss;

    [SerializeField] bool _activation; //used if the receiver needs a bool to activate
    [SerializeField] int _id;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPlayerNearBoss?.Invoke(_activation, _id);
            GetComponent<Collider>().enabled = false;
        }
    }
}
