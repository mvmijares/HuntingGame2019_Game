﻿using System.Collections;
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
    [Tooltip("Offset from the camera focal point")]
    public Vector3 offset;


    private Vector3 currentPosition;

    public Vector2 verticalLookClamp; //min, max for how much the player can look up or down
    private float verticalInput;
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

        }
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        component = GetComponent<Camera>();
        lookDirection = new Ray();
    }

    private void LateUpdate()
    {
        verticalInput += _player.playerInput.mouseY;
        verticalInput = Mathf.Clamp(verticalInput, verticalLookClamp.x, verticalLookClamp.y);
        verticalToRotation = Quaternion.Euler(-verticalInput,-5, 0);
        verticalFromRotation = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(verticalFromRotation, verticalToRotation, Time.deltaTime * verticalLookSpeed);

        if (_player)
            component.fieldOfView = (_player.aim) ? aimFOV : normalFOV;
        
       
        currentPosition = transform.localPosition;
        transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, distanceFromTarget);

        lookDirection = new Ray(transform.position, transform.forward); // TODO: Create a better method of creating camera look direction

    }
}