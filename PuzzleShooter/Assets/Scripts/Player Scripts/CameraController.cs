using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Player _player;
    
    Camera component;
    [Tooltip("Camera focal point")]
    public Transform lookAtTarget;
    public float normalFOV = 60f;
    public float aimFOV = 45f;
    public float distanceFromTarget;
    public float heightPosition;
    public float cameraLookSpeed;
    [Tooltip("Offset from the camera focal point")]
    public Vector3 offset;


    private Vector3 currentPosition;

    public Vector2 verticalLookClamp; //min, max for how much the player can look up or down
    private float verticalInput;
    private float horizontalInput;
    public float yLookSpeed;
    public float xLookSpeed;
    private Quaternion verticalFromRotation;
    private Quaternion verticalToRotation;
    public Ray lookDirection;

    private Transform _target;
    public Transform target { get { return _target; } }

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
            _target = new GameObject("Aim Focal Point").transform;
        }


    }
    private void Update()
    {
        LookAtPoint();
    }
    private void LateUpdate()
    {

        RotateCamera();
        CreateCameraDirection();

        MoveCamera();
    }

    
    private void MoveCamera()
    {
        Vector3 followVector = _player.transform.forward * distanceFromTarget;
        Vector3 heightVector = Vector3.up * heightPosition;

        Vector3 newPosition = _player.transform.position - followVector + heightVector;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraLookSpeed);

        if (_player)
            component.fieldOfView = (_player.playerInput.aim) ? aimFOV : normalFOV;

    }

    private void LookAtPoint()
    {
        Vector3 point = _player.cameraController.lookDirection.GetPoint(20.0f); //test var

        _target.position = point;
        transform.LookAt(_target);
    }
    private void RotateCamera()
    {

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
                Gizmos.DrawWireCube(_target.position, new Vector3(wireCubeDimension, wireCubeDimension, wireCubeDimension));
        }
    }

}
