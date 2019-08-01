using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseObject
{
    private Animator anim;
    PlayerInput _playerInput;
    public PlayerInput playerInput { get { return _playerInput; } }
    PlayerMovement _playerMovement;
    public PlayerMovement playerMovement { get { return playerMovement; } }
    public CameraController cameraController;
    private AudioSource audioSource;
    AimIKHelper _aimIKHelper;
    public AimIKHelper aimIKHelper { get { return _aimIKHelper; } }
    WeaponAim _weaponAim;
    public WeaponAim weaponAim { get { return _weaponAim;  } }
    Weapon _weapon;
    public Weapon weapon { get { return _weapon; } }
    [Tooltip("Player move speed")]
    public float moveSpeed;
    [Tooltip("Player turn speed")]
    public float turnSpeed;
    public float aimSpeed = 1;

    private float horizontalInput;
    private Quaternion quaternionFromRotation;
    private Quaternion quaternionToRotation;

    public override void ObjectInitialize(GameManager manager)
    {
        base.ObjectInitialize(manager);
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.Initialize(this);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.Initialize(this);
        _aimIKHelper = GetComponentInChildren<AimIKHelper>(); // assuming we only have one instance in player object
        _aimIKHelper.Initialize(this);
        _weaponAim = GetComponentInChildren<WeaponAim>();
        _weaponAim.Initialize(this);
        _weapon = GetComponentInChildren<Weapon>();
        _weapon.Initialize(this);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GetGameManager().audioHandler.GetClip("Background Music");
        cameraController.Initialize(this);
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        _playerInput.CustomUpdate();
        _playerMovement.CustomUpdate();
        cameraController.CustomUpdate();
        
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
    public override void CustomLateUpdate()
    {
        base.CustomLateUpdate();
        _weaponAim.CustomLateUpdate();
        _aimIKHelper.CustomLateUpdate();
        cameraController.CustomLateUpdate();
    }
}
