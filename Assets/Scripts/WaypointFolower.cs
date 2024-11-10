using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFolower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    int currentWaypoint =0;
    [SerializeField] private float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, waypoints[currentWaypoint].transform.position)<0.1f)
        {
            currentWaypoint ++;
            currentWaypoint = currentWaypoint % waypoints.Length;
        }

        transform.position=Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
    
    }
}
