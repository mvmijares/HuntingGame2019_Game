﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AIState
{
    Idle, Walk, Run
};
public enum EnemyType
{
    Chicken, Rooster, Goat, Pig
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
/// <summary>
/// TODO : Make Enemy an base class and derive seperate Enemy types
/// - Enemy Prefabs have different sizes and require different collision detection methods
/// 
/// </summary>
public class Enemy : MonoBehaviour
{
    //TODO : make a seperate class for enemy AI solving
    public enum AIState { Walk, Run, Idle}
    [SerializeField] AIState state;
    public EnemyType type;
    private EnemyManager _enemyManager;
    private Player player;
    private CapsuleCollider col;
    Rigidbody rigidbody;

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

    private bool isGrounded;
    private Vector3 feetPosition; //Feet position 
    public float groundCheckDistance; // Raycast distance for checking ground
    public float moveSpeed;
    public float turnSpeed;
    public Vector2 idleTimeMinMax;
    public float walkRadius;
    public float remainingDistance;
    public float detectionRadius;
    /// <summary>
    /// Constructor for Enemy
    /// </summary>
    /// <param name="manager">Reference to Enemy Manager class</param>
    /// <param name="name">Name of enemy.</param>
    /// <param name="position">World position for game object.</param>
    public void Initialize(EnemyManager manager)
    {
        _enemyManager = manager;
        col = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        healthComponent = GetComponent<OnHealth>();

        anim = GetComponentInChildren<Animator>();
        if (anim)
            model = anim.gameObject; // assuming our model has a animator attached to it.
        
        state = AIState.Idle;
        spawnPoint = transform.position;
        target.set = false;
        target.position = Vector3.zero;
        deathEffectPrefab = Resources.Load("EnemyDeath_Prefab") as GameObject;
        SetupEnemy();
        player = null;
        detectionRadius = 3.0f;
        isGrounded = false;
        groundCheckDistance = 1f;
    }

    public void SetInitialHealth(int size)
    {
        if (healthComponent)
            healthComponent.health = size;
    }
    /// <summary>
    /// Setup a definition for our enemy type.
    /// TODO : Create a enemy type class and use inheritence
    /// TODO : Create different values for each enemy type
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
            case EnemyType.Goat:
                {
                    idleTimeMinMax = new Vector2(3, 5f);
                    walkRadius = 5f;
                    remainingDistance = 1f;
                    break;
                }
            case EnemyType.Pig:
                {
                    idleTimeMinMax = new Vector2(3, 5f);
                    walkRadius = 5f;
                    remainingDistance = 1f;
                    break;
                }
            case EnemyType.Rooster:
                {
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
            PlayerDetection();
            AISolver();
            ApplyMovement();
        }
        else
        {
            PlayDeathAnimation();
            _enemyManager.DeleteEnemy(this); // TODO : Make a better way to handle enemy deletion
        }
    }

    private void PlayerDetection()
    {
        if (state != AIState.Run)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            int i = 0;
            while (i < colliders.Length)
            {
                if (colliders[i].gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    state = AIState.Run;
                    player = colliders[i].GetComponent<Player>();
                }
                i++;
            }
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
            case AIState.Run:
                {
                    HandleRunState();
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
    /// Method to handle enemy in a neutral state.
    /// Enemy is roaming and does not detect the player
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
    /// <summary>
    /// Method to handle enemy in scared state.
    /// Enemy is running away from player and will hide after x amount of time
    /// </summary>
    private void HandleRunState()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            moveDirection = Vector3.Reflect(direction, direction);
            
            moveSpeed = 5f;
            turnSpeed = 5f;

            if (GroundCheck())
            {
                rigidbody.AddForce(Vector3.up * 30.0f);
                anim.SetFloat("Speed", 1f);
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }
        }
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
    /// Raycasts downwards to check if the object is on the ground
    /// </summary>
    /// <returns></returns>
    private bool GroundCheck()
    {
        bool check = false;
        int groundMask = 1 << LayerMask.NameToLayer("Ground");
        feetPosition = transform.position + col.center + (Vector3.down * col.radius);
        if (Physics.Raycast(feetPosition, Vector3.down, groundCheckDistance, groundMask))
        {
            check = true;
        }
        return check;
    }
    /// <summary>
    /// Method to handle rotation and movement for our enemy
    /// </summary>
    private void ApplyMovement()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
        }
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
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