using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlashLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _startDelay = 0f;
    [SerializeField] private float _onDuration = 3f;
    [SerializeField] private float _offDuration = 1.5f;
    [SerializeField] private bool _random = false;

    private float timer = 0f;

    private void Start()
    {
        PickRandomDuration();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_startDelay > 0)
        {
            _startDelay -= Time.deltaTime;
            return;
        }
        
        // update timer :
        timer += Time.deltaTime;
        if ((_light.enabled && timer >= _onDuration) || (!_light.enabled && timer >= _offDuration))
        {
            ToggleLight();
            PickRandomDuration();
        }
        
    }

    private void PickRandomDuration()
    {
        if (!_random) return;
        _onDuration = Random.Range(0f, 4f);
        _offDuration = Random.Range(0f, 4f);
    }

    private void ToggleLight()
    {
        _light.enabled = !_light.enabled;
        timer = 0f;
    }


}
