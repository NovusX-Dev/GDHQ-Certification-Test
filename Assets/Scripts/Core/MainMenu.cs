using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{ 
    [SerializeField] string _levelToLoad;

    [Header("SFX")]
    [SerializeField] AudioClip[] _buttonSFXs;

    Light _mainLight;
    Fader _fader;

    private void Awake()
    {
        _mainLight = GameObject.FindGameObjectWithTag("Sun").GetComponentInChildren<Light>();
        _fader = GetComponentInChildren<Fader>();
    }

    private void Start()
    {
        LoadingData._gameBrightness = _mainLight.intensity;
        StartCoroutine(_fader.FadeIn(1f));
    }

    public void StartGameButton()
    {
        //AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        //LevelLoadingManager.Instance.LoadNextLevel(_levelToLoad);
        StartCoroutine(LoadLevelRoutine());
    }

    IEnumerator LoadLevelRoutine()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        yield return _fader.FadeOut(1f);
        LevelLoadingManager.Instance.LoadNextLevel(_levelToLoad);
    }

    public void SetDifficulty(int difficulty)
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
        LoadingData.difficultyIndex = difficulty;
    }

    public void SetBrightness(float bright)
    {
        _mainLight.intensity = bright;
        LoadingData._gameBrightness = bright;
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}
