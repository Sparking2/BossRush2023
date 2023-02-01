using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BoomerangBoss : BossBase
{
    [SerializeField] private float minShootingCooldown;
    [SerializeField] private float maxShootingCooldown;
    [SerializeField] private GameObject bossProjectilePrefab;

    [SerializeField] private ComponentBossShooters[] componentBosses;
    [SerializeField] private Boomerang boomerang;
    [SerializeField] private GameObject visualBoomerang;
    [SerializeField] private Transform boomerangPivot;
    public bool hasBoomerang;

    [SerializeField] private ComponentLookAtTarget componentLookAtTarget;
    private Vector3 launchPosition;
    private void Start()
    {

        foreach (ComponentBossShooters _component in componentBosses)
        {
            _component.SetMinMaxCooldowns(minShootingCooldown, maxShootingCooldown);
            _component.SetProjectile(bossProjectilePrefab);
            _component.SetCanShoot(true);
        }
        hasBoomerang = true;
        boomerang.transform.position = boomerangPivot.transform.position;
        boomerang.SetBoomerangBoss(this);
        boomerang.SetBersekerMode(isBerserker);
        componentLookAtTarget.SetPlayerTransform(playerPosition);
    }

    public override void PerformSpecialAttack()
    {

    }

    public void LaunchBoomerang()
    {

        hasBoomerang = false;
        visualBoomerang.SetActive(false);
        boomerang.SetBossPosition(boomerangPivot.transform);
        boomerang.OnBoomerangLaunch();
        
        switch (atk)
        {
            case 0:
                boomerang.PerformInAndOutAttack(playerPosition.position,isBerserker);
                break;
            case 1:
                boomerang.PerformBulletVortexAttack(isBerserker);
                break;
            case 2:
                boomerang.PerformExplosiveAttack(playerPosition);
                break;
            case 3:
                boomerang.PerformLaserAttack(launchPosition,isBerserker);
                break;
            default:
                boomerang.PerformInAndOutAttack(playerPosition.position,isBerserker);
                break;
        }
    }

    private int atk;

    private static Vector3 GetRandomLaunchPosition()
    {
        Vector3 launchPlace = Random.insideUnitSphere * 5.0f;
        NavMeshHit hit;
        NavMesh.SamplePosition(launchPlace, out hit, 5.0f, 1);
        Vector3 finalPosition = hit.position;
        finalPosition.y = 1.25f;
        return finalPosition;
    }

    public void OnBoomerangLaunchStart()
    {
        atk = Random.Range(0, 4);
        //atk = 3;
        switch (atk)
        {
            case 1:
                componentLookAtTarget.SetTemporalTarget(Vector3.zero);
                break;
            case 3:
                launchPosition = GetRandomLaunchPosition();
                componentLookAtTarget.SetTemporalTarget(launchPosition);
                break;
        }
        componentBosses[0].SetCanShoot(false);
    }

    public void OnBoomerangLaunchEnd()
    {
        componentBosses[0].SetCanShoot(true);
    }

    public void OnBoomerangReturn()
    {
        hasBoomerang = true;
        visualBoomerang.SetActive(true);
       // m_stateMachine.ChangeState(m_stateMachine.idleState);
    }
}
