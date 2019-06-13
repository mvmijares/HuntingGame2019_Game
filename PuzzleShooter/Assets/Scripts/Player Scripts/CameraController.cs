using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Player _player;
    Camera component;
    private Transform _target;
    public Transform target { get { return _target; } }

    public float normalFOV = 60f;
    public float aimFOV = 45f;
    
    public float distanceFromTarget;
    [Tooltip("Offset from the camera focal point")]
    public Vector3 offset;
    public Vector2 pivotMinMax; //min, max for how much the player can look up or down
    private float verticalInput;
    private float horizontalInput;
    public float yLookSpeed;
    public float xLookSpeed;
    public Ray lookDirection;

    //Debug variables
    public bool debugMode; //showing gizmos
    public float wireCubeDimension = 0.1f;
    //Initialization called by Player
    public void Initialize(Player player)
    {
        if (player)
        {
            _player = player;
            Cursor.lockState = CursorLockMode.Locked;
            component = GetComponent<Camera>();
            lookDirection = new Ray();
            _target = new GameObject("Camera Focal Point").transform;
            _target.position = _player.transform.position + offset;
            _target.SetParent(_player.transform);
            transform.SetParent(_target);
        }
    }
    public void CustomUpdate()
    {
        MoveCamera();
        CreateCameraDirection();
    }
    public void CustomLateUpdate()
    {
        RotateCamera();
    }
    private void MoveCamera()
    {
        transform.localPosition = Vector3.forward * -distanceFromTarget;

        float FOV = (_player.playerInput.aim) ? aimFOV : normalFOV;

        component.fieldOfView = Mathf.Lerp(component.fieldOfView, FOV, Time.deltaTime * _player.aimSpeed);
    }
    private void RotateCamera()
    {
        Quaternion newRotation;
        verticalInput += _player.playerInput.mouseY;

        verticalInput = Mathf.Clamp(verticalInput, pivotMinMax.x, pivotMinMax.y);

        newRotation = Quaternion.Euler(-verticalInput, 0, 0);
        _target.localRotation = Quaternion.Slerp(_target.localRotation, newRotation, Time.deltaTime * yLookSpeed);
    }

    private void CreateCameraDirection()
    {
        lookDirection = new Ray(transform.position, transform.forward); // TODO: Create a better method of creating camera look direction
    }
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.white;
            if (_target)
                Gizmos.DrawWireCube(_player.transform.position + offset, new Vector3(wireCubeDimension, wireCubeDimension, wireCubeDimension));
        }
    }

}
