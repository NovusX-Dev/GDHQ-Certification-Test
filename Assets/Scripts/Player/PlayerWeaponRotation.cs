using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponRotation : MonoBehaviour
{
    [SerializeField] Vector2 _clampedRotation;
    [SerializeField] bool _isAbove;

    private float _rotationY;

    Camera _cam;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = 0;
        transform.LookAt(mousePos);

        _rotationY = transform.eulerAngles.y;
        if(_isAbove) _rotationY = (_rotationY > 180f) ? _rotationY - 360 : _rotationY;
        _rotationY = Mathf.Clamp(_rotationY, _clampedRotation.x, _clampedRotation.y);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, _rotationY, transform.eulerAngles.z);

        

    }
}
