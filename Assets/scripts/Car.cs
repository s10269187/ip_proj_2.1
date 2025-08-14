using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour
{
    public enum CarState { Patrol }
    public CarState currentState = CarState.Patrol;

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
        switch (currentState)
        {
            case CarState.Patrol:
                Patrol();
                break;
        }
    }

    void Patrol()
    {
        if (waypoints.Count == 0) return;

        Vector3 targetPos = waypoints[currentWaypointIndex].position;
        Vector3 direction = (targetPos - transform.position).normalized;

        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(direction);

        if (Vector3.Distance(transform.position, targetPos) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
