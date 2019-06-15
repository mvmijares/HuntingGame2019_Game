using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponDamage;
    public float fireRate;
    public float maxAmmo;
    public float clip;

    private Player _player;

    //Information stored in player class
    [SerializeField] private Transform firePoint;
    public float fireDistance;
    private bool isCoroutinePlaying;
    
    public void Initialize(Player player)
    {
        if (player)
        {
            _player = player;
        }
    }
    private void Update()
    {
        HandleWeaponFire();
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
        isCoroutinePlaying = false;
    }
    /// <summary>
    /// Collision detection for weapon fire;
    /// </summary>
    private void Fire()
    {
        if (!_player) return;

        RaycastHit hit;
        Vector3 direction = _player.cameraController.lookDirection.direction;
        Debug.DrawRay(firePoint.position, direction * fireDistance, Color.red, 2.0f);
        //TODO : Rework a more efficient way of handling physics detection
        if (Physics.Raycast(firePoint.position, direction, out hit, fireDistance))
        {
            Debug.Log("We hit " + hit.collider.gameObject);
            if (hit.collider.GetComponent<OnHealth>() && !hit.collider.GetComponent<Player>())
            {
                hit.collider.GetComponent<OnHealth>().OnTakeDamage(weaponDamage);
            }
        }
    }
}
