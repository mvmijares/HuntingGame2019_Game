using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    FlightPath flightPath;

    OnHealth healthComponent;
    private int _health;
    [SerializeField] private float minDistance; // minimum distance before new path
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] Vector3 targetDestination;
    private Quaternion quaternionFromRotation;
    private Quaternion quaternionToRotation;

    // TODO : Move all mesh stuff to a seperate class
    public Transform meshContainer;
    public List<Material> materials;
    private void Awake()
    {
        flightPath = GameObject.FindObjectOfType<FlightPath>().GetComponent<FlightPath>();
        targetDestination = Vector3.zero;
        healthComponent = GetComponent<OnHealth>();
        
        //TODO : Create a better method of grabbing meshes
    }
    private void Update()
    {
        CheckHealth();
        if (targetDestination == Vector3.zero)
        {
            GetDestination();
        }
        //Move();
        //Rotate();
    }
    private void CheckHealth()
    {
        _health = healthComponent.health;
        if (_health <= 0)
        {
            //Animate the dissolve shader
            foreach (Material m in materials)
            {
                //Change the amount over time
                float alpha = m.GetFloat("Vector1_48B83A3A"); //property name from compiled PBR Shader
                alpha += Time.deltaTime;
                Debug.Log(alpha);
                m.SetFloat("Vector1_48B83A3A", alpha);
            }
        }
    }
    private void GetDestination()
    {
        targetDestination = flightPath.GetNewDestination();
    }
    private void Move()
    {
        float distance = (targetDestination - transform.position).magnitude;
        Vector3 direction = (targetDestination - transform.position) / distance;

        transform.Translate(direction * moveSpeed * Time.deltaTime);
        if((targetDestination - transform.position).magnitude < minDistance)
        {
            targetDestination = Vector3.zero;
        }
    }

    private void Rotate()
    {
        quaternionFromRotation = transform.rotation;
        Vector3 direction = (targetDestination - transform.position).normalized;

        quaternionToRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(quaternionFromRotation, quaternionToRotation, Time.deltaTime * turnSpeed);
    }
}
