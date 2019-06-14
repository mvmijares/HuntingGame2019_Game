using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] EnemyManager _eManager;
    //Testing Purposes
    public float numEnemies;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        if (_player)
            _player.ObjectInitialize(this);

        _eManager = FindObjectOfType<EnemyManager>();
        if (_eManager)
            _eManager.Initialize(this);

        if (numEnemies > 0)
        {
            for (int i = 0; i < numEnemies; i++)
            {
                _eManager.CreateNewEnemy();
            }
        }
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
