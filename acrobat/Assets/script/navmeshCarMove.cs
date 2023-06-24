using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmeshCarMove : MonoBehaviour
{
    [SerializeField] GameObject waypoint;
    NavMeshAgent nav;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(waypoint.transform.position);
    }

    void Update()
    {
        if (nav.remainingDistance ==0)
        {
            waypoint = waypoint.GetComponent<WaypointDispatch>().GoToNewWaypoint();
            nav.SetDestination(waypoint.transform.position);
        }
    }
}
