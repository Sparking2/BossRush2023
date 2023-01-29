using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStateMachine : MonoBehaviour
{
    public BossStatsScriptables bossStats;
    public BossState bossState = BossState.idle;

    public BossIdleState idleState;
    public BossChaseState chaseState;
    public BossChannelState channelState;
    public BossAttackState meleeAttackState;
    public BossAttackState rangedAttackState;
    public BossAttackState tackleState;
    public BossAttackState chargedAttackState;

    public BossAttackState[] attackStates;

    private BaseState currentState;

    [HideInInspector]
    public NavMeshAgent agent;

    public Transform playerTransform;

    public float attackRange;
    [HideInInspector] public Animator animator;
    [SerializeField] private Transform debugPosition;
    [HideInInspector] public BossBase bossBase;
    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bossBase = GetComponent<BossBase>();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackRange);

        //if (!bossStats) return;
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, bossStats.minWanderRadius);
        //Gizmos.DrawWireSphere(transform.position, bossStats.maxWanderRadius);
    }

    private void Start()
    {
        InitializeStats();
        InitializeStates();

        currentState = idleState;
        currentState.OnStateEnter(this);
    }

    private void InitializeStats()
    {
        agent.speed = bossStats.idleMovementSpeed;
    }

    public virtual void InitializeStates() // Base for the state machine, each boss will have different conditions to trigger each state.
    {
        idleState = new BossIdleState();
        chaseState = new BossChaseState();
        channelState = new BossChannelState();
        meleeAttackState = new BossAttackState();
    }

    private void Update()
    {
        if(currentState != null) currentState.OnStateUpdate(this);
    }

    public void ChangeState(BaseState _nextState)
    {
        currentState.OnStateExit(this);
        currentState = _nextState;
        currentState.OnStateEnter(this);
    }

    public Vector3 GetWanderPoint()
    {
        float wanderRadius = bossStats.GetWanderRadius();
        Vector3 waypointPosition = WaypointManager.Instance.GetWaypoint();
        Vector3 randomPosition = Random.insideUnitSphere * wanderRadius;
        randomPosition += waypointPosition;
        //randomPosition *= 1.5f;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, wanderRadius, 1);
        debugPosition.position = hit.position;
        return hit.position;

        //Vector3 newPoint = WaypointManager.Instance.GetWaypoint();
        //NavMeshHit hit;
        //NavMesh.SamplePosition(newPoint, out hit, 1.5f, 1);
        //return hit.position;
    }

    public enum BossState { idle,chasing,channeling,attacking,dead }
}
