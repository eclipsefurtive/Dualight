using _Script.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerReceiveLight : MonoBehaviour, ILightBehaviour
{
    [SerializeField] private Renderer _platformRenderer;
    [SerializeField] private Material _darkMaterial;
    [SerializeField] private Material _lightMaterial;

    [SerializeField] private bool _timerEnabled = false;
    [SerializeField] private float _timerDuration = 5f;

    private bool _isLighting;

    public void Update()
    {
        if (_timerEnabled)
        {
            _timerDuration -= Time.deltaTime;
            if (_timerDuration <= 0)
            {
                _timerEnabled = false;
                TimerEnd();
            }
        }
    }

    public void OnEnterLight()
    {
        if (!_isLighting)
        {
            _platformRenderer.material = _lightMaterial;
            _isLighting = true;
            _timerEnabled = true;
        }
    }

    public void OnExitLight()
    {
    }

    private void Awake()
    {
        _platformRenderer ??= GetComponent<Renderer>();
    }

     private void TimerEnd()
    {
        _platformRenderer.material = _darkMaterial;
        _isLighting = false;
        _timerDuration = 5f;
    }
}
