using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputController _input;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _jumpForce = 5.0f;

    private void Awake()
    {
        _input ??= GetComponent<PlayerInputController>();
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
}
