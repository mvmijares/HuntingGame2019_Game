using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    PlayerInput _playerInput;
    public PlayerInput playerInput { get { return _playerInput; } }
    PlayerMovement _playerMovement;
    public PlayerMovement playerMovement { get { return playerMovement; } }
    CameraController _cameraController;
    public CameraController cameraController { get { return _cameraController; } }
    AimIKHelper _aimIKHelper;
    public AimIKHelper aimIKHelper { get { return _aimIKHelper; } }
    WeaponAim _weaponAim;
    public WeaponAim weaponAim { get { return _weaponAim;  } } 
    public float moveSpeed;
    public float turnSpeed;




    private bool _aim;
    public bool aim { get { return _aim; } }
    private float horizontalInput;
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
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.Initialize(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.Initialize(this);
        _cameraController = GetComponentInChildren<CameraController>(); // assuming we only have one instance
        _cameraController.Initialize(this);
        _aimIKHelper = GetComponentInChildren<AimIKHelper>(); // assuming we only have one instance in player object
        _aimIKHelper.Initialize(this);
        _weaponAim = GetComponent<WeaponAim>();
        _weaponAim.Initialize(this);
    }

    private void Update()
    {
        //float horizontal = Input.GetAxis("Mouse X");
        //float vertical = Input.GetAxis("Vertical");

        //ApplyInput(horizontal, vertical);

        //_aim = Input.GetMouseButton(1);
        
        //anim.SetBool("Aim", _aim);

        //fire = Input.GetMouseButton(0);

        //if (fire)
        //{
        //    if (!isFireCoroutine)
        //    {   
        //        StartCoroutine("FireCoroutine");
        //        isFireCoroutine = true;
        //    }
        //}
        //else
        //{
        //    if (isFireCoroutine)
        //    {
        //        StopCoroutine("FireCoroutine");
        //        isFireCoroutine = false;
        //    }
        //}
    }

  
    private void ApplyInput(float horizontal, float vertical)
    {
    
        Move(vertical);
        if(vertical != 0)
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
        transform.rotation = Quaternion.Slerp(quaternionFromRotation, quaternionToRotation, Time.deltaTime);
    }

    /* Work on firing mechanics in a seperate class
     *   private IEnumerator FireCoroutine()
    {
        Fire();
        yield return new WaitForSeconds(fireRate);
        isFireCoroutine = false;
    }
    //private void Fire()
    //{
    //    RaycastHit hitInfo;
    //    Debug.DrawRay(firePoint.position, _cameraController.transform.forward * fireDistance,Color.red, 10.0f);
    //    if(Physics.Raycast(firePoint.position, _cameraController.forward, out hitInfo, fireDistance))
    //    {
    //        if (hitInfo.collider.GetComponent<OnHealth>())
    //        {
    //            Debug.Log("Has a health component");
    //            hitInfo.collider.GetComponent<OnHealth>().OnTakeDamage(1);
    //        }
    //    }
    //}
    */
}
