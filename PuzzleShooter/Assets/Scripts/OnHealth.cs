using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHealth : MonoBehaviour
{
    [Tooltip("Health prior to game start.")]
    public int baseHealth;
    private bool _isDead;
    public bool isDead
    {
        get
        {
            return _isDead = (_health <= 0) ? true : false;
        }
    }
    
    [SerializeField] private int _health;

    [HideInInspector]
    public int health { get { return _health; } }

    private void Awake()
    {
        _health = baseHealth;
    }
    public void OnTakeDamage(int value)
    {
        _health -= value;
    }
    public void OnHealUnity(int value)
    {
        _health += value;
    }

}
