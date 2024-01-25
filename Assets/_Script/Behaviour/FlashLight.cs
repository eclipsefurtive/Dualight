using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _onDuration = 3f;
    [SerializeField] private float _offDuration = 1.5f;
    [SerializeField] private bool _random = false;

    private bool _isLightOn = false;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // update timer :
        timer += Time.deltaTime;

        if (_random)
        {
            _onDuration = Random.Range(0f, 4f);
            _offDuration = Random.Range(0f, 4f);
        }
        if (_isLightOn && timer > _onDuration)
            {
                _light.enabled = false;
                _isLightOn = false;
                timer = 0f;

        }


        else if(!_isLightOn && timer > _offDuration)
            {
                _light.enabled = true;
                _isLightOn = true;
                timer = 0f;
            }
        
    }


}
