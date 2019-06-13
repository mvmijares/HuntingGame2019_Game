using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] EnemyManager _eManager;
    //Testing Purposes
    [SerializeField] Enemy enemy_1;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        if (_player)
            _player.ObjectInitialize(this);

        _eManager = FindObjectOfType<EnemyManager>();
        if(_eManager)
            _eManager.Obje
    }
    private void Update()
    {
        _player.CustomUpdate();
        enemy_1.CustomUpdate();
    }
    private void LateUpdate()
    {
        _player.CustomLateUpdate();
    }
}
