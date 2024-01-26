using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Objects;
using _Script.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, ILightBehaviour
{
    public enum State
    {
        Dark = 0,
        Light = 1
    }

    [SerializeField] private int _currentState = 0;
    
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _darkMaterial;
    [SerializeField] private Material _lightMaterial;
    
    [SerializeField] private PlayerInputController _input;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _jumpForce = 5.0f;

    private Dictionary<State, PlayerState> _playerStates;
    private PlayerState _activeState;

    private void Awake()
    {
        _input ??= GetComponent<PlayerInputController>();
        _renderer ??= GetComponent<Renderer>();
        
        _input.OnJump += Jump;
    }

    private void Start()
    {
        _playerStates = new Dictionary<State, PlayerState>
        {
            { State.Dark, new PlayerDarkState(this) },
            { State.Light, new PlayerLightState(this) }
        };

        _activeState = _playerStates[State.Dark];
        _activeState.OnEnterState();
    }

    private void Update()
    {
        Move(_input.HorizontalMovementValue);
        _activeState.OnUpdateState();
    }

    public void ChangeState(State newState)
    {
        _activeState.OnExitState();
        _activeState = _playerStates[newState];
        _activeState.OnEnterState();
        _currentState = (int)newState;
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
        ChangeState(State.Light);
    }
    
    public void OnExitLight()
    {
        ChangeState(State.Dark);
    }

    public Material LightMaterial => _lightMaterial;
    public Material DarkMaterial => _darkMaterial;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StickPlayer"))
        {
            transform.SetParent(other.transform);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StickPlayer"))
        {
            transform.SetParent(null);
        }
    }
}
