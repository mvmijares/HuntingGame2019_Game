using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        if (_player) _player.Initialize(this);
    }
    private void Update()
    {
        
    }
    private void LateUpdate()
    {
        // TODO : Setup a better method of order execution
        // ORDER
        // Set target, update IK animations, lastly move/ rotate camera to follow player
        _player.weaponAim.LateTick();
        _player.aimIKHelper.LateTick();
        _player.cameraController.LateTick();
    }
}
