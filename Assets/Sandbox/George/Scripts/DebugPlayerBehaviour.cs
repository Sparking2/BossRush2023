using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DebugPlayerBehaviour : MonoBehaviour
{

    private NavMeshAgent agent;
    private Vector3 targetPos;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        targetPos = GetWanderPoint();
        agent.SetDestination(targetPos);
    }

    private void Update()
    {
        if (agent.remainingDistance <= 0.5f)
        {
            targetPos = GetWanderPoint();
            agent.SetDestination(targetPos);
        }
    }

    public Vector3 GetWanderPoint()
    {
        float wanderRadius = 15f;
        Vector3 waypointPosition = WaypointManager.Instance.GetWaypoint();
        Vector3 randomPosition = Random.insideUnitSphere * wanderRadius;
        randomPosition += waypointPosition;
        //randomPosition *= 1.5f;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, wanderRadius, 1);
        return hit.position;

        //Vector3 newPoint = WaypointManager.Instance.GetWaypoint();
        //NavMeshHit hit;
        //NavMesh.SamplePosition(newPoint, out hit, 1.5f, 1);
        //return hit.position;
    }

}
