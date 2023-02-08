using Ammunition;
using Ammunition.Pool;
using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentIndependantGun : MonoBehaviour
{
    [SerializeField] private bool gunActive;
    [SerializeField] private bool isBerseker;
    private float shootDelay;
    private float shootCounter;
    [SerializeField] private ProjectileType projectilePrefab;
    [SerializeField] private ProjectileType specialProjectilePrefab;
    public void SetShootSpeed(float _speed)
    {
        shootDelay = _speed;
        shootCounter = shootDelay;
    }

    public void SetGunState(bool _active)
    {
        gunActive = _active;
    }

    public void SetBersekerState(bool _active)
    {
        isBerseker = _active;
    }

    private void Update()
    {
        if (!gunActive) return;
        if (shootCounter > 0.0f) shootCounter -= Time.deltaTime;
        else
        {
            shootCounter = shootDelay;
            if (isBerseker) ShootProjectile(projectilePrefab);
            else ShootProjectile(specialProjectilePrefab);
        }
    }


    private void ShootProjectile(ProjectileType _projectileType)
    {
        Projectile bullet = PoolManager.GetPool(_projectileType).Get();
        bullet.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(transform.forward));
        bullet.Fire(transform.forward);
    }
}


