using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Enemy> enemyList;
    GameManager _gameManager;
    public void Initialize(GameManager manager)
    {
        _gameManager = manager;
        enemyList = new List<Enemy>();

    }

    public void CreateNewEnemy()
    {
        Enemy enemy = new Enemy(this, "Chicken",);
        enemyList.Add(enemy);
    }
    public void UpdateEnemyList()
    {
        foreach(Enemy e in enemyList)
        {
            e.CustomUpdate();
        }
    }
    public void LateUpdateEnemyList()
    {
        foreach(Enemy e in enemyList)
        {
            e.CustomLateUpdate();
        }
    }
}
