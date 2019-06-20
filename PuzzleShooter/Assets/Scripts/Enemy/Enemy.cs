using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AIState
{
    Idle, Walk, Run
};
public enum EnemyType
{
    Chicken
}
struct EnemyTarget
{
    public bool set;
    public Vector3 position;
    public float totalDistance;
    public void Reset()
    {
        set = false;
        totalDistance = 0f;
        position = Vector3.zero;
    }
};

public class Enemy : MonoBehaviour
{
    //TODO : make a seperate class for enemy AI solving
    public enum AIState { Walk, Run, Idle}
    [SerializeField] AIState state;
    EnemyType type;
    private EnemyManager _enemyManager;
    private CapsuleCollider col;
    private OnHealth healthComponent;
    private GameObject model;
    private float idleTime;
    private float currIdleTime;
    private EnemyTarget target;
    private Animator anim;
    private Vector3 moveDirection;
    private Vector3 spawnPoint;
    private GameObject deathEffectPrefab;
    private bool isDeathAnimPlaying = false;

    public float moveSpeed;
    public float turnSpeed;
    public Vector2 idleTimeMinMax;
    public float walkRadius;
    public float remainingDistance;
    /// <summary>
    /// Constructor for Enemy
    /// </summary>
    /// <param name="manager">Reference to Enemy Manager class</param>
    /// <param name="name">Name of enemy.</param>
    /// <param name="position">World position for game object.</param>
    public void Initialize(EnemyManager manager)
    {
        _enemyManager = manager;
        anim = GetComponentInChildren<Animator>();
        if (anim)
            model = anim.gameObject; // assuming our model has a animator attached to it.

        col = GetComponent<CapsuleCollider>();
        healthComponent = GetComponent<OnHealth>();

        state = AIState.Idle;
        spawnPoint = transform.position;
        target.set = false;
        target.position = Vector3.zero;
        deathEffectPrefab = Resources.Load("EnemyDeath_Prefab") as GameObject;
        type = EnemyType.Chicken; // Prototyping purposes
        SetupEnemy();
    }
    /// <summary>
    /// Setup a definition for our enemy type.
    /// TODO : Create a enemy type class and use inheritence
    /// </summary>
    private void SetupEnemy()
    {
        switch (type)
        {
            case EnemyType.Chicken:
                {
                    //Should be setup in Enemy Factor
                    idleTimeMinMax = new Vector2(3, 5f);
                    walkRadius = 5f;
                    remainingDistance = 1f;
                    break;
                }
        }

        idleTime = UnityEngine.Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
    }
    //Enemy doesn't require a custom update
    private void Update()
    {
        bool dead = healthComponent.isDead;

        if (!dead)
        {
            AISolver();
            ApplyMovement();
        }
        else
        {
            PlayDeathAnimation();
            _enemyManager.DeleteEnemy(this); // TODO : Make a better way to handle enemy deletion
        }
    }
    /// <summary>
    /// Public method for enemy manager to call destruction.
    /// Used for destroying all enemies in list
    /// </summary>
    public void EnemyDestructionCall()
    {
        PlayDeathAnimation();
    }
    /// <summary>
    /// Handles death functionality for enemy.
    /// TODO : Make a interface for death animation calls
    /// </summary>
    private void PlayDeathAnimation()
    {
        if (!isDeathAnimPlaying)
        {
            GameObject clone = Instantiate(deathEffectPrefab, transform.position, deathEffectPrefab.transform.rotation) as GameObject;
            isDeathAnimPlaying = true;
        }
    }

    /// <summary>
    /// Method for state machine type AI system.
    /// </summary>
    private void AISolver()
    {
        switch (state)
        {
            case AIState.Idle:
                {
                    HandleIdleState();
                    break;
                }
            case AIState.Walk:
                {
                    HandleWalkState();
                    break;
                }
        }
    }
    /// <summary>
    /// Method to handle idle state
    /// </summary>
    private void HandleIdleState()
    {
        if(idleTime == 0)
        {
            idleTime = UnityEngine.Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
        }
        currIdleTime += Time.deltaTime;
        if(currIdleTime > idleTime)
        {
            currIdleTime = 0;
            idleTime = 0;
            state = AIState.Walk;
        }
        anim.SetFloat("Speed", 0f);
    }
    /// <summary>
    /// Method to handle walking state
    /// </summary>
    private void HandleWalkState()
    {
        //Find a point to move towards.
        //Walk (calculate direction) towards  target.
        //Check Distance
        // Switch to Idle State
        //TODO // Check if position is within game boundaries OR if we can actually get to target
        if (!target.set) {
            Vector3 randomPoint = spawnPoint + (UnityEngine.Random.insideUnitSphere * walkRadius);
            target.position = new Vector3(randomPoint.x, GetHeightPosition(randomPoint),randomPoint.z);
     
            target.set = true;
        }
        else
        {
            moveDirection = (target.position - transform.position).normalized;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            DistanceCheck();
        }
        anim.SetFloat("Speed", 0.5f);
    }

    private float GetHeightPosition(Vector3 randomPoint)
    {
        Vector3 heightPoint = new Vector3(randomPoint.x, 10, randomPoint.z);
        float height = 0;
        float offset = 0f; //offset position from the hit point on the y axis; 
        RaycastHit hit;
        Debug.DrawRay(heightPoint, Vector3.down * 1000f,Color.red, 2.0f);
        int groundMask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(heightPoint, Vector3.down, out hit, 10f, groundMask))
        {
            height = hit.point.y + offset;
        }
        return height;
    }

    /// <summary>
    /// Check distance before reseting state
    /// </summary>
    private void DistanceCheck()
    {
        if (target.set)
        {
            float magnitude = (target.position - transform.position).magnitude;
            if (magnitude <= remainingDistance)
            {
                if (state == AIState.Walk)
                {
                    state = AIState.Idle;
                    target.Reset();
                }
            }
        }
    }
    /// <summary>
    /// Method to handle rotation and movement for our enemy
    /// </summary>
    private void ApplyMovement()
    {
        if (target.set)
        {
            if (moveDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
            }
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }
    /// <summary>
    /// Debug Draw calls for behind the scenes logic
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (target.set)
        {
            Gizmos.DrawWireCube(target.position, Vector3.one);
        }
    }

}
