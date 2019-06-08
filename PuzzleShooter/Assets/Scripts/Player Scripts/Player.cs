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
    public CameraController cameraController;
    AimIKHelper _aimIKHelper;
    public AimIKHelper aimIKHelper { get { return _aimIKHelper; } }
    WeaponAim _weaponAim;
    public WeaponAim weaponAim { get { return _weaponAim;  } }
    Weapon _weapon;
    public Weapon weapon { get { return weapon; } }
    [Tooltip("Player move speed")]
    public float moveSpeed;
    [Tooltip("Player turn speed")]
    public float turnSpeed;


    private float horizontalInput;
    private Quaternion quaternionFromRotation;
    private Quaternion quaternionToRotation;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.Initialize(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.Initialize(this);
        _aimIKHelper = GetComponentInChildren<AimIKHelper>(); // assuming we only have one instance in player object
        _aimIKHelper.Initialize(this);
        _weaponAim = GetComponent<WeaponAim>();
        _weaponAim.Initialize(this);
        _weapon = GetComponentInChildren<Weapon>();
        _weapon.Initialize(this);


        cameraController.Initialize(this);
    }
}
