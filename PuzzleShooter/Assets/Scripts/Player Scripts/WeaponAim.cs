using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    private Player _player;
    private Transform _target;
    public Transform target { get { return _target; } } //target position set by Camera

    public float distanceFromPlayer; //how far the target should be

    public bool debugMode; //showing gizmos
    public float wireCubeDimension = 0.1f;
    public void Initialize(Player player)
    {
        if (player)
        {
            _player = player;

            _target = new GameObject("Aiming Focal Point").transform;

        }
    }

    private void Update()
    {
        if (_player)
        {
            Vector3 point = _player.cameraController.lookDirection.GetPoint(distanceFromPlayer);

            _target.position = point;
            SetTarget();
        }
    }

    private void SetTarget()
    {
        _player.aimIKHelper.SetTarget(_target);
    }

    private void OnDrawGizmos()
    {
        if (debugMode) {
            Gizmos.color = Color.white;
            if(_target)
                Gizmos.DrawWireCube(_target.position, new Vector3(wireCubeDimension, wireCubeDimension, wireCubeDimension));
        }
    }
}
