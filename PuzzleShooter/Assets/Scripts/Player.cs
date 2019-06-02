using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;

    Transform playerCamera;
    private float horizontalInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    private Quaternion quaternionFromRotation;
    private Quaternion quaternionToRotation;

    //TODO : Seperate firing from player class
    [SerializeField] private bool fire; //input check for firing
    private bool isFireCoroutine;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireDistance;
    [SerializeField] private float fireRate; // max fire time
    [SerializeField] private float currFireTime; //current timer on fire
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Vertical");

        ApplyInput(horizontal, vertical);

        bool aiming = Input.GetMouseButton(1);
        
        anim.SetBool("Aim", aiming);

        fire = Input.GetMouseButton(0);

        if (fire)
        {
            if (!isFireCoroutine)
            {   
                StartCoroutine("FireCoroutine");
                isFireCoroutine = true;
            }
        }
        else
        {
            if (isFireCoroutine)
            {
                StopCoroutine("FireCoroutine");
                isFireCoroutine = false;
            }
        }
    }

    private IEnumerator FireCoroutine()
    {
        Fire();
        yield return new WaitForSeconds(fireRate);
        isFireCoroutine = false;
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

    private void Fire()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(firePoint.position, playerCamera.forward * fireDistance,Color.red, 10.0f);
        if(Physics.Raycast(firePoint.position, playerCamera.forward, out hitInfo, fireDistance))
        {
            if (hitInfo.collider.GetComponent<OnHealth>())
            {
                Debug.Log("Has a health component");
                hitInfo.collider.GetComponent<OnHealth>().OnTakeDamage(1);
            }
        }
    }

}
