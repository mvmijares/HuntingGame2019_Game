using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Camera focal point")]
    public Transform lookAtTarget;

    public float distanceFromTarget;
    [Tooltip("Offset from the camera focal point")]
    public Vector3 offset;


    private Vector3 currentPosition;

    public Vector2 verticalLookClamp; //min, max for how much the player can look up or down
    private float verticalInput;
    [SerializeField] private float verticalLookSpeed;
    private Quaternion verticalFromRotation;
    private Quaternion verticalToRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        verticalInput += vertical;
        verticalInput = Mathf.Clamp(verticalInput, verticalLookClamp.x, verticalLookClamp.y);

        verticalToRotation = Quaternion.Euler(-verticalInput,-5, 0);
        verticalFromRotation = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(verticalFromRotation, verticalToRotation, Time.deltaTime * verticalLookSpeed);
        
        currentPosition = transform.localPosition;

        transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, distanceFromTarget);
    }
}
