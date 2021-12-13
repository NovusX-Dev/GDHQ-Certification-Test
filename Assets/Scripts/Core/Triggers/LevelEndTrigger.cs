using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public static event Action<string> OnLevelEnded;

    [SerializeField] string _levelToLoad;
    [SerializeField] AudioClip _winSong;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnLevelEnded?.Invoke(_levelToLoad);
            AudioManager.Instance.PlayMusic(_winSong);
        }
    }
}
