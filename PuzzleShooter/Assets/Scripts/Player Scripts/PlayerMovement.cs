using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Data

    private Player _player;
    private PlayerInput _playerInput;
    private Animator anim;

    private float cameraInputH; // caching horizontalInput
    private Quaternion quaternionFromRotation;
    private Quaternion quaternionToRotation;
    #endregion
    public void Initialize(Player player)
    {
        _player = player;
        _playerInput = player.playerInput;
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ApplyInput();
        HandleAnimations();
    }

    private void ApplyInput()
    {
        Move();
        Turn();
    }
    private void Move()
    {
        transform.Translate(transform.forward * _playerInput.vertical * _player.moveSpeed * Time.deltaTime);
    }
    private void Turn()
    {
        cameraInputH += _playerInput.mouseX;
        quaternionToRotation = Quaternion.Euler(0, cameraInputH, 0);
        transform.rotation = Quaternion.Slerp(quaternionFromRotation, quaternionToRotation, Time.deltaTime * _player.turnSpeed);
    }
    private void HandleAnimations()
    {
        anim.SetFloat("Speed", _playerInput.vertical);
        anim.SetBool("Aim", _playerInput.aim);
    }
}
