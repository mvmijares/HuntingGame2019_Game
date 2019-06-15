using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHealth : MonoBehaviour
{
    [Tooltip("Health prior to game start.")]
    public int health;
    private bool _isDead;
    public bool isDead
    {
        get
        {
            return _isDead = (health <= 0) ? true : false;
        }
    }
    
    public void OnTakeDamage(int value)
    {
        health -= value;
    }
    public void OnHealUnity(int value)
    {
        health += value;
    }

}
