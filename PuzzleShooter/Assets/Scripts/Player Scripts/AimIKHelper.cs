using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class AimIKHelper : MonoBehaviour
{
    private Player _player;
    [SerializeField] private AimIK rightArmAimIK;
    [SerializeField] private AimIK leftArmAimIK;
    [SerializeField] private AimIK spineAimIK;
    [SerializeField] private Transform _target;
    [SerializeField] private float positionWeightIK;
    public bool disableIK;
    public float weightSpeed;

    public void Initialize(Player player)
    {
        if (player)
        {
            _player = player;

            if (transform.Find("Right Arm IK"))
                rightArmAimIK = transform.Find("Right Arm IK").GetComponent<AimIK>();

            if (transform.Find("Left Arm IK"))
                leftArmAimIK = transform.Find("Left Arm IK").GetComponent<AimIK>();

            if (transform.Find("Spine IK"))
                spineAimIK = transform.Find("Spine IK").GetComponent<AimIK>();
        }
    }

    public void CustomLateUpdate()
    {
        if (!rightArmAimIK || !leftArmAimIK || !spineAimIK)
            return;
        if (disableIK)
            return;

        if(_target != null)
        {
            if (_player.playerInput.aim) //if we are aiming
            {
                positionWeightIK += Time.deltaTime * weightSpeed;
                positionWeightIK = Mathf.Clamp01(positionWeightIK);
                SetPositionWeight(positionWeightIK);
                SetIKPosition(_target.position);
            }
            else
            {
                positionWeightIK -= Time.deltaTime * weightSpeed;
                positionWeightIK = Mathf.Clamp01(positionWeightIK);
                SetPositionWeight(0);
            }
        }
        else
        {
            positionWeightIK -= Time.deltaTime * weightSpeed;
            positionWeightIK = Mathf.Clamp01(positionWeightIK);
            SetPositionWeight(0);
        }
    }
    private void SetPositionWeight(float weight)
    {
        rightArmAimIK.solver.IKPositionWeight = weight;
        leftArmAimIK.solver.IKPositionWeight = weight;
        spineAimIK.solver.IKPositionWeight = weight;
    }
    private void SetIKPosition(Vector3 position)
    {
        rightArmAimIK.solver.SetIKPosition(position);
        leftArmAimIK.solver.SetIKPosition(position);
        spineAimIK.solver.SetIKPosition(position);
    }
    public void SetTarget(Transform target)
    {
        if(_target == null)
            _target = target;
    }
}
