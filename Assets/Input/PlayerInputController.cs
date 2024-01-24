using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public event Action OnJump;
    public event Action OnCrouch;
    public event Action OnClick;

    private float _horizontalMovementValue;
    public float HorizontalMovementValue => _horizontalMovementValue;
    
    public Vector3 MousePositionScreen { get; private set; }
    public Vector3 MousePositionWorld { get; private set; }
    
    private InputAsset asset;

    private void Awake()
    {
        asset = new InputAsset();
        asset.Player.Click.performed += (context) => OnClick?.Invoke();
        asset.Player.Jump.performed += (context) => OnJump?.Invoke();
        asset.Player.Crouch.performed += (context) => OnCrouch?.Invoke();
    }

    private void Update()
    {
        _horizontalMovementValue = asset.Player.HorizontalMovement.ReadValue<float>();
    }

    private void OnEnable()
    {
        asset.Enable();
    }
    private void OnDisable()
    {
        asset.Disable();
    }

    private void OnDestroy()
    {
        OnJump = null;
        OnCrouch = null;
        OnClick = null;
    }
}
