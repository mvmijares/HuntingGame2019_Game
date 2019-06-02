using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Returns a position for flight path.
/// </summary>
public class FlightPath : MonoBehaviour
{

    SphereCollider sphereCollider;
    public void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    public Vector3 GetNewDestination()
    {
        Vector3 unitSphere;
        unitSphere = Random.insideUnitSphere * sphereCollider.radius;

        return transform.position + unitSphere;
    }
}
