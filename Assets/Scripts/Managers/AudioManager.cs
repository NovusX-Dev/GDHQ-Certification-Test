using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioMixer _mainMixer;
    [SerializeField] AudioSource _SFXSource;

    [Header("Music")]
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioClip _musicToPlayOnLevelStart;


    private void Start()
    {
        PlayMusic(_musicToPlayOnLevelStart);
        _mainMixer.SetFloat("musicVol", LoadingData._musicVolume);
        _mainMixer.SetFloat("sfxVol", LoadingData._sfxVolume);
    }

    public void PlayMultiSFX(AudioClip[] clips)
    {
        _SFXSource.clip = clips[Random.Range(0, clips.Length)];
        _SFXSource.Play();
    }

    public void PlaySingleSFX(AudioClip clip)
    {
        _SFXSource.clip = clip;
        _SFXSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = null;
        _musicSource.clip = _musicToPlayOnLevelStart;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        _mainMixer.SetFloat("musicVol", volume);
        LoadingData._musicVolume = volume;
    }

    public void SetSEFXVolume(float volume)
    {
        _mainMixer.SetFloat("sfxVol", volume);
        LoadingData._sfxVolume = volume;
    }
}
