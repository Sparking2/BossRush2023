using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public static class CustomTools
{
    public static Vector3 GetRandomPointOnMesh(float _wanderRadius,Vector3 initialPosition)
    {
        Vector3 randomPosition = Random.insideUnitSphere * _wanderRadius;
        randomPosition += initialPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, _wanderRadius, 1);
        return hit.position;
    }
}
