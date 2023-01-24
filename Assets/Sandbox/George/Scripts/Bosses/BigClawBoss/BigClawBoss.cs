using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawBoss : BossBase
{
    [SerializeField] private GameObject projectile;
    public void LookAtPlayer()
    {
        Vector3 target = playerPosition.position;
        target.y = 0.0f;
        transform.LookAt(target);
    }

    public void ShootProjectile(int index)
    {
        
    }

}
