using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDispatch : MonoBehaviour
{
    [SerializeField] GameObject[] arrayOfDestination;

    public GameObject GoToNewWaypoint()
    {
        return arrayOfDestination[Random.Range(0, arrayOfDestination.Length - 1)];
    }
}
