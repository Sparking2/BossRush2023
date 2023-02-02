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
    [SerializeField] private Transform shootpoint;
    [SerializeField] private GameObject projectilePrefab;

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
    public override void PerformAction()
    {
        int atk = Random.Range(0, 5);
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
        }
    }

    public void LaunchProjectile()
    {
        Instantiate(projectilePrefab, shootpoint.position, shootpoint.rotation);
    }


    private IEnumerator MeleeAttackPattern()
    {
        state = EntityState.attacking;
        animator.SetTrigger("chase");
        targetPoint = playerTransform.position;

        agent.speed = isBerseker ? chaseSpeed + 15.0f:chaseSpeed;

        agent.SetDestination(targetPoint);
        yield return waitUntilOnTriggerPoint;
        agent.SetDestination(transform.position);
        state = EntityState.attacking;
        animator.SetTrigger("meleeAtk");
        if (isBerseker)
        {

        } 
    }

    private IEnumerator RangedAttackPattern()
    {
        state = EntityState.attacking;
        SetLookComponent(true);
        yield return new WaitForSeconds(.75f);
        SetLookComponent(false);
        animator.SetTrigger("rangedAtk");
    }

    private IEnumerator TackleAttackPattern()
    {
        state = EntityState.attacking;
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
            Debug.Log(Vector3.Distance(transform.position, targetPoint));
            yield return tackleWait;
            agent.SetDestination(transform.position);
            targetPoint = playerTransform.position;
            totalTackles--;
            yield return smallWait;
        }
        componentLookAtTarget.RestoreLookSpeed();
        dangerLineRenderer.enabled = false;
        animator.SetTrigger("tackleEnd");
    }

    private IEnumerator CannonBallAttackPattern()
    {
        state = EntityState.channeling;
        animator.SetTrigger("chargedStart");
        componentLookAtTarget.SetLookSpeed(1.5f);
        onAnimationTransition = false;
        yield return waitUntilAnimationFinish;
        SetLookComponent(true);
        dangerLineRenderer.enabled = true;
        yield return new WaitForSeconds(2.5f);
        animator.SetTrigger("chargedShoot");
        SetLookComponent(false);
        componentLookAtTarget.RestoreLookSpeed();
        dangerLineRenderer.enabled = false;
        bigBall.gameObject.SetActive(true);
        bigBall.OnLaunch(transform.forward);
        yield return new WaitForSeconds(1.5f);
        animator.SetTrigger("chargedEnd");
    }

    public void AnimationTransitionFinished()
    {
        onAnimationTransition = true;
    }
}
