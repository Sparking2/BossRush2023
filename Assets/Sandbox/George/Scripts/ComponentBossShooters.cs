using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBossShooters : MonoBehaviour
{
     private bool canShoot;
     private float minShootCooldown = .5f;
     private float maxShootCooldown;
     private float shootCooldown;
     private GameObject bulletPrefab;


    private void Update()
    {
        if (canShoot == false) return;

        if (shootCooldown > 0.0f) shootCooldown -= Time.deltaTime;
        else
        {
            shootCooldown = Random.Range(minShootCooldown, maxShootCooldown);
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }

    public void SetMinMaxCooldowns(float _min, float _max)
    {
        minShootCooldown = _min;
        maxShootCooldown = _max;

        shootCooldown = Random.Range(minShootCooldown, maxShootCooldown);
    }

    public void SetProjectile(GameObject _projectilePrefab)
    {
        bulletPrefab = _projectilePrefab;
    }

    public void SetCanShoot(bool _canShot)
    {
        canShoot = _canShot;
    }

}
