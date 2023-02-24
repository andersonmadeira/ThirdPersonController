using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;

    public Vector2 _movementInput;
    float _verticalMovement;
    float _horizontalMovement;

    void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput(); 
    }

    void HandleMovementInput()
    {
        _verticalMovement = _movementInput.y;
        _horizontalMovement = _movementInput.x;
    }

    public float GetVerticalMovement()
    {
        return _verticalMovement;
    }

    public float GetHorizontalMovement()
    {
        return _horizontalMovement;
    }
}
