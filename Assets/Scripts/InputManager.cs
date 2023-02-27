using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimationManager animationManager;

    public Vector2 _playerMovementInput;
    public Vector2 _cameraRotationInput;

    float cameraVerticalInput;
    float cameraHorizontalInput;

    float _movementAmount;
    float _verticalMovement;
    float _horizontalMovement;

    public bool IsCameraBeingControlled = false;

    void Awake()
    {
        animationManager = GetComponent<AnimationManager>();        
    }

    void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }

        playerControls.Player.Movement.performed += OnPlayerMovement;
        playerControls.Player.CameraRotation.performed += OnCameraRotation;
        playerControls.Player.ToggleCameraRotation.performed += OnCameraRotationActivated;
        playerControls.Player.ToggleCameraRotation.canceled += OnCameraRotationDeactivated;

        playerControls.Enable();
    }

    public float GetPlayerMovementAmount()
    {
        return _movementAmount;
    }

    void OnPlayerMovement(InputAction.CallbackContext ctx)
    {
        _playerMovementInput = ctx.ReadValue<Vector2>();
    }

    void OnCameraRotation(InputAction.CallbackContext ctx)
    {
        _cameraRotationInput = ctx.ReadValue<Vector2>();
    }

    void OnCameraRotationActivated(InputAction.CallbackContext ctx)
    {
        IsCameraBeingControlled = true;
    }

    void OnCameraRotationDeactivated(InputAction.CallbackContext obj)
    {
        IsCameraBeingControlled = false;
    }

    void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Player.Movement.performed -= OnPlayerMovement;
            playerControls.Player.CameraRotation.performed -= OnCameraRotation;
            playerControls.Player.ToggleCameraRotation.performed -= OnCameraRotationActivated;
            playerControls.Player.ToggleCameraRotation.canceled -= OnCameraRotationDeactivated;

            playerControls.Disable();
        }
    }

    public void HandleAllInput()
    {
        HandleMovementInput(); 
    }

    void HandleMovementInput()
    {
        _verticalMovement = _playerMovementInput.y;
        _horizontalMovement = _playerMovementInput.x;
        cameraVerticalInput = _cameraRotationInput.y;
        cameraHorizontalInput = _cameraRotationInput.x;
        _movementAmount = Mathf.Clamp01(Mathf.Abs(_horizontalMovement) + Mathf.Abs(_verticalMovement));
        animationManager.UpdateAnimationValues(0, _movementAmount);
    }

    public float GetVerticalMovement()
    {
        return _verticalMovement;

    }

    public float GetHorizontalMovement()
    {
        return _horizontalMovement;
    }

    public float GetCameraVerticalInput()
    {
        return cameraVerticalInput;
    }

    public float GetCameraHorizontalInput()
    {
        return cameraHorizontalInput;
    }
}
