using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollowerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 1.0f;
    private int currentWaypoint = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(this.transform.position, waypoints[currentWaypoint].transform.position);
        if (distance < 0.1f)
        {
            currentWaypoint = nextWaypoint(currentWaypoint);
        }
        this.transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
    }

    private int nextWaypoint(int currenctWaypoint)
    {
        return (currentWaypoint + 1) % waypoints.Length;
    }

}
