using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Utils;
using TMPro;
using UnityEngine;

public class MovingBehaviour : MonoBehaviour
{
    [SerializeField] private Direction _direction;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _movementDistance = 5f;

    private Vector3 _startPosition;
    private Vector3 _directionVector;

    private void Awake()
    {
        _startPosition = transform.position;
    }
    
    void Update()
    {
        _directionVector = _direction switch
        {
            Direction.Up => Vector3.up,
            Direction.Right => Vector3.right,
            Direction.Down => Vector3.down,
            Direction.Left => Vector3.left,
            _ => Vector3.right
        };
        
        Move(); 
    }

    private void Move()
    {
        float pingPongValue = Mathf.PingPong(Time.time * _moveSpeed, _movementDistance);
        transform.position = _startPosition + _directionVector * pingPongValue;
    }
}
