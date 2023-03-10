using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputManager _inputManager;

    Vector3 _movementDirection;
    Transform _cameraTransform;

    Rigidbody _playerRigidbody;

    [SerializeField]
    float _walkSpeed = 1.5f;
    [SerializeField]
    float _runSpeed = 5f;
    [SerializeField]
    float _sprintSpeed = 7f;

    [SerializeField]
    float _rotationSpeed = 15;

    void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        CalculateMovementDirection();

        float movementAmount = _inputManager.GetPlayerMovementAmount();

        if (movementAmount > 0.5f)
        {
            _movementDirection *= _runSpeed;
        }
        else
        {
            _movementDirection *= _walkSpeed;
        }

        Vector3 movementVelocity = _movementDirection;
        _playerRigidbody.velocity = movementVelocity;
    }

    void HandleRotation()
    {
        CalculateMovementDirection();

        // Continue facing the direction we were moving to
        if (_movementDirection == Vector3.zero)
        {
            _movementDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(_movementDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed);

        transform.rotation = playerRotation;
    }

    void CalculateMovementDirection()
    {
        _movementDirection = new Vector3(_cameraTransform.forward.x, 0f, _cameraTransform.forward.z) * _inputManager.GetVerticalMovement();
        _movementDirection += _cameraTransform.right * _inputManager.GetHorizontalMovement();
        _movementDirection.Normalize();
        _movementDirection.y = 0;
    }
}
