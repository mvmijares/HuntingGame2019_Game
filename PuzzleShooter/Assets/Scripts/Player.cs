using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;

    private float horizontalInput;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    private Quaternion quaternionFromRotation;
    private Quaternion quaternionToRotation;

    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Vertical");

        ApplyInput(horizontal, vertical);

        bool aiming = Input.GetMouseButton(1);
        
        anim.SetBool("Aim", aiming);
        
    }

    private void ApplyInput(float horizontal, float vertical)
    {
        Move(vertical);
        Turn(horizontal);
    }
    private void Move(float input)
    {
        transform.Translate(Vector3.forward * input * moveSpeed * Time.deltaTime);

        if(input != 0)
        {
            anim.SetFloat("Speed", input);
        }
    }

    private void Turn(float input)
    {
        horizontalInput += input;
        quaternionToRotation = Quaternion.Euler(0, horizontalInput, 0);
        transform.rotation = Quaternion.Slerp(quaternionFromRotation, quaternionToRotation, Time.deltaTime * turnSpeed);
    }
}
