using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float waypointDistance;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, waypointDistance);
    }

    public Vector3 GetWanderPosition()
    {
        Vector3 point = Random.insideUnitSphere + transform.position;
        return  point * waypointDistance;
    }
}
