using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction _movement;

    [Header("Movement")]
    [SerializeField] float _flySpeed = 5f;
    [SerializeField] float _controlRollFactor = 2f;
    [SerializeField] float _xMinRange, _xMaxRange, _minZRange, _maxZRange;

    [Header("Thursters")]
    [SerializeField] ParticleSystem _leftThurster;
    [SerializeField] ParticleSystem _rightThurster;
    [SerializeField] float _minThursterEmission, _defaultThursterEmission, _maxThursterEmission;
   
    private float _currentFlySpeed;
    private float _xFly;
    private float _zFly;

    private void OnEnable()
    {
        _movement.Enable();
    }

    private void OnDisable()
    {
        _movement.Disable();
    }

    void Start()
    {
        ResestFlySpeed();
    }

    void Update()
    {
        ProcessMovement();
        ProcessRoll();
        ProcessThursters();
    }

    private void ProcessMovement()
    {
        _xFly = _movement.ReadValue<Vector2>().x;
        _zFly = _movement.ReadValue<Vector2>().y;

        float xOffset = _xFly * _currentFlySpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, _xMinRange, _xMaxRange);

        float zOffset = _zFly * _currentFlySpeed * Time.deltaTime;
        float rawZPos = transform.localPosition.z + zOffset;
        float clampedZPos = Mathf.Clamp(rawZPos, _minZRange, _maxZRange);

        transform.localPosition = new Vector3(clampedXPos, 0, clampedZPos);
    }

    private void ProcessRoll()
    {
        float roll = _zFly * _controlRollFactor;
        transform.localRotation = Quaternion.Euler(roll, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }

    private void ProcessThursters()
    {
        if(_xFly > 0.5f)
        {
            _leftThurster.emissionRate = _maxThursterEmission;
            _rightThurster.emissionRate = _maxThursterEmission;
        }
        else if(_xFly < -0.5f)
        {
            _leftThurster.emissionRate = _minThursterEmission;
            _rightThurster.emissionRate = _minThursterEmission;
        }
        else
        {
            _leftThurster.emissionRate = _defaultThursterEmission;
            _rightThurster.emissionRate = _defaultThursterEmission;
        }
    }

    public void IncreaseFlySpeed()
    {
        _currentFlySpeed += 7.5f;
    }

    public void ResestFlySpeed()
    {
        _currentFlySpeed = _flySpeed;    
    }
    
}
