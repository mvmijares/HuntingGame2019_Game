using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponDamage;
    public float fireRate;
    private int _maxClipSize;
    private int _clip;
    public int clip { get { return _clip; } }


    private bool reload;
    private Player _player;

    //Information stored in player class
    public Transform firePointTransform;
    private Vector3 firePointOrigin;
    public void SetFirePointOrigin(Vector3 point) { firePointOrigin = point; }

    public float fireDistance;
    private bool isCoroutinePlaying;
    private GameObject groundHitPrefab;
    private GameObject enemyHitPrefab;

    public void Initialize(Player player)
    {
        if (player)
        {
            _player = player;
        }
        groundHitPrefab = Resources.Load<GameObject>("GroundHit_Prefab");

        if (!groundHitPrefab)
            Debug.Log("Miss explosion prefab did not load");
        enemyHitPrefab = Resources.Load<GameObject>("EnemyHit_Prefab");
        if(!enemyHitPrefab)
            Debug.Log("enemy hit explosion prefab did not load");
    }
    private void Update()
    {
        HandleWeaponFire();
    }

    public void SetClipSize(int size)
    {
        _clip = size;
    }
    /// <summary>
    /// Method to handle weapon fire logic
    /// </summary>
    private void HandleWeaponFire()
    {
        if (_player.playerInput.fire)
        {
            if (!isCoroutinePlaying)
            {
                StartCoroutine(FireCoroutine());
                isCoroutinePlaying = true;
            }
        }
        else
        {
            if (isCoroutinePlaying)
            {
                StopCoroutine(FireCoroutine());
                isCoroutinePlaying = false;
            }
        }
    }
    /// <summary>
    /// Coroutine for handling fire rate with weapon firing
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireCoroutine()
    {
        Fire();
        yield return new WaitForSeconds(fireRate);
        reload = true;
        isCoroutinePlaying = false;
    }
    /// <summary>
    /// Collision detection for weapon fire;
    /// </summary>
    private void Fire()
    {
        if (!_player) return;

        if (_clip > 0)
        {
            _clip--;
            RaycastHit hit;
            if (Physics.Raycast(firePointOrigin, _player.cameraController.transform.forward, out hit, fireDistance))
            {
                CheckWeaponCollision(hit);
            }
        }

        if(clip <= 0)
            _player.GetGameManager().PlayerEmptyAmmoClip(); // TODO : Make an event for this
        
    }
    /// <summary>
    /// Function to handle weapon collision functionality
    /// </summary>
    /// <param name="hit">The object that is being checked by raycast</param>
    private void CheckWeaponCollision(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GameObject clone = Instantiate(groundHitPrefab, hit.point, groundHitPrefab.transform.rotation) as GameObject;
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hit.collider.GetComponent<OnHealth>().isDead)
        {
            GameObject clone = Instantiate(enemyHitPrefab, hit.point, enemyHitPrefab.transform.rotation) as GameObject;
        }

        if (hit.collider.GetComponent<OnHealth>() && !hit.collider.GetComponent<OnHealth>().isDead)
        {
            hit.collider.GetComponent<OnHealth>().OnTakeDamage(weaponDamage);
        }
    }

   
}
