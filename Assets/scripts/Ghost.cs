/// <summary>
/// Ghost.cs
/// This script is a script for 1 of the 3 ai
/// this controls the ghost in the second scene
/// it chases you and you die upon touching it.
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 15/08/2025 </date>
/// <StudentID> S10269187E </StudentID>
using UnityEngine;
using UnityEngine.AI;

    /// <summary>
    /// Reference to the player Transform.
    /// Location where the player will be teleported upon collision.
    /// Maximum distance at which the ghost detects and starts chasing the player.
    /// </summary>
public class Ghost : MonoBehaviour
{
    public Transform player;                 
    public Transform teleportDestination;    
    public float detectionRange = 250f;      

    /// <summary>
    /// Reference to the NavMeshAgent used for movement.
    ///  Defines the possible states of the ghost; idle or chase.
    /// The current state of the ghost from default to idle.
    /// </summary>
    private NavMeshAgent agent;
    private enum State { Idle, Chase }
    private State currentState = State.Idle;

    /// <summary>
    /// Initializes the NavMeshAgent reference.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Handles state switching based on the player's
    /// distance from the ghost.
    /// </summary
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                agent.isStopped = true;
                if (distance <= detectionRange)
                {
                    currentState = State.Chase;
                    agent.isStopped = false;
                }
                break;

            case State.Chase:
                if (distance > detectionRange)
                {
                    currentState = State.Idle;
                    agent.isStopped = true;
                }
                else
                {
                    ChasePlayer();
                }
                break;
        }
    }
    
    /// <summary>
    /// Makes the ghost chase the player by setting the NavMeshAgent's destination.
    /// </summary>
    void ChasePlayer()
    {
        agent.stoppingDistance = 0f; // No stopping distance, get close enough to hit
        agent.SetDestination(player.position);
    }

    /// <summary>
    /// If the ghost collides with the player, 
    /// teleport the player to the spawn point.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
        }
    }
    
    /// <summary>
    /// Teleports the player to the specified teleport destination. 
    /// Resets the Rigidbody velocity to prevent unintended physics movement.
    /// </summary>
    private void TeleportPlayer()
    {
        if (player == null || teleportDestination == null) return;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            
            player.transform.position = teleportDestination.position; 
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Debug.Log("Player teleported using Rigidbody.");
        }
        else
        {
            Debug.LogWarning("Player Rigidbody not found. Teleport failed.");
        }
    }
}
