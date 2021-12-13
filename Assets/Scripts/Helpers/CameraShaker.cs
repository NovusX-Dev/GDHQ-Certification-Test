using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float _weakShakeTimer;
    [SerializeField] float _weakShakeIntensity;
    [SerializeField] float _strongShakeTimer;
    [SerializeField] float _strongShakeIntensity;

    private float _newShakeIntensity;
    private float _shakeTimer;
    private float _totalShakeTimer;

    CinemachineVirtualCamera _vCam;
    CinemachineBasicMultiChannelPerlin _multichaneelPerling;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerReceivedDamage += WeakCamerShake;
        Okamoto.OnOkamotoDeath += StrongCameraShake;
        Leopard.OnLeopardDeath += StrongCameraShake;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerReceivedDamage -= WeakCamerShake;
        Okamoto.OnOkamotoDeath -= StrongCameraShake;
        Leopard.OnLeopardDeath -= StrongCameraShake;
    }

    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _multichaneelPerling = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if(_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            _multichaneelPerling.m_AmplitudeGain = Mathf.Lerp(_newShakeIntensity, 0, (1 - (_shakeTimer / _totalShakeTimer)));
        }
    }

    public void WeakCamerShake()
    {
        _multichaneelPerling.m_AmplitudeGain = _weakShakeIntensity;

        _shakeTimer = _weakShakeTimer;
        _shakeTimer = _weakShakeTimer;
        _newShakeIntensity = _weakShakeIntensity;
    }

    public void StrongCameraShake()
    {
        _multichaneelPerling.m_AmplitudeGain = _strongShakeIntensity;

        _shakeTimer = _strongShakeTimer;
        _shakeTimer = _strongShakeTimer;
        _newShakeIntensity = _strongShakeIntensity;
    }
}
