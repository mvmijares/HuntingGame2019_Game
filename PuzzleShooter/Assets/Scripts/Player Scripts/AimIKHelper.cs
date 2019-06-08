using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class AimIKHelper : MonoBehaviour
{
    private Player _player;
    [SerializeField] private AimIK aimIK;
    [SerializeField] private Transform _target;
    [SerializeField] private float positionWeightIK;
    public float weightSpeed;
    public void Initialize(Player player)
    {
        if (player)
        {
            _player = player;
            aimIK = GetComponent<AimIK>();
        }
    }

    private void LateUpdate()
    {
        if (!aimIK) return;

        if(_target != null)
        {
            if (_player.playerInput.aim) //if we are aiming
            {
                positionWeightIK += Time.deltaTime * weightSpeed;
                positionWeightIK = Mathf.Clamp01(positionWeightIK);
                //positionWeightIK = 1;
                aimIK.solver.IKPositionWeight = positionWeightIK;
                aimIK.solver.SetIKPosition(_target.position);
              
            }
            else
            {
                positionWeightIK -= Time.deltaTime * weightSpeed;
                positionWeightIK = Mathf.Clamp01(positionWeightIK);
                aimIK.solver.IKPositionWeight = 0;
            }
        }
        else
        {
            positionWeightIK -= Time.deltaTime * weightSpeed;
            positionWeightIK = Mathf.Clamp01(positionWeightIK);

            aimIK.solver.IKPositionWeight = 0;
        }
    }

    public void SetTarget(Transform target)
    {
        if(_target == null)
        _target = target;
    }
}
