using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @todo 
 * @todo Make camera not bounce behind the wall when player gets to close to it
 */
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    InputManager _inputManager;
    [SerializeField]
    Transform _target; // The target the camera will follow
    [SerializeField]
    Transform _cameraPivot; // The object the camera uses to pivot (up and down)
    private Transform _cameraTransform; // The transform of the camera being controlled in the scene
    Vector3 _cameraFollowVelocity = Vector3.zero;
    Vector3 _cameraVectorPosition;
    [SerializeField]
    float _cameraFollowSpeed = 0.2f;
    [SerializeField]
    float _cameraLookSpeed = 2f;
    [SerializeField]
    float _cameraPivotSpeed = 2f;
    [SerializeField]
    float _cameraSmoothMovementSpeed = 1;
    float _lookAngle; // Angle that the camera looks up and down
    float _pivotAngle; // Angle that the camera looks left and right
    [SerializeField]
    float minimumPivotAngle = -35;
    [SerializeField]
    float _maximumPivotAngle = 35;
    [Header("Collision")]
    [SerializeField]
    LayerMask _collisionLayers; // The layers we want the camera to collide with
    [SerializeField]
    float _cameraCollisionRadius = 0.2f;
    [SerializeField]
    float _cameraCollisionOffset = 0.2f; // How much the camera will jump off of objects it collides with
    [SerializeField]
    float _minimumCollisionOffset = 0.2f;
   
    float _defaultPosition;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _defaultPosition = _cameraTransform.localPosition.z;
    }

    public void HandleAllMovement()
    {
        FollowTarget();

        if (_inputManager.IsCameraBeingControlled)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            RotateCamera();
            HandleCameraCollisions();
        } else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(
            transform.position, _target.position, ref _cameraFollowVelocity, _cameraFollowSpeed);

        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        _lookAngle = Mathf.Lerp(_lookAngle, _lookAngle + (_inputManager.GetCameraHorizontalInput() * _cameraLookSpeed), _cameraSmoothMovementSpeed * Time.deltaTime);
        // -_inputManager.GetCameraVerticalInput() here to look up when the mouse moves up
        _pivotAngle = Mathf.Lerp(_pivotAngle, _pivotAngle + (-_inputManager.GetCameraVerticalInput() * _cameraPivotSpeed), _cameraSmoothMovementSpeed * Time.deltaTime);

        _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivotAngle, _maximumPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = _lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = _pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        _cameraPivot.localRotation = targetRotation;
    }

    void HandleCameraCollisions()
    {
        float targetPosition = _defaultPosition;
        Vector3 direction = _cameraTransform.position - _cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(_cameraPivot.transform.position, _cameraCollisionRadius, direction, out RaycastHit hit, Mathf.Abs(targetPosition), _collisionLayers)) 
        {
            float distance = Vector3.Distance(_cameraPivot.transform.position, hit.point);
            targetPosition = -(distance - _cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < _minimumCollisionOffset)
        {
            targetPosition = targetPosition - _minimumCollisionOffset;
        }

        _cameraVectorPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, targetPosition, 0.2f);
        _cameraTransform.localPosition = _cameraVectorPosition;
    }
}
