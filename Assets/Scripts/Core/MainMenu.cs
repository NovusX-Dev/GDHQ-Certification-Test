using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{ 
    [SerializeField] string _levelToLoad;

    Light _mainLight;

    [Header("SFX")]
    [SerializeField] AudioClip[] _buttonSFXs;

    private void Awake()
    {
        _mainLight = GameObject.FindGameObjectWithTag("Sun").GetComponentInChildren<Light>();
    }

    private void Start()
    {
        LoadingData._gameBrightness = _mainLight.intensity;
    }

    public void StartGameButton()
    {
        AudioManager.Instance.PlayMultiSFX(_buttonSFXs);
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
