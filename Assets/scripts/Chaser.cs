/// <summary>
/// Chaser.cs
/// This script is a script for 1 of the 3 ai
/// this controls the grandpa in the antique shop
/// he idles around and walks around his store.
/// </summary>
/// <author> Lee Jia Lu </author>
/// <date> 12/08/2025 </date>
/// <StudentID> S10269187E </StudentID>
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls a NavMeshAgent-based grandpa that patrols between waypoints 
/// and idles between movements. Handles state switching via coroutines.
/// </summary>
public class Chaser : MonoBehaviour
{
    NavMeshAgent myAgent;
    /// <summary>
    /// Tracks the currently running coroutine for state behaviour.
    /// </summary>
    private Coroutine stateCoroutine;

    /// <summary>
    /// The current state of the grandpa like idle and patrol
    /// </summary>
    public string currentState;

    /// <summary>
    /// List of patrol points grandpa is following
    /// Time in seconds the grandpa idles before resuming patrol.
    /// </summary>
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float idleTime = 2f;

    /// <summary>
    /// Animator to controls the animation; idle and walking
    /// Tracks the index of the current patrol point.
    /// </summary>
    private int currentPatrolIndex = 0;
    private Animator animator;

    /// <summary>
    /// Initializes NavMeshAgent, Animator,
    /// and begins the initial state (Patrol or Idle).
    /// </summary>
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myAgent.autoBraking = true;

        if (patrolPoints.Length > 0)
            StartCoroutine(SwitchState("Patrol"));
        else
            StartCoroutine(SwitchState("Idle"));
    }

    /// <summary>
    /// Switches the grandpa to a new state coroutine ("Idle" or "Patrol").
    /// Stops the old state coroutine before starting the new one.
    /// </summary>
    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState)
            yield break;

        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);

        currentState = newState;

        switch (newState)
        {
            case "Idle":
                stateCoroutine = StartCoroutine(Idle());
                break;
            case "Patrol":
                stateCoroutine = StartCoroutine(Patrol());
                break;
        }
    }

    /// <summary>
    /// Idle state: grandpa stops moving, plays idle animation, 
    /// waits for a duration, then switches back to patrol.
    /// </summary>
    IEnumerator Idle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("Idle", true);

        yield return new WaitForSeconds(idleTime);

        yield return StartCoroutine(SwitchState("Patrol"));
    }
    
    /// <summary>
    /// Patrol state: grandpa moves between patrol points in sequence, 
    /// plays walking animation, then switches back to idle.
    /// </summary>
    IEnumerator Patrol()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("isWalking", true);

        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned.");
            yield return StartCoroutine(SwitchState("Idle"));
            yield break;
        }

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        myAgent.SetDestination(targetPoint.position);

        // Wait until the agent reaches the destination
        while (myAgent.pathPending || myAgent.remainingDistance > myAgent.stoppingDistance)
        {
            yield return null;
        }

        // Move to the next point
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        // Switch to idle state
        yield return StartCoroutine(SwitchState("Idle"));
    }
}
