using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHealth : MonoBehaviour
{
    [Tooltip("Health prior to game start.")]
    public int initHealth;
    [SerializeField] private int _health;

    public int health { get { return _health; } }
    private void Awake()
    {
    
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
