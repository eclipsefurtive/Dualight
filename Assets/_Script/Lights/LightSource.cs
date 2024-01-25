using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script.Objects;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private CapsuleCollider _collider;
    
    [SerializeField] private float _lightRadius = 5.0f;
    private float _angleDelta = 2.0f;
    private Transform _lightSource;

    public float AngleDelta => _angleDelta;
    public float Radius => _lightRadius;
    public Vector3 Position => _lightSource.position;
    
    private void Awake()
    {
        if (!_light) _light = GetComponent<Light>();
        if (!_collider) _collider = GetComponent<CapsuleCollider>();

        _light.range = _lightRadius;
        _collider.radius = _lightRadius;
        _lightSource = transform;
    }

    private void OnEnable()
    {
        LightDetectionManager.Instance.AddLightSource(this);
    }

    private void OnDisable()
    {
        LightDetectionManager.Instance.RemoveLightSource(this);
    }
}
