/// <summary>
/// Car.cs
/// This script is a script for 1 of the 3 ai
/// handles the moving car and its rigidbody
/// When player touch it the player moves along with it
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 13/08/2025 </date>
/// <StudentID> S10269187E </StudentID>
using UnityEngine;
using System.Collections.Generic;

/// <summary>
///  Controls a car that patrols between a series of waypoints using Rigidbody movement
/// </summary>

public class Car : MonoBehaviour
{

    /// <summary>
    /// Different possible states the car can be in.
    /// </summary>
    
    public enum CarState { Patrol }
        /// <summary>
        /// The car moves along its assigned waypoints in order.
        /// </summary>
    

    /// <summary>
    /// The current behaviour/state of the car. Defaults to Patrol.
    /// </summary>
    
    public CarState currentState = CarState.Patrol;

    /// <summary>
    /// List of waypoints the car will follow while patrolling.
    /// </summary>


    public List<Transform> waypoints;

    /// <summary>
    /// Movement speed of the car while patrolling.
    /// </summary>
    public float speed = 5f;

    /// <summary>
    /// Tracks which waypoint the car is currently moving toward.
    /// </summary>
    private int currentWaypointIndex = 0;

    /// <summary>
    /// Rigidbody component used to move the car smoothly with physics.
    /// </summary>
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

    /// <summary>
    /// Moves the car between waypoints in sequence, looping back after the last one.
    /// </summary>
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
