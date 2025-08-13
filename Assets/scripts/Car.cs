using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 5f;

    private int currentWaypointIndex = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned for car patrol.");
        }
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        // Move towards the current waypoint
        Vector3 targetPos = waypoints[currentWaypointIndex].position;
        Vector3 direction = (targetPos - transform.position).normalized;

        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(direction);

        // If close enough, switch to the next waypoint
        if (Vector3.Distance(transform.position, targetPos) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
