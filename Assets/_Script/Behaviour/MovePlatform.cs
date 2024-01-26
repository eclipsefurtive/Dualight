using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _movementDistance = 5f;

    private Vector3 _startPosition;
   // private Vector3 _endPosition;
    private float _direction = 1;

   
    private void Awake()
    {
        _startPosition = transform.position;
    }
    void Start()
    {

    }

    void Update()
    {
        Move(); 
    }

    private void Move()
    {

        float pingPongValue = Mathf.PingPong(Time.time * _moveSpeed / _movementDistance, 1f);
        Vector3 newPosition = _startPosition + Vector3.right * _movementDistance * pingPongValue;

        transform.position = newPosition;

        /*
        //the new position
        Vector3 newPosition = transform.position + Vector3.right * (_direction * _moveSpeed * Time.deltaTime);
        // End position is ok?
        if (Mathf.Abs(newPosition.x)
        */
    }

    /*private void Move(float horizontalValue)
    {
        if (horizontalValue == 0) return;
        transform.position += transform.forward * (horizontalValue * _moveSpeed * Time.deltaTime);
    }*/
}
