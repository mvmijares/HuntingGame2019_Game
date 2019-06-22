using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO : Setup Base class for objects to inherit from. 
/// Methods for base class
/// - Initialization function
/// - An update function
/// - A late update function
/// </summary>
public class BaseObject : MonoBehaviour
{
    GameManager _gameManager;
    public GameManager GetGameManager() { return _gameManager; }
    /// <summary>
    /// BaseObject custom initialization function
    /// </summary>
    /// <param name="manager"></param>
    public virtual void ObjectInitialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    /// <summary>
    /// BaseObject custom update function
    /// </summary>
    public virtual void CustomUpdate()
    {

    }
    /// <summary>
    /// BaseObject class custom LateUpdate function
    /// </summary>
    public virtual void CustomLateUpdate() { }

}
