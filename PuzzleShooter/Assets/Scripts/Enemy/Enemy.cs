using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    Idle, Walk, Run
};

public class Enemy : MonoBehaviour
{
    private GameManager _gameManager;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Tick()
    {
            
    }
    public void LateTick()
    {

    }
    private void ApplyInput()
    {

    }
    private void Movement()
    {

    }
    
}
