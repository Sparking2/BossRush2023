using Ammunition;
using Ammunition.Pool;
using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBossShooters : MonoBehaviour
{

     private bool canShoot;
     private float minShootCooldown = .5f;
     private float maxShootCooldown;
     private float shootCooldown;
     public bool playerInSight;
     private ProjectileType boomerBotProjectile;


    private void Update()
    {
        if (canShoot == false) return;

        HandleShoot();
    }

    private void HandleShoot()
    {
        if (shootCooldown > 0.0f) shootCooldown -= Time.deltaTime;
        else
        {
            if (!playerInSight) return;
            shootCooldown = Random.Range(minShootCooldown, maxShootCooldown);
            //Instantiate(bulletPrefab, transform.position, transform.rotation);
 
            Projectile bullet = PoolManager.GetPool(boomerBotProjectile).Get();
            bullet.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(transform.forward));
            bullet.Fire(transform.forward);
        }
    }

    public void SetMinMaxCooldowns(float _min, float _max)
    {
        minShootCooldown = _min;
        maxShootCooldown = _max;

        shootCooldown = Random.Range(minShootCooldown, maxShootCooldown);
    }

    public void SetProjectile(ProjectileType _projectilePrefab)
    {
        boomerBotProjectile = _projectilePrefab;
    }

    public void SetCanShoot(bool _canShot)
    {
        canShoot = _canShot;
    }

}
