using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Enemy> enemyList;
    GameManager _gameManager;
    public void Initilize(GameManager manager)
    {
        _gameManager = manager;
        enemyList = new List<Enemy>();

    }

    public void CreateNewEnemy()
    {
        Enemy enemy = new Enemy();

    }
}
