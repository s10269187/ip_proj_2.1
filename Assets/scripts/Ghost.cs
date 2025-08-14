using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Transform player;                 
    public Transform teleportDestination;    
    public float detectionRange = 250f;      

    private NavMeshAgent agent;
    private enum State { Idle, Chase }
    private State currentState = State.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

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

    void ChasePlayer()
    {
        agent.stoppingDistance = 0f; // No stopping distance, get close enough to hit
        agent.SetDestination(player.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
        }
    }

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
