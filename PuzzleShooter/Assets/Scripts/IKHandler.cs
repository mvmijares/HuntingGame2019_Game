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
    public Transform weapon;
    [SerializeField] Transform lookAtTarget; //Position of where we want the IK to aim at
    public float distanceFromCamera = 20; // variable to calculate a Vector3 from camera


    public Transform rightHandTransform;
    [SerializeField] Transform rightHandIKTarget;
    public float rightHandIKWeight;

    public Transform rightElbowTransform;
    [SerializeField] Transform rightElbowIKTarget;
    public float rightElbowIKWeight;
    /// <summary>
    /// Work on later
    /// </summary>
    //public Transform leftHandTransform;
    //[SerializeField] Transform leftHandIKTarget;
    //public float leftHandIKWeight;



    //public Transform leftElbowTransform;
    //[SerializeField] Transform leftElbowIKTarget;
    //public float leftElbowIKWeight;

    Transform bodyBone;
    [SerializeField] Transform bodyIKTarget;
    public float bodyIKWeight;

    public float calculatedAngle;
    public float weightSpeed = 1;

    private float cubeSize = 0.15f;
    private void Start()
    {
        lookAtTarget = new GameObject("Look At Target").transform;
        lookAtTarget.SetParent(transform.parent);
        anim = GetComponent<Animator>();

        bodyBone = anim.GetBoneTransform(HumanBodyBones.Spine);

        //Old Code
        /**
        //rightHandIKTarget = new GameObject("Right Hand IK Target").transform;
        //rightHandIKTarget.position = rightHandTransform.position;

        //rightElbowIKTarget = new GameObject("Right Eblow IK Target").transform;
        //rightElbowIKTarget.position = rightElbowTransform.position;

        //leftHandIKTarget = new GameObject().transform;
        //bodyIKTarget = new GameObject().transform;
        **/ 
    }

    private void Update()
    {
      
        
    }

    private void FixedUpdate()
    {
        lookAtTarget.position = Vector3.Lerp(lookAtTarget.position, cam.lookDirection.GetPoint(distanceFromCamera), Time.deltaTime * 5);
        Vector3 direction = (rightHandTransform.position - lookAtTarget.position).normalized;

        rightHandIKTarget.rotation.SetFromToRotation(rightHandIKTarget.rotation.eulerAngles, direction);
    }
    private void LateUpdate()
    {
        bodyBone.LookAt(lookAtTarget);
    }
    //Old Code
    //private void CalculateLookPosition()
    //{
    //    lookAtTarget.position = Vector3.Lerp(lookAtTarget.position, cam.lookDirection.GetPoint(distanceFromCamera), Time.deltaTime * 5);
    //    

    //    rightHandIKTarget.position = rightHandTransform.position + direction * -1f;

    //    CalculateRotation(direction);
    //}

    //private void CalculateRotation(Vector3 direction)
    //{

    //    Vector3 newDirection = rightHandTransform.rotation.eulerAngles + direction;
    //    newDirection *= -1f;
    //    rightHandTransform.rotation = Quaternion.FromToRotation(rightHandTransform.rotation.eulerAngles, newDirection);

    //    rightElbowTransform.localRotation = Quaternion.FromToRotation(rightHandTransform.rotation.eulerAngles, newDirection);

    //}

    //private void CalculateLookWeight()
    //{
    //    Vector3 directionTowardsTarget = lookAtTarget.position - transform.position;
    //    calculatedAngle = Vector3.Angle(transform.forward, directionTowardsTarget);

    //    targetWeight = (calculatedAngle < 90) ? 1 : 0;

    //    lookWeight = Mathf.Lerp(lookWeight, targetWeight, Time.deltaTime * weightSpeed);

    //}

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {

            //anim.SetLookAtWeight(lookWeight, bodyWeight, headWeight, headWeight, clampWeight);
            //anim.SetLookAtPosition(lookAtTarget.position);

            //if (rightHandIKTarget)
            //{
            //    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
            //    anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(rightHandIKTarget)
            Gizmos.DrawWireCube(rightHandIKTarget.position, new Vector3(cubeSize, cubeSize, cubeSize));
    }
}
