using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlinker : MonoBehaviour
{
    [SerializeField] bool _renderersInChildren = false;
    [SerializeField] Color _startingColor = Color.white;
    [SerializeField] Color _damagedColor = Color.black;
    [SerializeField] float _blinkSpeed = 3f;
    [SerializeField] float _blinkTimer = 1f;

    private bool _canBlink;
    private WaitForSeconds _blinkYield;
    private float _currentTimer;

    public bool CanBlink
    {
        get { return _canBlink; }
        set { _canBlink = value; }
    }

    Renderer _oneRenderer = null;
    Material _oneMaterial = null;

    Renderer[] _childrenRenderers = null;
    Material[] _childrenMaterials = null;

    private Material[] Get_childrenMaterials()
    {
        return _childrenMaterials;
    }

    private void Start()
    {
        _blinkYield = new WaitForSeconds(_blinkTimer);
        _currentTimer = _blinkTimer;

        if (_renderersInChildren)
        {
            _childrenRenderers = GetComponentsInChildren<Renderer>();
            _childrenMaterials = new Material[_childrenRenderers.Length];
            for (int i = 0; i < _childrenRenderers.Length; i++)
            {
                _childrenMaterials[i] = _childrenRenderers[i].material;
            }
        }
        else
        {
            _oneRenderer = GetComponent<Renderer>();
            _oneMaterial = _oneRenderer.material;
        }
    }

    private void Update()
    {
        if (_canBlink)
        {
            if (_currentTimer > 0)
            {
                if (_renderersInChildren)
                {
                    BlinkArray();
                }
                else
                {
                    BlinkRenderer();
                }
                _currentTimer -= Time.deltaTime;
            }
            else if (_currentTimer < 0)
            {
                ChangeToOriginalColor();
                _currentTimer = _blinkTimer;
                _canBlink = false;
            }
        }
    }

    public void BlinkRenderer()
    {
        _oneMaterial.color = Color.Lerp(_startingColor, _damagedColor, Mathf.PingPong(Time.time * _blinkSpeed, 1));

    }

    public void BlinkArray()
    {
        foreach (var mat in _childrenMaterials)
        {
            mat.color = Color.Lerp(_startingColor, _damagedColor, Mathf.PingPong(Time.time * _blinkSpeed, 1));
        }


    }

    private void ChangeToOriginalColor()
    {
        if (_oneMaterial != null) _oneMaterial.color = Color.white;

        if (_childrenMaterials != null)
        {
            foreach (var mat in _childrenMaterials)
            {
                mat.color = Color.white;
            }
        }
    }

}
