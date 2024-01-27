using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script.Objects;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Light _light;
    
    [SerializeField] private float _lightRadius = 5.0f;
    private Transform _lightSource;

    public float Radius => _lightRadius;
    public Vector3 Position => _lightSource.position;
    public bool IsLightOn => _light.isActiveAndEnabled;
    
    private void Awake()
    {
        if (!_light) _light = GetComponent<Light>();

        _light.range = _lightRadius;
        _lightSource = transform;
    }

    private void Start()
    {
        LightDetectionManager.Instance.AddLightSource(this);
    }

    private void OnDisable()
    {
        LightDetectionManager.Instance.RemoveLightSource(this);
    }

    public void SetRadius(float radius)
    {
        _lightRadius = radius;
        _light.range = radius;
    }
}
