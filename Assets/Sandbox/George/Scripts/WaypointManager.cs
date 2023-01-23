using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    [SerializeField]
    private List<Waypoint> waypoints;

    private void Awake()
    {
        Instance = this;
        foreach (Waypoint child in transform.GetComponentsInChildren<Waypoint>())
        {
            waypoints.Add(child);
        }
    }

    private void Start()
    {

    }

    public Vector3 GetWaypoint()
    {
        return waypoints[Random.Range(0, waypoints.Count)].GetWanderPosition();
    }
}
