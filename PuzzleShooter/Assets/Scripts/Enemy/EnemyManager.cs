using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform spawnZoneCenter;
    public float spawnZoneRadius;
    //Raycast from y height to ground to get height position for spawning object
    private float raycastHeight = 10f;
    public float raycastDistance = 15f;
    List<Enemy> enemyList;
    GameManager _gameManager;
    public GameObject enemyPrefab;
    private bool groundCheck = false;
    private int groundMask;
    /// <summary>
    /// Initialization our our manager class
    /// </summary>
    /// <param name="manager"></param>
    public void Initialize(GameManager manager)
    {
        _gameManager = manager;
        enemyList = new List<Enemy>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }
    /// <summary>
    /// Create a new enemy during run-time
    /// </summary>
    public void CreateNewEnemy()
    {
        Enemy newEnemy = Instantiate(enemyPrefab, GetSpawnPosition(), enemyPrefab.transform.rotation).GetComponent<Enemy>();
        newEnemy.Initialize(this);
        enemyList.Add(newEnemy);
    }
    /// <summary>
    /// Delete an Enemy from our list
    /// </summary>
    /// <param name="target"></param>
    public void DeleteEnemy(Enemy target)
    {
        if(enemyList.Contains(target))
            enemyList.Remove(target);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 newPosition = Vector3.zero;
        Vector3 tempOfPosition = Vector3.zero;
        float hieght = 1f;
        float offset = 0.1f;

        newPosition = Random.insideUnitSphere * spawnZoneRadius + spawnZoneCenter.position;
        tempOfPosition = new Vector3(newPosition.x, raycastHeight, newPosition.y);

        RaycastHit hit;
        Debug.DrawRay(tempOfPosition, Vector3.down * raycastDistance, Color.blue);
        if (Physics.Raycast(tempOfPosition, Vector3.down, out hit, raycastDistance, groundMask))
        {
            groundCheck = true;
            hieght = hit.point.y + offset;
        }
        else
        {
            groundCheck = false;
        }
        newPosition = new Vector3(newPosition.x, hieght, newPosition.z);
        return newPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(spawnZoneCenter)
            Gizmos.DrawWireSphere(spawnZoneCenter.position, spawnZoneRadius);
        
    }
}
