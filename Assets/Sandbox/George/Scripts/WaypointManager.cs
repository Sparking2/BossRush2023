using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    [SerializeField]
    private List<Transform> waypoints;
    [SerializeField] private Transform lastWaypoint;
    private void Awake()
    {
        Instance = this;

    }

    public Vector3 GetWaypoint()
    {
        Transform _waypoint = waypoints[Random.Range(0, waypoints.Count)];

        if(_waypoint == lastWaypoint)
        {
            return transform.position;
        }
        lastWaypoint = _waypoint;


        return _waypoint.position;
    }
}
