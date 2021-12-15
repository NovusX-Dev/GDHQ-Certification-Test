using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerTimelineManager : MonoBehaviour
{
    private double _screenShotTime;

    PlayableDirector _playerTimeline;

    private void Awake()
    {
        _playerTimeline = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        BossTrigger.OnPlayerNearBoss += PauseTimeline;
    }

    private void OnDisable()
    {
        BossTrigger.OnPlayerNearBoss -= PauseTimeline;
    }

    public void PauseTimeline()
    {
        _playerTimeline.Pause();
    }

    public void ScreenShotTime()
    {
        _screenShotTime = _playerTimeline.time;
    }

    public void ResumeTimeline(float waitTime)
    {
        StartCoroutine(ResumeRoutine(waitTime));
    }

    public void ResumeOnScreenShotTime(float waitTime)
    {
        _playerTimeline.time = _screenShotTime;
        StartCoroutine(ResumeRoutine(waitTime));
    }

    IEnumerator ResumeRoutine(float waitTime)
    {
        if(waitTime > 0)
        {
            yield return new WaitForSeconds(waitTime);
            _playerTimeline.Play();
        }
        else
        {
            yield return null;
            _playerTimeline.Play();
        }
        
    }

    private void PauseTimeline(bool wait, int waitTime)
    {
        StartCoroutine(PauseRoutine(wait, waitTime));
    }

    IEnumerator PauseRoutine(bool wait, int waitTime)
    {
        if(wait)
        {
            yield return new WaitForSeconds(((float)waitTime - (float)waitTime) + 3f);
        }
        else
        {
            yield return null;
        }
        _playerTimeline.Pause();
    }
}
