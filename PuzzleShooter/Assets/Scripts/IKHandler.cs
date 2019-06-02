using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandler : MonoBehaviour
{
    public CameraController cam; // TODO: Setup a way to grab player camera more efficiently
    private Animator anim;
    public float lookWeight;
    public float bodyWeight;
    public float headWeight;
    public float clampWeight;

    float targetWeight;

    public Transform rightShoulder;
    public Transform rightHandIKTarget;
    public float rightHandIKWeight;

    public Transform leftHandIKTarget;
    public float leftHandIKWeight;

    [SerializeField] Transform lookAtTarget; //Position of where we want the IK to aim at
    public float distanceFromCamera = 20; // variable to calculate a Vector3 from camera

    public float calculatedAngle;
    public float weightSpeed = 1;

    private void Start()
    {
        lookAtTarget = new GameObject().transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

        CalculateLookWeight();
        CalculateShoulderRotation();
    }

    private void CalculateShoulderRotation()
    {
        lookAtTarget.position = Vector3.Lerp(lookAtTarget.position, cam.lookDirection.GetPoint(distanceFromCamera), Time.deltaTime * 5);
        rightShoulder.LookAt(lookAtTarget);
        Debug.Log("Called");
    }

    private void CalculateLookWeight()
    {
        Vector3 directionTowardsTarget = lookAtTarget.position - transform.position;
        calculatedAngle = Vector3.Angle(transform.forward, directionTowardsTarget);

        targetWeight = (calculatedAngle < 90) ? 1 : 0;

        lookWeight = Mathf.Lerp(lookWeight, targetWeight, Time.deltaTime * weightSpeed);

        rightHandIKWeight = lookWeight;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //anim.SetLookAtWeight(lookWeight, bodyWeight, headWeight, headWeight, clampWeight);
        Debug.Log("Being called");
        //anim.SetLookAtPosition(cam.lookDirection.GetPoint(distanceFromCamera));
        if (rightHandIKTarget)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
        }
    }
}
