using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        if (_player)
            _player.ObjectInitialize(this);
    }
    private void Update()
    {
        _player.CustomUpdate();
    }
    private void LateUpdate()
    {
        _player.CustomLateUpdate();
    }
}
