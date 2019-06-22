using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Data
    Player _player;
    [SerializeField] private float _horizontal;
    public float horizontal { get { return _horizontal; } }
    [SerializeField] private float _vertical;
    public float vertical { get { return _vertical; } }
    [SerializeField] private bool _aim;
    public bool aim { get { return _aim; } }
    [SerializeField]
    private bool _fire;
    public bool fire { get { return _fire; } }
    [SerializeField]
    private float _mouseX;
    public float mouseX { get { return _mouseX; } }
    [SerializeField]
    private float _mouseY;
    public float mouseY { get { return _mouseY; } }

    public float mouseYSpeed;
    public float mouseXSpeed;
    #endregion

    public void Initialize(Player player)
    {
        _player = player;
    }
    public void CustomUpdate()
    {
        GetPlayerInput();
    }
    private void GetPlayerInput()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");
        _aim = Input.GetMouseButton(1);
        _fire = Input.GetMouseButton(0);
    }
}
