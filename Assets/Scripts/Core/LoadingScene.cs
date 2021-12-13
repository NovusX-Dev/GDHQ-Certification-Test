using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] float _loadingTime = 5f;
    [SerializeField] GameObject _controlsPanel;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
        //if(LoadingData.sceneToLoad == "Main Menu")
        //{
        //    _controlsPanel.SetActive(true);
        //}
        //else
        //{
        //    _controlsPanel.SetActive(false);
        //}

        _controlsPanel.SetActive(LoadingData.sceneToLoad == "Main Menu" ? false : true);
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return new WaitForSeconds(_loadingTime);
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }

        yield return null;
    }
}
