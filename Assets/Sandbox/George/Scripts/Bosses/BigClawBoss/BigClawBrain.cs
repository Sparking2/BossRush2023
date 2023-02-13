using Ammunition;
using Ammunition.Pool;
using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigClawBrain : EntityBrainBase
{
    [Header("Big claw general settings: "),Space(10)]
    [SerializeField] private LineRenderer dangerLineRenderer;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Transform linePivot;

    [Header("Chase attack settings:"), Space(10)]
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float attackRange;
    [Header("Ranged attack settings: "),Space(10)]
    [SerializeField] private Transform[] shootpoints;
    [SerializeField] private ProjectileType projectilePrefab;
    [SerializeField] private ProjectileType specialProjectilePrefab;

    [Header("Tackle attack settings: "), Space(10)]
    [SerializeField] private float tackleSpeed;
    [SerializeField] private float tackleWaitTime;
    [SerializeField] private float smallWaitTime;
    [SerializeField] private ParticleSystem berserkerTackleParticles;
    [Header("Cannon ball attack settings: "),Space(10)]
    [SerializeField] private bool onAnimationTransition;
    [SerializeField] private BigClawFeet bigBall;


    private ComponentLookAtTarget componentLookAtTarget;

    private WaitUntil waitUntilOnTriggerPoint;
    private WaitUntil waitUntilAnimationFinish;
    private WaitForSeconds tackleWait;
    private WaitForSeconds smallWait;

    [SerializeField] private ComponentLookAtTarget gunComponentLook;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public override void OnAwake()
    {
        waitUntilOnTriggerPoint = new WaitUntil(() => Vector3.Distance(transform.position,targetPoint) <= attackRange);
        waitUntilAnimationFinish = new WaitUntil(() => onAnimationTransition == true);
        tackleWait = new WaitForSeconds(tackleWaitTime);
        smallWait = new WaitForSeconds(smallWaitTime);
        componentLookAtTarget = GetComponent<ComponentLookAtTarget>();
        componentLookAtTarget.SetPlayerTransform(playerTargetTransform);
        componentLookAtTarget.canLookAtTarget = false;
    }

    public override void OnStart()
    {
        dangerLineRenderer.transform.SetParent(null);
        dangerLineRenderer.enabled = false;

        gunComponentLook.SetPlayerTransform(playerTargetTransform);

        animator.Play("Introduction");
    }

    public override void Update()
    {
        base.Update();
        //if (state != EntityState.idle) return;
        //PerformAction();
    }

    public override void EnterBersekerMode()
    {
        base.EnterBersekerMode();
        if (dangerLineRenderer.enabled) dangerLineRenderer.enabled = false;
    }


    public override void OnUpdate()
    {
        SetDangerLine();
    }

    public void SetDangerLine()
    {
        if (state != EntityState.channeling)  return;
        if (dangerLineRenderer.transform.parent) dangerLineRenderer.transform.SetParent(null);
        RaycastHit hit;
        dangerLineRenderer.SetPosition(0, linePivot.position);
        //if (Physics.Raycast(linePivot.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, wallMask))
        //{
        //    dangerLineRenderer.SetPosition(1, hit.point);
        //}
        Vector3 dangerPos = new Vector3(playerTargetTransform.position.x, transform.position.y, playerTargetTransform.position.z);
        dangerLineRenderer.SetPosition(1, dangerPos);
    }
    private void SetLookComponent(bool _state) 
    {
        componentLookAtTarget.canLookAtTarget = _state;
        componentLookAtTarget.lookAtPlayer = _state;
    }

    public void LookAtPlayer() // This methods are used on the animator to have better control on when the entity can look at the player.
    {
        SetLookComponent(true);
    }

    public void StopLooking()
    {
        SetLookComponent(false);
    }
    public override void PerformAction()
    {
        int atk = Random.Range(0, 5);
        //atk = 5;
        
        state = EntityState.attacking;
        switch (atk)
        {
            case 0:
                StartCoroutine(MeleeAttackPattern());
                break;
            case 1:
                StartCoroutine(TackleAttackPattern());
                break;
            case 2:
                StartCoroutine(RangedAttackPattern());
                break;
            case 3:
                StartCoroutine(SecundaryRangedAttack());
                break;
            case 4:
                if(isBerseker) StartCoroutine(CannonBallAttackPattern());
                else StartCoroutine(MeleeAttackPattern());
                break;
        }
    }
    public void LaunchProjectile(ProjectileType _projectileType)
    {
        if (isBerseker)
        {
            foreach(Transform t in shootpoints)
            {
                Projectile bullet = PoolManager.GetPool(_projectileType).Get();
                bullet.transform.SetPositionAndRotation(t.position, Quaternion.LookRotation(t.forward));
                bullet.Fire(t.forward);
            }
        } else
        {
            Projectile bullet = PoolManager.GetPool(_projectileType).Get();
            bullet.transform.SetPositionAndRotation(shootpoints[1].position, Quaternion.LookRotation(shootpoints[1].forward));
            bullet.Fire(shootpoints[1].forward);
        }


        //Projectile bullet = PoolManager.GetPool(_projectileType).Get();
        //bullet.transform.SetPositionAndRotation(shootpoint.position, Quaternion.LookRotation(shootpoint.forward));
        //bullet.Fire(transform.forward);


    }

    //public void LaunchSuperProjectile()
    //{
    //    specialProjectilePrefab

    //}
    private IEnumerator MeleeAttackPattern()
    {
        yield return smallWait;
        animator.SetTrigger("chaseStart");
        agent.speed = isBerseker ? chaseSpeed + 15.0f:chaseSpeed;
        onAnimationTransition = false;
        yield return waitUntilAnimationFinish;
        targetPoint = playerTransform.position;
        agent.SetDestination(targetPoint);
        yield return waitUntilOnTriggerPoint;
        agent.SetDestination(transform.position);
        animator.SetTrigger("chaseAtk");
    }

    private IEnumerator TackleAttackPattern()
    {
        yield return smallWait;
        onAnimationTransition = false;
        animator.Play("TackleStart");
        state = EntityState.channeling;
        yield return smallWait;
        dangerLineRenderer.enabled = true;
        yield return waitUntilAnimationFinish;

        targetPoint = playerPos;
        targetPoint.y = transform.position.y;

        agent.acceleration = 75.0f;
        agent.speed = isBerseker? tackleSpeed + 5.0f:tackleSpeed;

        agent.SetDestination(targetPoint);

        if (isBerseker) berserkerTackleParticles.Play();

        state = EntityState.attacking;
        yield return waitUntilOnTriggerPoint;
        animator.SetTrigger("tackleAtk");
        agent.SetDestination(transform.position);
        dangerLineRenderer.enabled = false;
        berserkerTackleParticles.Stop();
    }

    private IEnumerator RangedAttackPattern()
    {
        yield return smallWait;
        SetLookComponent(true);
        yield return new WaitForSeconds(.75f);
        SetLookComponent(false);
        animator.SetTrigger("rangedAtk1");
    }
    private IEnumerator SecundaryRangedAttack()
    {
        yield return smallWait;
        animator.SetTrigger("rangedAtk2");
        yield return null;
    }

    private IEnumerator CannonBallAttackPattern()
    {
        yield return smallWait;
        state = EntityState.channeling;
        animator.SetTrigger("cannonAtk");
        componentLookAtTarget.SetLookSpeed(15.0f);
        SetLookComponent(true);
        yield return new WaitForSeconds(5.0f);
        bigBall.ReturnToOrigin();
    }

    public void OnFeetReturned()
    {
        animator.SetTrigger("cannonEnd");
    }

    public void LaunchCannonBall()
    {
        bigBall.Launch();
    }

    public void AnimationTransitionFinished()
    {
        onAnimationTransition = true;
    }
}
