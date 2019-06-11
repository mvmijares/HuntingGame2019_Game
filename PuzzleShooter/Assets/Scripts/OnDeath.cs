using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// TODO : Setup a interface class that calls Event Functions
/// </summary>
public class OnDeath : MonoBehaviour
{
    Material mat;
    public void Initialize()
    {
        mat = GetComponent<Material>();
    }
    public void OnDeathEventCalled()
    {

    }
}
