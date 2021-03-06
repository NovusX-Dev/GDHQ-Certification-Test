using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeOutImmediate()
    {
        _canvasGroup.alpha = 1;
    }

    public IEnumerator FadeOut(float time)
    {
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
}

