using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EntityBrainBase : MonoBehaviour
{
    public BossStatsScriptables bossStats;
    public bool isBerseker;
    [SerializeField] private float restingTime;
    [SerializeField] private bool hasMovingAnimation;
    [HideInInspector] public bool canDoAction = true;
    private float originalAcceleration;
    public EntityState state = EntityState.idle;
    private float thinkTimer;

    [HideInInspector] public Vector3 targetPoint;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Vector3 playerPos;
    public WaitUntil waitUntilIsOnTarget;

    private ComponentHealth componentHealth;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        componentHealth = GetComponent<ComponentHealth>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (bossStats) componentHealth.SetHealth(bossStats.maxHealth,this);


        waitUntilIsOnTarget = new WaitUntil(() => HandleTargetCheck(targetPoint));

        OnAwake();
    }

    private void Start()
    {
        thinkTimer = bossStats.idleTime;
        targetPoint = playerTransform.position;
        originalAcceleration = agent.acceleration;
        canDoAction = true;
        SetBaseAgentSettings();
        OnStart();
    }

    public void SetBaseAgentSettings()
    {
        if (!bossStats) return;
        agent.speed = bossStats.idleMovementSpeed;
        agent.acceleration = originalAcceleration;
    }

    private void Update()
    {
        playerPos = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.5f, playerTransform.position.z);
        OnUpdate();
        if (state != EntityState.idle) return;
        if (thinkTimer > 0f) thinkTimer -= Time.deltaTime;
        else 
        {
            if (canDoAction) PerformAction();
            else
            {
                StartCoroutine(MoveToRandomPoint());
                thinkTimer = bossStats.GetWaitBeforeWander(isBerseker);
            }
        } 
    }

    public void OnActionFinished()
    {
        state = EntityState.idle;
        thinkTimer = isBerseker? .5f: bossStats.idleTime;
        canDoAction = false;
        SetBaseAgentSettings();
        Invoke("RestoreAction", isBerseker? restingTime/2:restingTime);
    }

    private void RestoreAction()
    {
        canDoAction = true;
    }
 
    public IEnumerator MoveToRandomPoint()
    {
        state = EntityState.moving;
        targetPoint = CustomTools.GetRandomPointOnMesh(bossStats.GetWanderRadius(),Vector3.zero);
        if (hasMovingAnimation) animator.SetBool("isMoving", true);
        agent.SetDestination(targetPoint);
        yield return waitUntilIsOnTarget;
        animator.SetBool("isMoving", false);
        state = EntityState.idle;
        thinkTimer = bossStats.idleTime / 2;
    }

    private bool HandleTargetCheck(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= 0.5;
    }

    public virtual void EnterBersekerMode()
    {
        if (isBerseker) return;
        isBerseker = true;
        restingTime /= 2;
    }

    #region Abstracts methods
    public abstract void OnAwake();
    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void PerformAction();    // Does an action, can be moving, channeling, attacking, all of this is made in the actuall entity boss script 

    #endregion
    
    public enum EntityState { idle, channeling, attacking,moving}
}