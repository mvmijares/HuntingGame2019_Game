using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AIState
{
    Idle, Walk, Run
};

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
    private EnemyManager _enemyManager;
    public float moveSpeed;
    public float turnSpeed;
    public Vector2 idleTimeMinMax;
    [SerializeField] private float idleTime;
    [SerializeField] private float currIdleTime;
    public float walkRadius;
    [SerializeField] EnemyTarget target;
    public float remainingDistance;
    Animator anim;
    Vector3 moveDirection;

    public void Initialize(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
        state = AIState.Idle;
        target.set = false;
        target.position = Vector3.zero;
        idleTime = Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
        anim = GetComponentInChildren<Animator>();
    }

    public void CustomUpdate()
    {
        AISolver();
        ApplyMovement();
        HandleAnimations();
    }

   
    public void CustomLateUpdate()
    {

    }
    private void HandleAnimations()
    {
        
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
            idleTime = Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
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
            target.position = (transform.position + Random.insideUnitSphere) * walkRadius;
            target.position = new Vector3(target.position.x, 0, target.position.z);
            target.set = true;
        }
        else
        {
            moveDirection = (target.position - transform.position).normalized;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            anim.SetFloat("Speed", 0.5f);
            DistanceCheck();
        }
    }

    /// <summary>
    /// Check distance before reseting state
    /// </summary>
    private void DistanceCheck()
    {
        if (target.set)
        {
            float magnitude = (target.position - transform.position).sqrMagnitude;
            if (magnitude <= remainingDistance * remainingDistance)
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
            Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
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
