using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerBotBrain : EntityBrainBase
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
    private float waitTime;
    private Vector3 launchPosition;
    public override void OnAwake()
    {
        
    }

    public override void OnStart()
    {
        SetWeaponStats(minShootingCooldown, maxShootingCooldown, bossProjectilePrefab);
        hasBoomerang = true;
        boomerang.transform.position = boomerangPivot.transform.position;
        boomerang.SetBoomerangBoss(this);
        boomerang.SetBersekerMode(isBerseker);
        componentLookAtTarget.SetPlayerTransform(playerTransform);
        waitTime = bossStats.GetWaitBeforeWander(isBerseker);
    }

    private void SetWeaponStats(float min,float max,GameObject projectile)
    {
        foreach (ComponentBossShooters _component in componentBosses)
        {
            _component.SetMinMaxCooldowns(min, max);
            _component.SetProjectile(projectile);
            _component.SetCanShoot(true);
        }
    }

    public override void OnUpdate()
    {
       if(agent.remainingDistance <= 0.3f)
        {
            if (waitTime > 0.0f) waitTime -= Time.deltaTime;
            else
            {
                targetPoint = CustomTools.GetRandomPointOnMesh(15f, transform.position);
                agent.SetDestination(targetPoint);
                waitTime = bossStats.GetWaitBeforeWander(isBerseker);
            }
 
        }
    }

    public override void PerformAction()
    {
        state = EntityState.attacking;
        animator.SetTrigger("launch");
        canDoAction = false;
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
                boomerang.PerformInAndOutAttack(playerTransform.position, isBerseker);
                break;
            case 1:
                boomerang.PerformBulletVortexAttack(isBerseker);
                break;
            case 2:
                boomerang.PerformExplosiveAttack(playerTransform);
                break;
            case 3:
                boomerang.PerformLaserAttack(launchPosition, isBerseker);
                break;
            default:
                boomerang.PerformInAndOutAttack(playerTransform.position, isBerseker);
                break;
        }
    }

    private int atk;

    private static Vector3 GetRandomLaunchPosition()
    {
        Vector3 launchPlace = Random.insideUnitSphere * 5.0f;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(launchPlace, out hit, 5.0f, 1);
        Vector3 finalPosition = hit.position;
        finalPosition.y = 1.25f;
        return finalPosition;
    }

    public void OnBoomerangLaunchStart()
    {
        atk = Random.Range(0, 4);
        
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
        OnActionFinished();
        // m_stateMachine.ChangeState(m_stateMachine.idleState);
    }

    public override void EnterBersekerMode()
    {
        base.EnterBersekerMode();
        SetWeaponStats(minShootingCooldown/2, maxShootingCooldown/2, bossProjectilePrefab);

    }
}
