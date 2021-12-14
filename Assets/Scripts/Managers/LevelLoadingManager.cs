using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadingManager : Singleton<LevelLoadingManager>
{
    private void Start()
    {
        AudioManager.Instance.SetMusicVolume(-15f);
    }

    public void LoadNextLevel(string levelName)
    {
        LoadingData.sceneToLoad = levelName;
        SceneManager.LoadScene("Loading Scene");
    }

    public void RestartLevel()
    {
        LoadingData.sceneToLoad = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Loading Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
