using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossStateMachine : MonoBehaviour
{
    [HideInInspector]
    public BossStatsScriptables bossStats;
    public BossState bossState = BossState.idle;

    public BossIdleState idleState;
    public BossChaseState chaseState;
    public BossChannelState channelState;
    // All attack states should be added to this array. 
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
        bossStats = bossBase.statsScriptables;
        InitializeStats();
        InitializeStates();

        currentState = idleState;
        currentState.OnStateEnter(this);
    }

    private void InitializeStats()
    {
        if (!bossStats)
        {
            Debug.LogWarning("No scriptable found in boss stats");
            return;
        }
        agent.speed = bossStats.idleMovementSpeed;
    }

    public virtual void InitializeStates() // Base for the state machine, each boss will have different conditions to trigger each state.
    {
        idleState = new BossIdleState();
        chaseState = new BossChaseState();
        channelState = new BossChannelState();
       // meleeAttackState = new BossAttackState();
    }

    private void Update()
    {
        if (currentState == null) return;
        if (currentState.doOnFixed) return;
        currentState.OnStateUpdate(this);
    }

    private void FixedUpdate()
    {
        if (currentState == null) return;
        if (!currentState.doOnFixed) return;
        currentState.OnStateUpdate(this);
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
        if(debugPosition) debugPosition.position = hit.position;
        return hit.position;

        //Vector3 newPoint = WaypointManager.Instance.GetWaypoint();
        //NavMeshHit hit;
        //NavMesh.SamplePosition(newPoint, out hit, 1.5f, 1);
        //return hit.position;
    }

    public Vector3 GetTargetPoint(Vector3 _targetPosition)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(_targetPosition, out hit, 3, 1);
        return hit.position;
    }

    public enum BossState { idle,chasing,channeling,attacking,dead }
}
