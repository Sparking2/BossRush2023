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
    [Header("Cannon ball attack settings: "),Space(10)]
    [SerializeField] private bool onAnimationTransition;
    [SerializeField] private BigClawFeet bigBall;


    private ComponentLookAtTarget componentLookAtTarget;

    private WaitUntil waitUntilOnTriggerPoint;
    private WaitUntil waitUntilAnimationFinish;
    private WaitForSeconds tackleWait;
    private WaitForSeconds smallWait;

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
        componentLookAtTarget.SetPlayerTransform(playerTransform);
        componentLookAtTarget.canLookAtTarget = false;
    }

    public override void OnStart()
    {
        dangerLineRenderer.transform.SetParent(null);
        dangerLineRenderer.enabled = false;
        
    }

    public override void OnUpdate()
    {
        SetDangerLine();
    }

    public void SetDangerLine()
    {
        if (state != EntityState.channeling) return;
        if (dangerLineRenderer.transform.parent) dangerLineRenderer.transform.SetParent(null);
        RaycastHit hit;
        dangerLineRenderer.SetPosition(0, linePivot.position);
        if (Physics.Raycast(linePivot.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, wallMask))
        {
            dangerLineRenderer.SetPosition(1, hit.point);
        }
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
        int atk = Random.Range(0, 6);
        atk = 5;
        switch (atk)
        {
            case 0:
                StartCoroutine(MeleeAttackPattern());
                break;
            case 1:
                StartCoroutine(RangedAttackPattern());
                break;
            case 2:
                StartCoroutine(TackleAttackPattern());
                break;
            case 3:
                StartCoroutine(CannonBallAttackPattern());
                break;
            case 4:
                StartCoroutine(MoveToRandomPoint());
                break;
            case 5:
                StartCoroutine(SecundaryRangedAttack());
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
            bullet.Fire(transform.forward);
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
        state = EntityState.attacking;
        animator.SetTrigger("chaseStart");
        agent.speed = isBerseker ? chaseSpeed + 15.0f:chaseSpeed;
        onAnimationTransition = false;
        yield return waitUntilAnimationFinish;
        targetPoint = playerTransform.position;
        agent.SetDestination(targetPoint);
        yield return waitUntilOnTriggerPoint;
        agent.SetDestination(transform.position);
        state = EntityState.attacking;
        animator.SetTrigger("chaseAtk");
        if (isBerseker)
        {

        } 
    }

    private IEnumerator SecundaryRangedAttack()
    {
        state = EntityState.attacking;

        animator.SetTrigger("rangedAtk2");
        yield return null;
    }

    private IEnumerator RangedAttackPattern()
    {
        state = EntityState.attacking;
        SetLookComponent(true);
        yield return new WaitForSeconds(.75f);
        SetLookComponent(false);
        animator.SetTrigger("rangedAtk1");
    }

    private IEnumerator TackleAttackPattern()
    {
        state = EntityState.attacking;
        animator.SetBool("tackleEnd", false);
        animator.SetTrigger("tackleStart");

        agent.speed = tackleSpeed;
        agent.acceleration = 75f;
        yield return smallWait;
        int totalTackles = 3;
        dangerLineRenderer.enabled = true;
        componentLookAtTarget.SetLookSpeed(15.0f);
        while (totalTackles > 0)
        {
            SetLookComponent(true);
            state = EntityState.channeling;
            // Channel particles and line renderer effect here
            yield return tackleWait;
            SetLookComponent(false);
            targetPoint = playerTransform.position;
            agent.SetDestination(targetPoint);
            state = EntityState.attacking;
            yield return waitUntilOnTriggerPoint;
            agent.SetDestination(transform.position);
            animator.SetTrigger("tackleAtk");
            targetPoint = playerTransform.position;
            totalTackles--;
            yield return smallWait;
        }
        componentLookAtTarget.RestoreLookSpeed();
        dangerLineRenderer.enabled = false;
        animator.SetBool("tackleEnd",true);
        OnActionFinished();
    }

    private IEnumerator CannonBallAttackPattern()
    {
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
