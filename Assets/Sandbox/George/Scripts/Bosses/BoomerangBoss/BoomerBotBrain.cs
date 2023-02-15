using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerBotBrain : EntityBrainBase
{
    [Header("BoomerangBot settings: "),Space(10)]
    [SerializeField] private float minShootingCooldown;
    [SerializeField] private float maxShootingCooldown;
    [SerializeField] private ProjectileType bossProjectilePrefab;

    [SerializeField] private ComponentBossShooters[] componentBosses;
    [SerializeField] private Boomerang boomerang;
    [SerializeField] private GameObject visualBoomerang;
    [SerializeField] private Transform boomerangPivot;
    public bool hasBoomerang;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform aimHandle;
    [SerializeField] private ComponentLookAtTarget componentLookAtTarget;
    [Header("Movement parameters: "), Space(10)]
    [SerializeField] private float waitOnArrival;
    [SerializeField] private float minSpeed, maxSpeed;
    [SerializeField] private ComponentRotator wheelRotator;
    private float waitTime;

    private Vector3 launchPosition;
    private bool doOnce;

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
        componentLookAtTarget.SetLookSpeed(0f);
        waitTime = waitOnArrival;
        targetPoint = CustomTools.GetRandomPointOnMesh(15f, transform.position);
        animator.Play("Introduction");
    }

    public override IEnumerator MoveToRandomPoint()
    {
        state = EntityState.moving;
        agent.SetDestination(transform.position);
        wheelRotator.SetRotationSpeed(0f);
        yield return new WaitForSeconds(waitTime);
        wheelRotator.SetRotationSpeed(250.0f);
        targetPoint = CustomTools.GetRandomPointOnMesh(25.0f, Vector3.zero);
        agent.speed = Random.Range(minSpeed, maxSpeed);
        agent.SetDestination(targetPoint);
        yield return waitUntilIsOnTarget;
        OnMovementFinished();
    }

    private void OnMovementFinished()
    {
        state = EntityState.idle;
        componentLookAtTarget.SetLookSpeed(25f);
        StartCoroutine(MoveToRandomPoint());
    }

    public void StartWeapons()
    {
        foreach (ComponentBossShooters _component in componentBosses)
        {
            _component.SetCanShoot(true);
        }
    }


    private void SetWeaponStats(float min,float max, ProjectileType projectile)
    {
        foreach (ComponentBossShooters _component in componentBosses)
        {
            _component.SetMinMaxCooldowns(min, max);
            _component.SetProjectile(projectile);
            _component.SetCanShoot(false);
        }
    }

    public override void OnUpdate()
    {
        HandleAiming();
    }

    private void HandleAiming()
    {
        RaycastHit hit;
        Vector3 _direction;
   
        playerPos = new Vector3(playerTransform.position.x, aimHandle.position.y, playerTransform.position.z);
        _direction = playerPos - aimHandle.position;
        if (Physics.Raycast(aimHandle.position, _direction, out hit, Mathf.Infinity, playerMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.DrawRay(aimHandle.position, _direction * hit.distance, Color.green);
            }
            else
            {
                Debug.DrawRay(aimHandle.position, _direction * hit.distance, Color.red);
            }

            foreach (ComponentBossShooters _component in componentBosses)
            {
                _component.playerInSight = hit.transform.CompareTag("Player");
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
                boomerang.PerformInAndOutAttack(playerPos, isBerseker);
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
                boomerang.PerformInAndOutAttack(playerPos, isBerseker);
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
