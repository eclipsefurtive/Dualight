using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Objects;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, ILightBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _darkMaterial;
    [SerializeField] private Material _lightMaterial;
    
    [SerializeField] private PlayerInputController _input;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _jumpForce = 5.0f;

    private void Awake()
    {
        _input ??= GetComponent<PlayerInputController>();
        _renderer ??= GetComponent<Renderer>();
        
        _input.OnJump += Jump;
    }

    private void Update()
    {
        Move(_input.HorizontalMovementValue);
    }

    private void Move(float horizontalValue)
    {
        if (horizontalValue == 0) return;
        transform.position += transform.forward * (horizontalValue * _moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        _rigidbody.AddRelativeForce(Vector3.up * _jumpForce);
    }

    public void OnEnterLight()
    {
        _renderer.material = _lightMaterial;
    }

    public void OnExitLight()
    {
        _renderer.material = _darkMaterial;
    }
}
