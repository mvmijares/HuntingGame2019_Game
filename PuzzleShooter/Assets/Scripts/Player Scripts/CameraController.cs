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
    [Tooltip("Offset from the camera focal point")]
    public Vector3 offset;


    private Vector3 currentPosition;

    public Vector2 verticalLookClamp; //min, max for how much the player can look up or down
    private float verticalInput;
    private float horizontalInput;
    [SerializeField] private float verticalLookSpeed;
    private Quaternion verticalFromRotation;
    private Quaternion verticalToRotation;
    public Ray lookDirection;

    //Initialization called by Player
    public void Initialize(Player player)
    {
        if (player)
        {
            _player = player;
            Cursor.lockState = CursorLockMode.Locked;
            component = GetComponent<Camera>();
            lookDirection = new Ray();

            
        }
    }

    private void LateUpdate()
    {
        MoveCamera();
        RotateCamera();
        CreateCameraDirection();
    }

    
    private void MoveCamera()
    {
        Vector3 followVector = _player.transform.forward * distanceFromTarget;
        Vector3 heightVector = Vector3.up * heightPosition;

        transform.position = _player.transform.position - followVector + heightVector;

        if (_player)
            component.fieldOfView = (_player.playerInput.aim) ? aimFOV : normalFOV;

    }
    private void RotateCamera()
    {

        verticalInput += _player.playerInput.mouseY;
        horizontalInput += _player.playerInput.mouseX;

        //verticalInput = Mathf.Clamp(verticalInput, verticalLookClamp.x, verticalLookClamp.y);
        //verticalToRotation = Quaternion.Euler(0, -verticalInput, 0);
        //transform.rotation = Quaternion.Slerp(transform.rotation, verticalToRotation, Time.deltaTime * _player.turnSpeed);

        Quaternion horizontalRotation = Quaternion.Euler(0, -horizontalInput, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, horizontalRotation, 0);
       
    }
    
    private void CreateCameraDirection()
    {
        lookDirection = new Ray(transform.position, transform.forward); // TODO: Create a better method of creating camera look direction
    }
}
