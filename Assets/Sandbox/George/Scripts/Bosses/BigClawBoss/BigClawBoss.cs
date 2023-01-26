using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawBoss : BossBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootpoint;
    [SerializeField] private BigClawFeet bigBall;
    public void ShootProjectile(int index)
    {
        Instantiate(projectile, shootpoint.position, shootpoint.rotation);
    }

    public override void PerformSpecialAttack()
    {
        bigBall.gameObject.SetActive(true);
        bigBall.OnLaunch(transform.forward);
    }

}
