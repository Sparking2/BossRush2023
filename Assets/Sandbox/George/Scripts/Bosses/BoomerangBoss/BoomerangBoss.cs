using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBoss : BossBase
{
    [SerializeField] private float minShootingCooldown;
    [SerializeField] private float maxShootingCooldown;
    [SerializeField] private GameObject bossProjectilePrefab;

    [SerializeField] private ComponentBossShooters[] componentBosses;
    [SerializeField] private Boomerang boomerang;
    [SerializeField] private GameObject visualBoomerang;
    [SerializeField] private Transform boomerangPivot;

    private BossStateMachine m_stateMachine;
    private void Start()
    {
        m_stateMachine = GetComponent<BossStateMachine>();
        foreach (ComponentBossShooters _component in componentBosses)
        {
            _component.SetMinMaxCooldowns(minShootingCooldown, maxShootingCooldown);
            _component.SetProjectile(bossProjectilePrefab);
            _component.SetCanShoot(true);
        }

        boomerang.transform.position = boomerangPivot.transform.position;
        boomerang.SetBoomerangBoss(this);
    }

    public override void PerformSpecialAttack()
    {

    }

    public void LaunchBoomerang()
    {
        visualBoomerang.SetActive(false);
        boomerang.SetBossPosition(boomerangPivot.transform);
        //boomerang.PerformInAndOutAttack(playerPosition.position);
        boomerang.PerformBulletVortexAttack();
    }

    public void OnBoomerangLaunchStart()
    {
        componentBosses[0].SetCanShoot(false);
    }

    public void OnBoomerangLaunchEnd()
    {
        componentBosses[0].SetCanShoot(true);
    }

    public void OnBoomerangReturn()
    {
        visualBoomerang.SetActive(true);
        m_stateMachine.ChangeState(m_stateMachine.idleState);
    }
}
