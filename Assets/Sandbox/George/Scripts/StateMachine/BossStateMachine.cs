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
    public BossAttackState attackState;

    private BaseState currentState;

    [HideInInspector]
    public NavMeshAgent agent;

    public Transform playerTransform;

    public float attackRange;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
        agent.speed = bossStats.movementSpeed;
    }

    public virtual void InitializeStates() // Base for the state machine, each boss will have different conditions to trigger each state.
    {
        idleState = new BossIdleState();
        chaseState = new BossChaseState();
        channelState = new BossChannelState();
        attackState = new BossAttackState();
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

        Vector3 newPoint = WaypointManager.Instance.GetWaypoint();
       // newPoint += transform.position;
       //float WaypointRadius = WaypointManager.Instance.
        NavMeshHit hit;
        NavMesh.SamplePosition(newPoint, out hit, 1.5f, 1);
      //  NavMesh.CalculatePath(transform.position,hit,1);
        return hit.position;
    }

    public enum BossState { idle,chasing,channeling,attacking,dead }
}
